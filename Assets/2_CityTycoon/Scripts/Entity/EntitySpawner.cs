using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CityTycoon
{
    public class EntitySpawner : MonoBehaviour
    {
        [Header("References")]
        public GameObject spwnerParent;
        [SerializeField] private GameObject[] entityPrefabs;

        [Header("Attributes")]
        [SerializeField] private int baseEntity = 8; //จำนวน Enemies
        [SerializeField] private float entityPerSecond = 0.5f; //ระยะเวลาการเกิด
        private float timeBetweenWaves = 0f; //คลูดาวน์เวฟ
        private float difficultyScalingFactor = 0f; //ศูตรูจะเพิ่มขึ้นเรื่อยๆ
        private float entityPerSecondCap = 1f;

        [Header("Events")]
        public static UnityEvent onEntityDestory = new UnityEvent();

        private int currentWave = 1;
        private float timeSinceLastSpawn = 0;
        private int entityAlive;
        private int entityLeftToSpawn;
        private float eps; //enemies per secode;
        private bool isSpawning = false;

        private void Awake()
        {
            onEntityDestory.AddListener(EntityDestoryed);
        }

        public void Start()
        {
            //BuildManager.main.Init();
            StartCoroutine(StartWave());
        }

        public void Update()
        {
            if (!isSpawning) return;

            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= (1f / eps) && entityLeftToSpawn > 0)
            {
                SpwanEntity();
                entityLeftToSpawn--;
                entityAlive++;
                timeSinceLastSpawn = 0f;
                EndWave();
            }
            /*
            if (entityAlive == 0 && entityLeftToSpawn == 0)
            {
                EndWave();
            }*/
        }
        private void EntityDestoryed()
        {
            entityAlive--;
        }

        private IEnumerator StartWave()
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            isSpawning = true;
            entityLeftToSpawn = EntityPerWave();
            eps = EntityPerSecond();
        }

        private void EndWave()
        {
            isSpawning = false;
            timeSinceLastSpawn = 0f;
            currentWave++;
            StartCoroutine(StartWave());
        }

        private void SpwanEntity()
        {
            int rand = Random.Range(0, entityPrefabs.Length);
            GameObject prefabToSpwan = entityPrefabs[rand];
            GameObject go = Instantiate(prefabToSpwan, GameManager.Instance.EntityManager().startPoint.position, Quaternion.identity);
            go.transform.SetParent(spwnerParent.transform);
        }

        private int EntityPerWave()
        {
            //ศูตรูจะเพิ่มขึ้นเรื่อยๆ
            return Mathf.RoundToInt(baseEntity * Mathf.Pow(currentWave, difficultyScalingFactor));
        }

        private float EntityPerSecond()
        {
            //ศูตรูจะเพิ่มขึ้นเรื่อยๆ
            return Mathf.Clamp(entityPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0f, entityPerSecondCap);
        }

    }
}

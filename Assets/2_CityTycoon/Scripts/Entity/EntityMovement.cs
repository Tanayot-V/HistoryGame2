using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CityTycoon
{
    public class EntityMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        [Header("Attributes")]
        [SerializeField] private float moveSpeed = 2f;

        private Transform target;
        private int pathIndex = 0;

        private float baseSpeed;
        private bool isPaused = false;

        [Header("Events")]
        public static UnityEvent onEnemyDestory = new UnityEvent();
        public static UnityEvent onIntermediatePathStop = new UnityEvent();

        private void Start()
        {
            baseSpeed = moveSpeed;
            target = GameManager.Instance.EntityManager().path[pathIndex];
        }

        private void Update()
        {
            if (isPaused) return;
            if (Vector2.Distance(target.position, transform.position) <= 0.1f)
            {
                pathIndex++;
                if (pathIndex == GameManager.Instance.EntityManager().path.Length)
                {
                    EntitySpawner.onEntityDestory.Invoke();
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    target = GameManager.Instance.EntityManager().path[pathIndex];
                    onIntermediatePathStop.Invoke();
                    StartCoroutine(StopAndChangeColor());
                }
            }

            IEnumerator StopAndChangeColor()
            {
                isPaused = true;
                rb.velocity = Vector2.zero;
                //this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
                yield return new WaitForSeconds(2f);
                //this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                isPaused = false;
            }
        }

        private void FixedUpdate()
        {
            if (isPaused)
            {
                rb.velocity = Vector2.zero; // Ensure the entity is completely stopped
                return;
            }

            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }

        public void UpdateSpeed(float newSpeed)
        {
            moveSpeed = newSpeed;
        }

        public void ResetSpeed()
        {
            moveSpeed = baseSpeed;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CityTycoon
{
    public class BCTime : MonoBehaviour //Base
    {
        [SerializeField] Currency currency;
        [SerializeField] PeopleController peopleController;
        [SerializeField] TMPro.TextMeshProUGUI bcText;

        [SerializeField] float currentTime = 7000f;
        [SerializeField] float speedUP = 4f;
        private float baseSpeedUp;
        private bool isUpgraded = false;
        private bool isPaused = false;

        public float spawnTimer = 0f;
        public float deathTimer = 0f;


        public void Init(float _currentTime)
        {
            currentTime = _currentTime;
            peopleController.peopleInven = currency.GetInventoryCost(CostType.PEOPLE);
            peopleController.bcTimeInven = currency.GetInventoryCost(CostType.BC);
            bcText = peopleController.bcTimeInven.text;
        }

        private void Start()
        {
            baseSpeedUp = speedUP;
        }

        void Update()
        {

            /*
 * if (Input.GetKeyDown(KeyCode.P))
{
    TogglePause();
}
if (Input.GetKeyDown(KeyCode.U))
{
    UpgradeTime(100);
}*/
            /*
            if (bcText != null) bcText.text = currentTime.ToString("f0");
            if (!isPaused)
            {
                if (currentTime > 0)
                {
                    currentTime -= speedUP * Time.deltaTime;
                }
                else currentTime = 0;

                spawnTimer += Time.deltaTime;
                if (spawnTimer >= 3f)
                {
                    peopleController.SpawnPeople();
                    spawnTimer = 0f;
                }

                deathTimer += Time.deltaTime;
                if (deathTimer >= 5f)
                {
                    peopleController.KillPeople();
                    deathTimer = 0f; 
                }
            }
            */                              

        }

        void UpgradeTime(float _speedUP)
        {
            if (!isUpgraded)
            {
                speedUP = _speedUP; 
                isUpgraded = true; 
                Debug.Log("Upgrade triggered! Decrement per frame increased.");
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            Debug.Log(isPaused ? "Timer paused." : "Timer resumed.");
        }

        public void UpdateSpeedUpTime(float _speed)
        {
            speedUP = _speed;
        }

        public void ResetSpeedUpTime()
        {
            speedUP = baseSpeedUp;
        }
    }
}

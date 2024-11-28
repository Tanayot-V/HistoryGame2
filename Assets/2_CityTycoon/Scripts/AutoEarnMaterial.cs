using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;
using static CityTycoon.BuildingBase;

namespace CityTycoon
{
    //สคลิปนี้ไม่ได้ใช้แล้ว
    public class AutoEarnMaterial : MonoBehaviour
    {
        public bool isSpawning = false;
        private GameObject earnObj;
        private Earn currentEarnClick;
        public GameObject earnPrefab;
        public GameObject earnParent;
        private BuildingBaseState state;
        private Currency currency;
        private Coroutine coroutine;
        private float timeRemaining;

        private void SetInstance()
        {
            if(currency == null) currency = GameManager.Instance.Currency();
        }

        public void Update() {}

        //ใช้ตอนเริ่ม
        public void StartEarn(BuildBaseObj _buildBaseObj)
        {
            state = _buildBaseObj.state;
            SetInstance();
            if (state.level <= 0) return;
            else
            {
                //ถ้ายังสปอนอยู่
                if (!isSpawning)
                {
                    //ใช้ตอนอัพเกรดแล้วยังไม่ได้เก็บของ
                    currentEarnClick = _buildBaseObj.upgradesCurrerntRef.earnClick;//state.model.upgrades[state.level].earn;
                    if (coroutine == null)
                    {
                        coroutine = StartCoroutine(EarnMaterials());
                    }
                    //ใช้ตอนอัพเกรดแล้วเวลายังเดินอยู่
                    else
                    {
                        StopCoroutine(coroutine);
                        coroutine = StartCoroutine(EarnMaterials());
                        isSpawning = false; 
                    }
                }
                else
                {
                    Debug.Log("isSpawning = true");
                }
            }
        }
        
        private IEnumerator EarnMaterials()
        {
            if (earnObj == null)
            {
                earnObj = Instantiate(earnPrefab, transform.position, Quaternion.identity);
                earnObj.transform.SetParent(earnParent.transform);
                earnObj.transform.GetComponent<RectTransform>().localScale = Vector3.one;
                earnObj.GetComponent<WorldToUIPos>().targetTransform = this.transform;
                //earnObj.GetComponent<EarnButton>().SetEarnTime(currentEarn.timeSpawn , currentEarn.timeSpawn);
            }

            while (true)
            {
                if (isSpawning)
                {
                    yield return new WaitUntil(() => !isSpawning);
                }

                timeRemaining = currentEarnClick.timeSpawn;

                while (timeRemaining > 0)
                {
                    earnObj.GetComponent<EarnButton>().SetEarnTime(timeRemaining , currentEarnClick.timeSpawn);
                    yield return new WaitForSeconds(1f); // Countdown by 1 second
                    timeRemaining--;
                }
                EarnItem();
            }

            void EarnItem()
            {
                //Debug.Log($"Earned {currentEarnClick.amount} of {currentEarnClick.earnType}");
                InventoryCost inventoryCost = currency.GetInventoryCost(currentEarnClick.earnType);
                //earnObj.GetComponent<EarnButton>().SetClaimButton(inventoryCost.sprite , currentEarnClick.amount);

                isSpawning = true;
                SetEarnButton(earnObj);

                earnObj.GetComponent<EarnButton>().button.onClick.RemoveAllListeners();
                earnObj.GetComponent<EarnButton>().button.onClick.AddListener(OnButtonClick);
                void OnButtonClick()
                {
                    Earn();
                    SoundManager.Instance.PlayAudioSource("Eff_2");
                }
            }
        }

        public void Earn()
        {
            SetInstance();
            isSpawning = false;
            //currency.GetInventoryCost(currentEarnClick.earnType).Increase(currentEarnClick.amount);
            //Debug.Log($"Earned {currentEarnClick.amount} of {currentEarnClick.earnType}");
            GameManager.Instance.UpgradeBuildingUI().HideBuildUIObj();
        }

        public void ClaimClick()
        {
            if (isSpawning && earnObj != null)
            {
                earnObj.GetComponent<EarnButton>().button.GetComponent<Button>().onClick.Invoke();
            }
        }

        public void SetEarnButton(GameObject _obj)
        {
            earnObj = _obj;
        }

        public GameObject GetEarnButton()
        {
            return earnObj;
        }
    }
}

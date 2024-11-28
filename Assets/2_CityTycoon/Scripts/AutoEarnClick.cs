using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CityTycoon
{
    public class AutoEarnClick : MonoBehaviour
    {
        [Header("<color=#FFFFFF>Ref.</color>")]
        private GameObject earnPrefab;
        private GameObject earnParent;
        private EarnButton earnButton;
        private EffectType effectType;
        private Earn currentEarnClick;

        [SerializeField] private float timeSpawn;
        [SerializeField] private int amountEarned;        // จำนวนที่ได้รับ
        private bool isEarning = false;
        private bool isStartGame = false;

        public void SetPrefeb(GameObject _earnPrefab, GameObject _earnParent)
        {
            earnPrefab = _earnPrefab;
            earnParent = _earnParent;
        }

        public void SetEarn(Earn _earnClick, EffectType _effectType)
        {
            currentEarnClick = _earnClick;
            effectType = _effectType;
            timeSpawn = currentEarnClick.timeSpawn;
            StartEarn();
        }

        public void StartEarn()
        {
            if (!isEarning)
            {
                isEarning = true;
                if (!isStartGame)
                {
                    timeSpawn = 0;
                    isStartGame = true;
                }
                else timeSpawn = 3;
                StartCoroutine(EarnMaterialRoutine());
            }
        }

        public void StopEarn()
        {
            isEarning = false;
            StopCoroutine(EarnMaterialRoutine());
        }

        private IEnumerator EarnMaterialRoutine()
        {
            while (isEarning)
            {
                yield return new WaitForSeconds(timeSpawn);

                // สุ่มจำนวนที่ได้รับ
                amountEarned = Random.Range(currentEarnClick.range[0], currentEarnClick.range[1]);

                // เพิ่มของ (หรือทำอย่างอื่นตามที่คุณต้องการ)
                Debug.Log($"<color=yellow>{name} Auto Click Earned {currentEarnClick.earnType} {amountEarned}!</color>");

                // ตัวอย่างการเพิ่มจำนวนใน Inventory
                // InventoryManager.Instance.AddMaterial(materialName, amountEarned);

                if (amountEarned > 0)
                {
                    if (earnButton == null)
                    {
                        earnButton = Instantiate(earnPrefab, transform.position, Quaternion.identity).GetComponent<EarnButton>();
                        earnButton.transform.SetParent(earnParent.transform);
                        earnButton.transform.GetComponent<RectTransform>().localScale = Vector3.one;
                        earnButton.GetComponent<WorldToUIPos>().targetTransform = this.transform;
                    }
                    else earnButton.gameObject.SetActive(true);

                    Currency currency = GameManager.Instance.Currency();
                    InventoryCost inventoryCost = currency.GetInventoryCost(currentEarnClick.earnType);
                    earnButton.GetComponent<EarnButton>().SetClaimButton(inventoryCost.sprite, amountEarned);
                    StopEarn();

                    earnButton.GetComponent<EarnButton>().button.onClick.RemoveAllListeners();
                    earnButton.GetComponent<EarnButton>().button.onClick.AddListener(OnButtonClick);
                    void OnButtonClick()
                    {
                        currency.GetInventoryCost(currentEarnClick.earnType).Increase(amountEarned);
                        Debug.Log($"Earned {amountEarned} of {currentEarnClick.earnType}");
                        GameManager.Instance.UpgradeBuildingUI().HideBuildUIObj();
                        this.GetComponent<EffectBuilding>().UseEffectBuilding(effectType);
                        earnButton.gameObject.SetActive(false);
                        GameObject effect = Instantiate(currency.bollonEffectPrefab, this.transform.position, Quaternion.identity);
                        StartEarn();
                        StartCoroutine(SpawnBalloonsWithDelay());
                        SoundManager.Instance.PlayAudioSource("Eff_2");
                    }
                }
            }
        }

        private IEnumerator SpawnBalloonsWithDelay()
        {
            for (int i = 0; i < amountEarned; i++)
            {
                // สร้างบอลลูนที่ตำแหน่งของตัวเอง
                GameObject effect = Instantiate(GameManager.Instance.Currency().bollonEffectPrefab, this.transform.position, Quaternion.identity);

                // ตั้งค่าหรือเพิ่มเอฟเฟกต์อื่นๆ ให้บอลลูน (ถ้ามี)
                effect.transform.SetParent(transform);

                // หน่วงเวลา 0.25 วินาทีก่อนสร้างบอลลูนตัวถัดไป
                yield return new WaitForSeconds(0.25f);
            }
        }

        public void ClaimClick()
        {
            if (earnButton != null)
            {
                earnButton.GetComponent<EarnButton>().button.GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}

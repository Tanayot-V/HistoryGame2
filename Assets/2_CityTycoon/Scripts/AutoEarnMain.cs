using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CityTycoon
{
    public class AutoEarnMain : MonoBehaviour
    {
        [Header("<color=#FFFFFF>Ref.</color>")]
        private Image barFillImg;
        private EffectType effectType;
        private Earn currentEarnMain;

        [SerializeField] private float timeSpawn;
        [SerializeField] private int amountEarned;
        float elapsedTime = 0f;
        private bool isEarning = false;

        public void SetFillBCBar(Image _barFillImg)
        {
            barFillImg = _barFillImg;
        }

        public void SetEarn(Earn _earnClick, EffectType _effectType)
        {
            effectType = _effectType;
            currentEarnMain = _earnClick;
            if (currentEarnMain.timeSpawn <= 0) timeSpawn = 3;
            else timeSpawn = currentEarnMain.timeSpawn;
            StartAutoEarn();
        }

        public void StartAutoEarn()
        {
            if (!isEarning)
            {
                isEarning = true;
                StartCoroutine(EarnMaterialRoutine());
            }
        }

        public void StopAutoEarn()
        {
            isEarning = false;
            StopCoroutine(EarnMaterialRoutine());
        }

        private IEnumerator EarnMaterialRoutine()
        {
            while (isEarning)
            {
                elapsedTime = 0f;
                while (elapsedTime < timeSpawn)
                {
                    elapsedTime += Time.deltaTime;
                    if (barFillImg != null)
                    {
                        barFillImg.fillAmount = elapsedTime / timeSpawn;
                    }

                    yield return null;
                }

                amountEarned = Random.Range(currentEarnMain.range[0], currentEarnMain.range[1]);
                Debug.Log($"<color=#FFDB77>{name} Auto main Earned {currentEarnMain.earnType} {amountEarned}!</color>");
                if (amountEarned > 0)
                {
                    GameManager.Instance.Currency().GetInventoryCost(currentEarnMain.earnType).Increase(amountEarned);
                    this.GetComponent<EffectBuilding>().UseEffectBuilding(effectType);
                    Debug.Log($"<color=#green>Earned {amountEarned} of {currentEarnMain.earnType}</color>");
                }

                if (barFillImg != null)
                {
                    barFillImg.fillAmount = 0f;
                }

                //ให้มันเล่นแค่แหล่งอาหารมี่เดียวพอ
                if (this.GetComponent<BuildBaseObj>().state.baseID == "Lv1_BuildingBase_2")
                {
                    GameManager.Instance.PeopleController().PeopleEatFood();
                }
            }
        }
    }
}

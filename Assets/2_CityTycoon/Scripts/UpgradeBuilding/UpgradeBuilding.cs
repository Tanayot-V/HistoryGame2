using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CityTycoon
{
    public class UpgradeBuilding : MonoBehaviour
    {
        private BuildBaseObj currentBuildingBase;

        public void SetCurrentBuildingBase(BuildBaseObj _buildBaseObj)
        {
            currentBuildingBase = _buildBaseObj;
        }

        public BuildBaseObj GetCurrentBuildingBase()
        {
            if (currentBuildingBase != null) return currentBuildingBase;
            else
            {
                Debug.LogError("CurrentBuildingBase null");
                return null;
            }
        }

        //อัพเกรด
        public void SetUpgradeBuilding()
        {
            BuildingBaseState state = currentBuildingBase.state;
            if (state.IsCanUpgrade(currentBuildingBase))
            {
                //currentBuildingBase.GetComponent<AutoEarnClick>().ClaimClick(); //ถ้ามีไปไอเทมที่ยังไม่ได้ cialm
                state.UseItems(currentBuildingBase);
                Debug.Log("state.level =" + state.level);
                state.LevelUp(); //ปรับเลเวล
                currentBuildingBase.SetBuildingBaseState();

                if (currentBuildingBase.buildingBaseRef.isResidence)
                {
                    GameManager.Instance.PeopleController().SetPeopleMax(currentBuildingBase,currentBuildingBase.upgradesCurrerntRef.maxPeople);
                }
                if (currentBuildingBase.buildingBaseRef.isResidence) return;

                currentBuildingBase.GetComponent<AutoEarnClick>().SetEarn(currentBuildingBase.upgradesCurrerntRef.earnClick, currentBuildingBase.upgradesCurrerntRef.effectType);
                currentBuildingBase.GetComponent<AutoEarnMain>().SetEarn(currentBuildingBase.upgradesCurrerntRef.earnMain, currentBuildingBase.upgradesCurrerntRef.effectType);
                GameManager.Instance.BCTime().UpdateSpeedUpTime(currentBuildingBase.upgradesCurrerntRef.speedUpBC);
            }
        }

        public bool IsCanUpgrade()
        {
            Debug.Log("IsCanUpgrade1: Lv1_BuildingBase_1 " + GameManager.Instance.CreateBuilding().IsLevel1ResidenceUpgrade());
            if (currentBuildingBase.state.baseID == "Lv1_BuildingBase_1" && GameManager.Instance.CreateBuilding().IsLevel1ResidenceUpgrade()) return true;
            else return currentBuildingBase.state.IsCanUpgrade(currentBuildingBase);
        }

        public List<CurrencyCost> GetRequestCost()
        {
            return currentBuildingBase.state.model.GetUpgrade(currentBuildingBase.state.level).currencyCosts;
        }
    }
}

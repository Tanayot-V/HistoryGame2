using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CityTycoon.BuildingBase;

namespace CityTycoon
{
    public class BuildBaseObj : MonoBehaviour //Secments
    {
        [Header("<color=#FFFFFF>Ref.</color>")]
        public BuildingBase buildingBaseRef;
        public List<Upgrade> upgradesRef;
        public Upgrade upgradesCurrerntRef;
        public Upgrade upgradesTargetRef;

        [Header("<color=green>State.</color>")]
        public BuildingBaseState state = new BuildingBaseState();
        private UpgradeBuilding upgradeBuilding;
        private UpgradeBuildingUI upgradeBuildingUI;

        private void SetInstance()
        {
            if (upgradeBuilding == null)
            {
                upgradeBuilding = GameManager.Instance.UpgradeBuilding();
            }
            if (upgradeBuildingUI == null)
            {
                upgradeBuildingUI = GameManager.Instance.UpgradeBuildingUI();
            }
        }
        
        public void SetBuildingBaseState()
        {
            Debug.Log("SetBuildingBaseState");
            upgradesCurrerntRef = upgradesRef[state.level - 1];
            if(!state.IsMaxLevel()) upgradesTargetRef = upgradesRef[state.level];

            UiController.Instance.DestorySlot(this.gameObject);
            GameObject asset = Instantiate(this.state.SetPrefab(), Vector3.zero, Quaternion.identity);
            asset.transform.SetParent(this.transform);
            asset.transform.localPosition = Vector3.zero;
            this.GetComponent<BoxCollider2D>().offset = asset.GetComponent<BoxCollider2D>().offset;
            this.GetComponent<BoxCollider2D>().size = asset.GetComponent<BoxCollider2D>().size;
            asset.GetComponent<BoxCollider2D>().enabled = false;

            //effect upgrades
            if (asset.transform.childCount >= 2)
            {
                if(asset.transform.GetChild(1).gameObject.activeSelf) asset.transform.GetChild(1).gameObject.SetActive(false);
                if (state.level > 1)
                {
                    asset.transform.GetChild(1).gameObject.SetActive(true);
                    StartCoroutine(UiController.Instance.WaitForSecond(3, () =>
                    {
                        asset.transform.GetChild(1).gameObject.SetActive(false);
                    }));
                }
            }
        }
        
        private void OnMouseDown()
        {
            if (UiController.IsPointerOverUIObject()) return;
            SetInstance();
            StartCoroutine(UiController.Instance.WaitForSecond(0.2f, () => {
                if (GameManager.Instance.CameraSmooth().isDragging) return;
                //Callback;
                upgradeBuildingUI.ShowUpgradeUI(this);
                /*
                upgradeBuilding.SetCurrentBuildingBase(this);
                if (state.level <= 0)
                {
                    upgradeBuildingUI.ShowUpgradeUI_Level0();
                }
                else
                {
                    Debug.Log(state.baseID + "_" + state.level);
                    upgradeBuildingUI.ShowUpgradeUI(this);
                }*/
            }));
            SoundManager.Instance.PlayAudioSource("Eff_1");
        }
    }
}

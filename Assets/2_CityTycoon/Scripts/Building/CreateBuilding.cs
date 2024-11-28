using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace CityTycoon
{
    public class CreateBuilding : MonoBehaviour //BASE
    {
        [Header("Prefab")]
        [SerializeField] GameObject basePrefab;
        [SerializeField] GameObject parentBuild;
        [SerializeField] GameObject parentEnv;
        [SerializeField] GameObject parentEntityPath;

        [Header("Era")]
        [SerializeField] private int indexEra;
        [SerializeField] EraModelSO currentEraModel;
        private BuildBaseModelData buildbasemodelData;
        private UpgradeBuildingUI upgradeBuildingUI;
        private Currency currency;
        [SerializeField] private List<BuildBaseObj> baseBuildObjList;
        public TMPro.TextMeshProUGUI levelTX;

        public void Init()
        {
            SetInstance();
            indexEra = 0;
            InitCreateBuilding(GameManager.Instance.EraData().eraDataSO[indexEra]);
            upgradeBuildingUI.HideBuildUIObj();
            levelTX.text = "LV." + (indexEra + 1).ToString();
            //GameManager.Instance.DialogController().DialogControllerInit();
            //GameManager.Instance.DialogController().PlaySceneInit("dialog5");
        }

        public void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("IsAllMaxLevel:"+ IsAllMaxLevel());
                //เปลี่ยนยุค
                if (IsAllMaxLevel())
                {
                    GameManager.Instance.LoadingPage().ShowGame2Loading(3);
                    indexEra += 1;
                    InitCreateBuilding(GameManager.Instance.EraData().eraDataSO[indexEra]);
                }
            }
            */
        }

        private void SetInstance()
        {
            buildbasemodelData = GameManager.Instance.BuildBaseModelData();
            upgradeBuildingUI = GameManager.Instance.UpgradeBuildingUI();
            currency = GameManager.Instance.Currency();
        }

        public void InitCreateBuilding(EraModelSO _eraModelSO = null)
        {
            currentEraModel = _eraModelSO;
            UiController.Instance.DestorySlot(upgradeBuildingUI.earnParent);
            UiController.Instance.DestorySlot(parentBuild);
            baseBuildObjList.Clear();
            _eraModelSO.buildingBases.ToList().ForEach(o => {
                BuildBaseObj buildBase = Instantiate(basePrefab, o.position, Quaternion.identity).GetComponent<BuildBaseObj>();
                buildBase.gameObject.name = o.name;
                buildBase.transform.SetParent(parentBuild.transform);
                baseBuildObjList.Add(buildBase);

                //SetDataBuildings
                buildBase.state = new BuildingBaseState(
                    o.baseID,
                    buildbasemodelData.GetBuildingBaseSO(o.baseID),
                    1,
                    o.position );

                buildBase.buildingBaseRef = o;
                buildBase.upgradesRef = o.upgrades;
                buildBase.upgradesCurrerntRef = o.upgrades[0];
                buildBase.GetComponent<BuildBaseObj>().SetBuildingBaseState();

                if (buildBase.buildingBaseRef.isResidence)
                {
                    GameManager.Instance.PeopleController().SetPeopleMax(buildBase,buildBase.upgradesCurrerntRef.maxPeople);
                }
                if (buildBase.buildingBaseRef.isResidence) return;

                //AutoEarnClick
                buildBase.GetComponent<AutoEarnClick>().SetPrefeb(GameManager.Instance.UpgradeBuildingUI().earnPrefab, GameManager.Instance.UpgradeBuildingUI().earnParent);
                buildBase.GetComponent<AutoEarnClick>().SetEarn(buildBase.upgradesCurrerntRef.earnClick, buildBase.upgradesCurrerntRef.effectType);

                //AutoEarnMain
                buildBase.GetComponent<AutoEarnMain>().SetEarn(buildBase.upgradesCurrerntRef.earnMain, buildBase.upgradesCurrerntRef.effectType);
                buildBase.GetComponent<AutoEarnMain>().SetFillBCBar(GameManager.Instance.Currency().fillFoodImg);
            });
            /*
            if (currentEraModel.envPrefab != null)
            {
                UiController.Instance.DestorySlot(parentEnv);
                GameObject env = Instantiate(currentEraModel.envPrefab, Vector3.zero, Quaternion.identity);
                env.transform.SetParent(parentEnv.transform);
            }

            if (currentEraModel.entityPath != null)
            {
                UiController.Instance.DestorySlot(parentEntityPath);
                GameObject path = Instantiate(currentEraModel.entityPath, Vector3.zero, Quaternion.identity);
                path.transform.SetParent(parentEntityPath.transform);
                GameManager.Instance.EntityManager().SetPath(path.GetComponent<EntityPath>());
            }
            UiController.Instance.DestorySlot(GameManager.Instance.EntitySpawner().spwnerParent);
            */
        }

        public bool IsAllMaxLevel()
        {
            bool isAllMaxLevel = true;

            if (baseBuildObjList.Count <= 0) return false;
            baseBuildObjList.ForEach(o => {
                if (!o.state.IsMaxLevel())
                {
                    isAllMaxLevel = false;
                }
            });
            return isAllMaxLevel;
        }

        //แหล่งหาหาอาร #1,#2 = Lv 3
        public bool IsLevel1ResidenceUpgrade()
        {
            if (indexEra == 0) return baseBuildObjList[1].state.level == 3 && baseBuildObjList[2].state.level == 3;
            else return false;
        }

        public BuildBaseObj GetBasebuildList(int _index)
        {
            return baseBuildObjList[_index];
        }
    }
}

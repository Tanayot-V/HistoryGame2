using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

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
        //[SerializeField] private EraModelSO[] eraDataSO;
        [SerializeField] EraModelSO currentEraModel;
        private UpgradeBuildingUI upgradeBuildingUI;
        [SerializeField] private List<BuildBaseObj> baseBuildObjList;
        public TMPro.TextMeshProUGUI levelTX;

        public void Init()
        {

            SetInstance();
            indexEra = 0;
            //InitCreateBuilding(GameManager.Instance.EraData().eraDataSO[indexEra]);
            //currentEraModel = eraDataSO[indexEra];
            InitCreateBuilding(currentEraModel);
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
            upgradeBuildingUI = GameManager.Instance.UpgradeBuildingUI();
        }

        public void InitCreateBuilding(EraModelSO _eraModelSO = null)
        {
            currentEraModel = _eraModelSO;
            UiController.Instance.DestorySlot(upgradeBuildingUI.earnParent);
            UiController.Instance.DestorySlot(parentBuild);
            UiController.Instance.DestorySlot(upgradeBuildingUI.nameParent);
            baseBuildObjList.Clear();
            _eraModelSO.buildingBases.ToList().ForEach(o => {
                BuildBaseObj buildBase = Instantiate(basePrefab, o.position, Quaternion.identity).GetComponent<BuildBaseObj>();
                buildBase.gameObject.name = o.name;
                buildBase.transform.SetParent(parentBuild.transform);
                baseBuildObjList.Add(buildBase);
                //SetDataBuildings
                buildBase.state = new BuildingBaseState(o.baseID,o,1,o.position );

                buildBase.buildingBaseRef = o;
                buildBase.upgradesRef = o.upgrades;
                buildBase.upgradesCurrerntRef = o.upgrades[0];
                buildBase.SetBuildingBaseState();

                if (buildBase.buildingBaseRef.isResidence)
                {
                    GameManager.Instance.PeopleController().SetPeopleMax(buildBase,buildBase.upgradesCurrerntRef.maxPeople);
                }

                //Name Display
                WorldToUIPos nameDisplay = UiController.Instance.InstantiateUIView(upgradeBuildingUI.namePrefab, upgradeBuildingUI.nameParent).GetComponent<WorldToUIPos>();
                nameDisplay.targetTransform = buildBase.transform;
                nameDisplay.offsetX = buildBase.upgradesCurrerntRef.nameOffset.x;
                nameDisplay.offsetY = buildBase.upgradesCurrerntRef.nameOffset.y;
                nameDisplay.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = $"{buildBase.upgradesCurrerntRef.level} {buildBase.upgradesCurrerntRef.displayName}";
                buildBase.nameDisplay = nameDisplay;
                if (buildBase.buildingBaseRef.isResidence) return;

                //AutoEarnClick
                buildBase.GetComponent<AutoEarnClick>().SetPrefeb(upgradeBuildingUI.earnPrefab, upgradeBuildingUI.earnParent);
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

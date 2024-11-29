using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CityTycoon
{
    public class UpgradeBuildingUI : MonoBehaviour //Main
    {
        private InventoryCost inventoryCost;
        private List<CurrencyCost> currencyCosts;

        [Header("Class Instance")]
        private BuildBaseModelData buildBaseModelData;
        private UpgradeBuilding upgradeBuilding;
        private Currency currency;

        [Header("UI")]
        [SerializeField] Sprite[] upgreadeSP;
        [SerializeField] private GameObject buildUIObj;

        [Header("Level")]
        [SerializeField] private GameObject buildUIObj_Lv;
        [SerializeField] private TMPro.TextMeshProUGUI headerLvTx;
        [SerializeField] private Image buildImg;
        [SerializeField] private Button buttonUpgrade;
        [SerializeField] private GameObject requsetCostParent;
        [SerializeField] private GameObject requsetCostPrefab;

        [Header("Level 0")]
        [SerializeField] private GameObject buildUIObj_0;
        [SerializeField] private TMPro.TextMeshProUGUI headerLvTx_0;
        [SerializeField] private Image buildImg_0;
        [SerializeField] private Button buttonUpgrade_0;

        [Header("Auto Earn and Name")]
        public GameObject earnParent;
        public GameObject earnPrefab;
        public GameObject nameParent;
        public GameObject namePrefab;

        private void SetInstance()
        {
            if (upgradeBuilding == null) upgradeBuilding = GameManager.Instance.UpgradeBuilding();
            if (buildBaseModelData == null) buildBaseModelData = GameManager.Instance.BuildBaseModelData();
            if (currency == null) currency = GameManager.Instance.Currency();
        }

        public void HideBuildUIObj()
        {
            if (!buildUIObj.activeSelf) return;
            buildUIObj.SetActive(false);
            Time.timeScale = 1;
        }

        public void ShowUpgradeUICallBack(System.Action callback)
        {
            //ShowUpgradeUI()
            callback?.Invoke(); // เรียก Callback เมื่อทำงานเสร็จ
        }

        bool isCanUpgrade;
        public void ShowUpgradeUI(BuildBaseObj _buildBaseObj)
        {
            //if (SummaryView.Instance.loadingGO.activeSelf) return;
            if (_buildBaseObj.state.IsMaxLevel()) return;
            if (buildUIObj.activeSelf) return;
            SetInstance();

            upgradeBuilding.SetCurrentBuildingBase(_buildBaseObj);
            buildUIObj.SetActive(true); //Transition
            UIView(_buildBaseObj.state.baseID, _buildBaseObj.state.level);
            Time.timeScale = 0;

            void UIView(string _buildingID, int _level)
            {
                buildUIObj_Lv.SetActive(true);
                buildUIObj_0.SetActive(false);
                headerLvTx.text = $"เลเวล {_level} >> {_level + 1}";
                buildImg.sprite = _buildBaseObj.upgradesTargetRef.sprite;

                //Request Costs
                UiController.Instance.DestorySlot(requsetCostParent);
                currencyCosts = _buildBaseObj.upgradesCurrerntRef.currencyCosts;//upgradeBuilding.GetRequestCost();
                Debug.Log("currencyCosts:" + currencyCosts.Count);
                currencyCosts.ForEach(o =>
                {
                    inventoryCost = currency.GetInventoryCost(o.costType);
                    GameObject requsetSlot = UiController.Instance.InstantiateUIView(requsetCostPrefab, requsetCostParent);
                    requsetSlot.transform.GetChild(0).GetComponent<Image>().sprite = inventoryCost.sprite;
                    requsetSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = $"{inventoryCost.amount}/{o.amount}";

                    if (inventoryCost.amount < o.amount) requsetSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().color = Color.gray;
                    else requsetSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().color = UiController.Instance.SetColorWithHex("#4C2B2C");//Color.black;
                });

                HandleBuildBase(_buildBaseObj);

                //ปุ่มสีเขียว และ สีเทา
                if (_buildBaseObj.state.baseID == "Lv1_BuildingBase_1")
                {
                    isCanUpgrade = upgradeBuilding.IsCanUpgrade() && GameManager.Instance.CreateBuilding().IsLevel1ResidenceUpgrade();

                }
                else
                {
                    isCanUpgrade = upgradeBuilding.IsCanUpgrade();
                }
                if (isCanUpgrade) buttonUpgrade.transform.GetChild(0).GetComponent<Image>().sprite = upgreadeSP[0];
                else buttonUpgrade.transform.GetChild(0).GetComponent<Image>().sprite = upgreadeSP[1];

                //buttonUpgrade.onClick.RemoveAllListeners();
                //buttonUpgrade.onClick.AddListener(OnButtonClick);

            }
        }

        //ด่านที่ 1 #2 #3 Level = 3 Ui view
        private void HandleBuildBase(BuildBaseObj _buildBaseObj)
        {
            if (_buildBaseObj.state.baseID == "Lv1_BuildingBase_1")
            {
                SetupBuildBase(GameManager.Instance.CreateBuilding().GetBasebuildList(1));
                SetupBuildBase(GameManager.Instance.CreateBuilding().GetBasebuildList(2));
            }

            void SetupBuildBase(BuildBaseObj _buildBaseObj)
            {
                GameObject requsetSlot = UiController.Instance.InstantiateUIView(requsetCostPrefab, requsetCostParent);
                requsetSlot.transform.GetChild(0).GetComponent<Image>().sprite = _buildBaseObj.upgradesCurrerntRef.sprite;
                requsetSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = $"Level {_buildBaseObj.state.level}/3";
                if (_buildBaseObj.state.level < 3) requsetSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().color = Color.gray;
                else requsetSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().color = UiController.Instance.SetColorWithHex("#4C2B2C");
            }
        }

        public void OnButtonClick()
        {
            Debug.Log("OnButtonClick:" + isCanUpgrade);
            if (isCanUpgrade)
            {
                upgradeBuilding.SetUpgradeBuilding();                
                SoundClick();
                HideBuildUIObj();
            }
            else
            {
                return;
            }
        }

        /*
        public void ShowUpgradeUI_Level0()
        {
            SetInstance();

            if (buildUIObj.activeSelf) return;
            GameManager.Instance.UITransitionController().UpgradeBuildUI(buildUIObj);
            SetUpgradeUILevel0();

            void SetUpgradeUILevel0()
            {
                buildUIObj_Lv.SetActive(false);
                buildUIObj_0.SetActive(true);
                headerLvTx_0.text = "เลเวล 1";
                buildImg_0.sprite = upgradeBuilding.GetCurrentBuildingBase().state.model.GetUpgrade(1).icon;

                buttonUpgrade_0.onClick.RemoveAllListeners();
                buttonUpgrade_0.onClick.AddListener(OnButtonClick);
                void OnButtonClick()
                {
                    Debug.Log("OnButtonClick 0");
                    upgradeBuilding.SetUpgradeBuilding();
                    SoundClick();
                    buildUIObj.SetActive(false);
                }
            }
        }

        public void ShowUpgradeUI2(BuildingBaseState state)
        {
            Debug.Log($"Click5:{state.IsMaxLevel()}");
            if (state.IsMaxLevel()) return;
            if (buildUIObj.activeSelf) return;
            SetInstance();

            buildUIObj.SetActive(true); //Transition
            SetUpgradeUILevel(state.baseID, state.level);
            Debug.Log("Click6:");

            void SetUpgradeUILevel(string _buildingID, int _level)
            {
                buildUIObj_Lv.SetActive(true);
                buildUIObj_0.SetActive(false);
                headerLvTx.text = $"เลเวล {_level} >> {_level+1}";
                buildImg.sprite = buildBaseModelData.GetBuildingBaseSO(_buildingID).GetUpgrade(_level+1).icon;

                //Request Costs
                UiController.Instance.DestorySlot(requsetCostParent);
                currencyCosts = upgradeBuilding.GetRequestCost();
                currencyCosts.ForEach(o => {
                    inventoryCost = currency.GetInventoryCost(o.costType);
                    GameObject requsetSlot = Instantiate(requsetCostPrefab, transform.position, Quaternion.identity);
                    requsetSlot.transform.SetParent(requsetCostParent.transform);
                    requsetSlot.transform.GetComponent<RectTransform>().localScale = Vector3.one;
                    requsetSlot.transform.GetChild(0).GetComponent<Image>().sprite = inventoryCost.sprite;
                    requsetSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = $"{inventoryCost.amount}/{o.amount}";

                    if (inventoryCost.amount < o.amount) requsetSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().color = Color.gray;
                    else requsetSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
                });

                //ปุ่มสีเขียว และ สีเทา
                bool isCanUpgrade = upgradeBuilding.IsCanUpgrade();
                if (isCanUpgrade)
                {
                    buttonUpgrade.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    buttonUpgrade.GetComponent<Image>().color = Color.grey;
                }

                buttonUpgrade.onClick.RemoveAllListeners();
                buttonUpgrade.onClick.AddListener(OnButtonClick);
                void OnButtonClick()
                {
                    Debug.Log("OnButtonClick");
                    if (isCanUpgrade)
                    {
                        upgradeBuilding.SetUpgradeBuilding();
                        SoundClick();
                        buildUIObj.SetActive(false);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        */
        public void SoundClick()
        {
            SoundManager.Instance.PlayAudioSource("Eff_2");
        }
    }
}

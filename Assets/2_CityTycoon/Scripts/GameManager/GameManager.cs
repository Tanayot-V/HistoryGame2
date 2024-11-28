using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CityTycoon;

namespace CityTycoon
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private void Awake()
        {
            Instance = this;
        }

        [Header("Main Game")]
        [SerializeField] private AudioModelSO audioModelSO;
        [SerializeField] private EffectModelSO effectModelSO;
        [SerializeField] private LoadingPage loadingPage;
        [SerializeField] private CameraSmooth cameraSmooth;
        [SerializeField] private UITransitionController uiTransitionController;

        [Header("Data Dialogs")]
        [SerializeField] private BottomBarController bottomBarController;
        [SerializeField] private DialogController dialogController;
        [SerializeField] private DialogsData dialogsData;
        [SerializeField] private SpeakerData speakerData;

        [Header("Data Building")]
        [SerializeField] private BCTime bcTime;
        [SerializeField] private UpgradeBuildingUI upgradeBuildingUI;
        [SerializeField] private UpgradeBuilding upgradeBuilding;
        [SerializeField] private CreateBuilding createBuilding;
        [SerializeField] private BuildBaseModelData buildBaseModelData;
        [SerializeField] private Currency currency;
        [SerializeField] private PeopleController peopleController;

        [Header("Entity")]
        [SerializeField] private EntityManager entityManager;
        [SerializeField] private EntitySpawner entitySpawner;

        public void Start()
        {
            
            SoundManager.Instance.Init(audioModelSO);
            //SoundManager.Instance.PlayAudioSource("BGM_1");
            EffectManager.Instance.Init(effectModelSO);
            SettingState(1);
            SummaryView.Instance.OnLoading(false);
            SummaryView.Instance.OnStartGame();
            currency.InitInventoryCost();
            /*
            //LoadingPage().ShowBigLoading(3);
            
            DialogController().DialogControllerInit();
            DialogController().PlaySceneInit("dialog_1");*/
        }

        public void StartButton()
        {
            CreateBuilding().Init();
            BCTime().TogglePause();
            SummaryView.Instance.loadingGO.SetActive(false);
        }

        private void SettingState(int _Level)
        {
            switch (_Level)
            {
                case 1:
                    Currency().GetInventoryCost(CostType.PEOPLE).amount = 0;
                    Currency().GetInventoryCost(CostType.PEOPLE).Increase(5);

                    Currency().GetInventoryCost(CostType.FOOD).amount = 0;
                    //Currency().GetInventoryCost(CostType.FOOD).Increase(5);

                    BCTime().Init(7000);
                    break;
            }
            Currency().InitInventoryCost();
            BCTime().TogglePause();
        }

        public PeopleController PeopleController()
        {
            return DataCenterManager.GetData(ref peopleController, "PeopleController");
        }

        public BCTime BCTime()
        {
            return DataCenterManager.GetData(ref bcTime, "BCTime");
        }

        public UITransitionController UITransitionController()
        {
            return DataCenterManager.GetData(ref uiTransitionController, "UITransitionController");
        }

        public LoadingPage LoadingPage()
        {
            return DataCenterManager.GetData(ref loadingPage, "main- LoadingPage");
        }

        public CameraSmooth CameraSmooth()
        {
            return DataCenterManager.GetData(ref cameraSmooth, "Main Camera");
        }

        #region Dialog Data
        public DialogsData DialogsData()
        {
            return DataCenterManager.GetData(ref dialogsData, "DialogsData");
        }

        public DialogController DialogController()
        {
            return DataCenterManager.GetData(ref dialogController, "DialogController");
        }

        public SpeakerData SpeakerData()
        {
            return DataCenterManager.GetData(ref speakerData, "SpeakerData");
        }

        public BottomBarController BottomBarController()
        {
            return DataCenterManager.GetData(ref bottomBarController, "main - ButtomberController");
        }
        #endregion

        #region Data Building
        /*
        public EraData EraData()
        {
            return DataCenterManager.GetData(ref eraData, "EraData");
        }*/

        public UpgradeBuildingUI UpgradeBuildingUI()
        {
            return DataCenterManager.GetData(ref upgradeBuildingUI, "UpgradeBuildingUI");
        }

        public UpgradeBuilding UpgradeBuilding()
        {
            return DataCenterManager.GetData(ref upgradeBuilding, "UpgradeBuilding");
        }

        public CreateBuilding CreateBuilding()
        {
            return DataCenterManager.GetData(ref createBuilding, "-Start- CreateBuilding");
        }

        public BuildBaseModelData BuildBaseModelData()
        {
            return DataCenterManager.GetData(ref buildBaseModelData, "BuildBaseModelData");
        }

        public Currency Currency()
        {
            return DataCenterManager.GetData(ref currency, "Currency");
        }
        #endregion

        #region Entity
        public EntityManager EntityManager()
        {
            return DataCenterManager.GetData(ref entityManager, "EntityManager");
        }

        public EntitySpawner EntitySpawner()
        {
            return DataCenterManager.GetData(ref entitySpawner, "EntitySpawner");
        }
        #endregion
    }
}

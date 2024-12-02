using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

namespace CityTycoon
{
    public class SummaryView : Singletons<SummaryView>
    {
        public GameObject loadingGO;
        [SerializeField] private GameObject startGameGO;
        [SerializeField] private GameObject victoryGO;
        [SerializeField] private GameObject loseGO;
        [SerializeField] private GameObject triviaGO;
        bool isSummary;

        public void Update()
        {
            /*
            if(Input.GetKeyDown(KeyCode.V))
            {
                OnVictory();
            }*/
        }
        private void InitUIView()
        {
            loadingGO.SetActive(false);
            startGameGO.SetActive(false);
            victoryGO.SetActive(false);
            loseGO.SetActive(false);
            triviaGO.SetActive(false);
        }

        public void OnLoading(bool _isIn)
        {
            InitUIView();
            loadingGO.SetActive(true);
            SkeletonGraphic skeletonAnimation = loadingGO.GetComponent<SkeletonGraphic>();

            if(_isIn) skeletonAnimation.AnimationState.SetAnimation(0, "in", false);
            else skeletonAnimation.AnimationState.SetAnimation(0, "out", false);
        }

        public void OnStartGame()
        {
            InitUIView();
            loadingGO.SetActive(true);
            startGameGO.SetActive(true);
        }

        public void OnVictory()
        {
            InitUIView();
            victoryGO.SetActive(true);
            isSummary = true;
            Time.timeScale = 0;

            GameObject content = victoryGO.transform.GetChild(1).gameObject;
            content.GetComponent<UIBounceAnimation>().enabled = false;
            UITransition.Instance.ScaleOneSet(content.gameObject,new Vector3(1.5f,1.5f,1.5f), Vector3.one,0.5f, () => {
                content.GetComponent<UIBounceAnimation>().enabled = true;
            });
        }

        public void OnLose()
        {
            if (isSummary) return;
            InitUIView();
            loseGO.SetActive(true);
            isSummary = true;
            Time.timeScale = 0;

            GameObject content = loseGO.transform.GetChild(1).gameObject;
            content.GetComponent<UIBounceAnimation>().enabled = false;
            UITransition.Instance.ScaleOneSet(content.gameObject, new Vector3(1.5f, 1.5f, 1.5f), Vector3.one, 0.5f, () => {
                content.GetComponent<UIBounceAnimation>().enabled = true;
            });
        }

        public void OnTrivia()
        {
            InitUIView();
            triviaGO.SetActive(true);
            Time.timeScale = 0;
            GameObject content = triviaGO.transform.GetChild(1).gameObject;
            UITransition.Instance.CanvasGroupAlpha(content.gameObject, 0, 1, 1, null);
        }

        public void OnClickTrivia(string _name)
        {
            Time.timeScale = 1;
            //OnLoading(true);
            DataCenterManager.Instance.LoadSceneByName(_name);
            /*
            StartCoroutine(
            UiController.Instance.WaitForSecond(3,()=>{
                DataCenterManager.Instance.LoadSceneByName(_name);
            }));*/
        }
    }
}

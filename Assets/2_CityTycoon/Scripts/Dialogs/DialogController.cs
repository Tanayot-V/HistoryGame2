using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CityTycoon
{
    public class DialogController : MonoBehaviour
    {
        [Header("Data")]
        private Dialog currentDialog;

        [Header("References")]
        private BottomBarController dialogBar;
        private DialogsData dialogsData;

        private State state = State.IDLE;
        private enum State
        {
            IDLE, ANIMATE
        }

        public void DialogControllerInit()
        {
            dialogBar = GameManager.Instance.BottomBarController();
            dialogsData = GameManager.Instance.DialogsData();
            dialogsData.InitData();
            GameManager.Instance.BottomBarController().SetSpeakerData(GameManager.Instance.SpeakerData());
        }

        public void PlaySceneInit(string _dialogID)
        {
            currentDialog = dialogsData.GetDialog(_dialogID);
            state = State.IDLE;
            dialogBar.Show();
            dialogBar.PlayScene(currentDialog);
        }

        public void EnterDialogBarButton()
        {
            if (dialogBar.IsHidden()) return;
            if (state == State.IDLE)
            {
                if (dialogBar.IsCompleted())
                {
                    if (dialogBar.IsLastSentence())
                    {
                        PlaySwitchScene(currentDialog);
                    }
                    else
                    {
                        dialogBar.PlayNextSentence();
                    }
                }
                else
                {
                    dialogBar.Skip();
                }
            }          
        }

        public void PlaySwitchScene(Dialog scene)
        {
            StartCoroutine(SwitchScene(scene));
        }

        private IEnumerator SwitchScene(Dialog scene)
        {
            state = State.ANIMATE;
            currentDialog = scene;
            dialogBar.Hide();
            yield return new WaitForSeconds(1f);

            switch (scene.nextAction)
            {
                case DialogTakeAction.NEXTDIALOG:
                    if (scene.nextActionID != null) StartCoroutine(NEXTDIALOG(scene.nextActionID));
                    break;
                case DialogTakeAction.NONE:break;
            }

            IEnumerator NEXTDIALOG(string _dialogID)
            {
                currentDialog = dialogsData.GetDialog(_dialogID);
                yield return new WaitForSeconds(1f);
                dialogBar.ClearText();
                dialogBar.Show();
                yield return new WaitForSeconds(1f);
                dialogBar.PlayScene(currentDialog);
                state = State.IDLE;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarnButton : MonoBehaviour
{
    [Header("ClaimButton")]
    public GameObject claimButtonObj;
    public Button button;
    public Image iconImg;
    public TMPro.TextMeshProUGUI amountTX;

    [Header("EarnTime")]
    public GameObject earnTimeObj;
    public Image fillImg;
    public TMPro.TextMeshProUGUI timeTX;

    public void SetClaimButton(Sprite _icon, int _amount)
    {
        claimButtonObj.SetActive(true);
        earnTimeObj.SetActive(false);
        iconImg.sprite = _icon;
        amountTX.text = "x " + _amount.ToString();
    }

    public void SetEarnTime(float _timeRemaining, float _maxTime)
    {
        claimButtonObj.SetActive(false);
        earnTimeObj.SetActive(true);
        timeTX.text = _timeRemaining.ToString();
        fillImg.fillAmount = _timeRemaining / _maxTime;
    }
}

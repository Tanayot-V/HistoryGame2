using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CityTycoon;
using UnityEngine;
using UnityEngine.UI;

namespace CityTycoon
{
    public enum CostType
    {
        None,
        BC, //เวลา BC
        PEOPLE, //ประชากร
        FOOD //อาหาร
    }

    [System.Serializable]
    public class InventoryCost
    {
        public CostType costType;
        public string comment;
        public Sprite sprite;
        public int amount;
        public Image imageIcon;
        public TMPro.TextMeshProUGUI text;

        public void SetText()
        {
            text.text = amount.ToString("f0");
        }

        public void Increase(int _amount)
        {
            amount += _amount;
            SetText();
        }

        public void IncreaseMax(int _amount , int _max)
        {
            if (_amount <= 0) return;
            amount += _amount;
            if (amount > _max) amount = _max;
            SetText();
        }


        public void Decrease(int _amount)
        {
            Debug.Log("Decrease :" + _amount);
            amount -= _amount;
            if (amount < 0) amount = 0;
            SetText();
        }

        public void DecreaseMax(int _amount, int _max)
        {
            amount = _max;
            SetText();
        }
    }

    [System.Serializable]
    public class InventoryCostModel
    {
        public string id;
        public string displayName;
        public CostType costType;
        public Sprite sprite;
    }

    public class Currency : MonoBehaviour
    {
        public GameObject bollonEffectPrefab;
        public Image fillPeopleImg;
        public Image fillFoodImg;

        [SerializeField] private List<InventoryCostModel> inventoryCostModel;
        [SerializeField] private List<InventoryCost> inventoryCosts;

        public void InitInventoryCost()
        {
            inventoryCosts.ForEach(o => { o.SetText(); });
        }

        public List<InventoryCost> GetInventoryAllCosts()
        {
            return inventoryCosts;
        }

        public Dictionary<CostType, InventoryCost> inventoryCostsDic = new Dictionary<CostType, InventoryCost>();
        public InventoryCost GetInventoryCost(CostType _costType)
        {
            if (inventoryCostsDic.ContainsKey(_costType))
            {
                return inventoryCostsDic[_costType];
            }
            else
            {
                return inventoryCostsDic[_costType] = inventoryCosts.Find(o => o.costType == _costType);
            }
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CityTycoon
{
    [System.Serializable]
    public class BuildingBaseState //BASE
    {
        public string baseID;
        public BuildingBase model;
        public int level;
        public Vector2 position;

        public BuildingBaseState()
        {
            baseID = "BaseID Unknow";
            model = null;
            level = 1;
            position = Vector2.zero;
        }

        public BuildingBaseState(string _baseId, BuildingBase _model, int _level,Vector2 _position)
        {
            baseID = _baseId;
            model = _model;
            level = _level;
            position = _position;
        }

        public void LevelUp()
        {
            if(!IsMaxLevel()) level += 1;
            Debug.Log(baseID+" level =" + level);
        }

        public GameObject SetPrefab()
        {
            if (level <= 0) level = 1;
            return model.upgrades[level-1].prefab;
        }

        public void SetSprite(SpriteRenderer _spriteRenderer)
        {
            _spriteRenderer.sprite = model.upgrades[level].sprite;
        }

        public bool IsMaxLevel()
        {
            return level == (model.upgrades.Count);
        }

        public void UseItems(BuildBaseObj _buildBaseObj)
        {
            List<InventoryCost> inventoryCost = GameManager.Instance.Currency().GetInventoryAllCosts();
            Dictionary<CostType, int> requiredCost = _buildBaseObj.upgradesCurrerntRef.currencyCosts.ToDictionary(o => o.costType, o => o.amount);
            //Dictionary<CostType, int> requiredCost = upgradeTarget.currencyCosts.ToDictionary(o => o.costType, o => o.amount);

            foreach (var cost in requiredCost)
            {
                var inventoryItem = inventoryCost.FirstOrDefault(item => item.costType == cost.Key);

                if (inventoryItem != null && inventoryItem.amount >= cost.Value)
                {
                    inventoryItem.Decrease(cost.Value);
                }
            }
        }

        public bool IsCanUpgrade(BuildBaseObj _buildBaseObj)
        {
            Dictionary<CostType, int> inventoryCost = GameManager.Instance.Currency().GetInventoryAllCosts().ToDictionary(o => o.costType, o => o.amount);
            Dictionary<CostType, int> requiredCost = _buildBaseObj.upgradesCurrerntRef.currencyCosts.ToDictionary(o => o.costType, o => o.amount);//model.upgrades[level].currencyCosts.ToDictionary(o => o.costType, o => o.amount);
            //Dictionary<CostType, int> requiredCost = upgradeTarget.currencyCosts.ToDictionary(o => o.costType, o => o.amount);//model.upgrades[level].currencyCosts.ToDictionary(o => o.costType, o => o.amount);

            Debug.Log("<color=#FFFFFF>IsCanUpgrade : Food: </color>" + requiredCost[CostType.FOOD]);
            if (HasItems(requiredCost))
            {
                //UseItems(requiredCost);
                Debug.Log("IsCanUpgrad = true!");
                return true;
            }
            else
            {
                Debug.Log($"Not enough materials to upgrade the item.");
                return false;
            }

            // เช็คว่ามีไอเทมตามที่ต้องการหรือไม่
            bool HasItems(Dictionary<CostType, int> requiredCost)
            {
                foreach (var required in requiredCost)
                {
                    if (!inventoryCost.ContainsKey(required.Key) || inventoryCost[required.Key] < required.Value)
                    {
                        //Debug.Log($"Requset = {required.Key} = {required.Value}| Inventory = {inventoryCost[required.Key]} ");
                        return false;
                    }
                }
                return true;
            }
        }
    }
}

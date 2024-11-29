using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CityTycoon
{
    public class BuildBaseModelData : MonoBehaviour
    {
        [SerializeField] private List<BuildingBase> buildingBases;

        private Dictionary<string, BuildingBase> buildbaseDic = new Dictionary<string, BuildingBase>();
        public BuildingBase GetBuildingBaseSO(string _id)
        {
            if (buildbaseDic.ContainsKey(_id))
            {
                return buildbaseDic[_id];
            }
            else
            {
                BuildingBase foundDic = buildingBases.FirstOrDefault(o => o.baseID == _id);
                if (foundDic != null)
                {
                    buildbaseDic[_id] = foundDic;
                    return foundDic;
                }
                else
                {
                    Debug.LogError($"Dialog not found: {_id}");
                    return default(BuildingBase);
                }
            }
        }
    }

    [System.Serializable]
    public class Upgrade
    {
        public string level;
        public string displayName;
        public Vector2 nameOffset;
        public Sprite icon;
        public Sprite sprite;
        public GameObject prefab;
        public int maxPeople;//isResidence
        public float speedUpBC;
        public EffectType effectType;
        public List<CurrencyCost> currencyCosts;
        public Earn earnMain;
        public Earn earnClick;

    }

    [System.Serializable]
    public class Earn
    {
        public CostType earnType;
        public int[] range;
        public float timeSpawn;
    }

    [System.Serializable]
    public class CurrencyCost
    {
        public CostType costType;
        public int amount;

        public CurrencyCost(CostType _costType, int _amount)
        {
            this.costType = _costType;
            this.amount = _amount;
        }
    }

    public enum EffectType
    {
        None,
        KillPeople01,
        KillPeople02,
        KillPeople03,
        KillPeople04
    }
}

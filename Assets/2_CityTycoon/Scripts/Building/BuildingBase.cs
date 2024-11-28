using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CityTycoon
{
    [CreateAssetMenu(fileName = "BuildingBase", menuName = "City Tycoon/Building Base")]
    [System.Serializable]
    public class BuildingBase : ScriptableObject
    {
        public string baseID;
        public string displayName;
        public Vector3 position;
        public bool isResidence;
        public List<Upgrade> upgrades;

        [System.Serializable]
        public struct Upgrade
        {
            public string level;
            public string displayName;
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
        public struct Earn
        {
            public CostType earnType;
            public int[] range;
            public float timeSpawn;
        }

        public Upgrade GetUpgrade(int _level)
        {
            if (_level >= upgrades.Count)
            {
                return new Upgrade();
            }
            else
            {
                return upgrades[_level];
            } 
        }
    }

    [System.Serializable]
    public struct CurrencyCost
    {
        public CostType costType;
        public int amount;

        public CurrencyCost(CostType _costType , int _amount)
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

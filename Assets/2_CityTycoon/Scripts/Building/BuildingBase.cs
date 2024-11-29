using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

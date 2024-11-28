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
}

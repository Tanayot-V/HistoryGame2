using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CityTycoon
{
    [CreateAssetMenu(fileName = "EraModelSO", menuName = "City Tycoon/EraModelSO")]
    [System.Serializable]
    public class EraModelSO : ScriptableObject
    {
        [Header("<color=#FFFFFF>Base Data</color>")]
        public string eraID;
        public GameObject envPrefab; 
        public GameObject entityPath; 

        [Header("<color=#00FFD2>BC.</color>")]
        public float bcBegin; 
        public float speedUpBCBegin; 

        [Header("<color=green>Building Base</color>")]
        public BuildingBase[] buildingBases;
    }
}

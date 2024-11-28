using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CityTycoon
{
    public class UITransitionController : MonoBehaviour
    {
        private float[] duration;
        public GameObject[] goObjs;

        void Start()
        {
            duration = UITransition.Instance.InitializeDurations();
        }

        public void UpgradeBuildUI(GameObject _obj)
        {
            //if (_obj.activeSelf) return;
            _obj.SetActive(true);
            UITransition.Instance.SlideOneY(_obj, new Vector2(0, -1500), new Vector2(0, 240), duration[10]);
        }
    }
}

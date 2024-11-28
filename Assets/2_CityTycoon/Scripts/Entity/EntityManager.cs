using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CityTycoon
{
    public class EntityManager : MonoBehaviour
    {
        public Transform startPoint;
        public Transform[] path;

        public void SetPath(EntityPath _EntityPath)
        {
            startPoint = _EntityPath.startPoint;
            path = _EntityPath.path;
        }
    }
}

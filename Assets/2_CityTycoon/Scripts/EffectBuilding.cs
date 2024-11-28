using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CityTycoon
{
    public class EffectBuilding : MonoBehaviour
    {
        public void UseEffectBuilding(EffectType _effectType)
        {
            if (_effectType == EffectType.None) return;
            KillPeople(_effectType);
            Maxpeople(_effectType);

            void KillPeople(EffectType _effectType)
            {
                int rend = 0;
                switch (_effectType)
                {
                    case EffectType.KillPeople01:
                         rend = Random.Range(1, 15);
                        break;
                    case EffectType.KillPeople02:
                         rend = Random.Range(1, 10);
                        break;
                    case EffectType.KillPeople03:
                        rend = Random.Range(1, 20);
                        break;
                    case EffectType.KillPeople04:
                        rend = Random.Range(1, 22);
                        break;
                }
                Debug.Log($"Effect: Kill people.{rend}");

                if (rend <= 3)
                {
                    Debug.Log($"<color=red>Effect: Kill people.</color>");
                    GameManager.Instance.PeopleController().KillPeopleAmount(1);
                }
            }

            void Maxpeople(EffectType _effectType)
            {

            }
        }
    }
}

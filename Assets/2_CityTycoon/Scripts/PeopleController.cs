using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CityTycoon
{
    public class PeopleController : MonoBehaviour
    {
        [Header("<color=#FFFFFF>Ref.</color>")]
        [SerializeField] Currency currency;
        [SerializeField] BCTime bCTime;
        [SerializeField] BuildBaseObj residenceBuildBase;
        [SerializeField] List<GameObject> peopleList;

        public InventoryCost peopleInven;
        public InventoryCost bcTimeInven;

        private Image peopleFillImg;
        private int peopleMax;
        private bool isStartGame;

        public void Start()
        {
            if (peopleList.Count <= 0) return; 
            peopleList.ForEach(o => { o.SetActive(false); });
            for (int i = 0; i < 5; i++)
            {
                peopleList[i].SetActive(true);
            }
        }

        public void SetPeopleMax(BuildBaseObj _residence , int _peopleMax)
        {
            residenceBuildBase = _residence;
            if (residenceBuildBase.state.IsMaxLevel())
                peopleMax = _peopleMax + 3;
            else
                peopleMax = _peopleMax;

            if (!isStartGame)
            {
                StartCoroutine(UiController.Instance.WaitForSecond(6, () =>
                {
                    isStartGame = true;
                }));
            }
        }

        public void Update()
        {
            /*
            if (peopleFillImg != null)
            {
                peopleFillImg.fillAmount = bCTime.deathTimer / 5f;
            }
            else peopleFillImg = currency.fillPeopleImg;

            //peopleInven.amount = Mathf.Min(peopleInven.amount, peopleMax);
            //if (peopleInven.amount >= peopleMax) peopleInven.amount = peopleMax;
            if (isStartGame)
            {
                if (peopleInven.amount <= 0)
                {
                    peopleInven.amount = 0;
                    SummaryView.Instance.OnLose();
                }
            }*/
        }

        private Coroutine victoryCoroutine;

        public void SpawnPeople()
        {
            //ใส่สูตรคนเกิด
            int people = Random.Range(1, Mathf.FloorToInt(peopleInven.amount / 3f));
            if (people <= 0) return;
            SpwanPeopleAmount(people);
            Debug.Log($"<color=green> A person was born! Total people alive: {people}|{peopleInven.amount}</color>");

            if (peopleInven.amount > 7 && bcTimeInven.amount <= 1500)
            {
                if (victoryCoroutine == null) // หากยังไม่มีการนับถอยหลัง
                {
                    victoryCoroutine = StartCoroutine(WaitForVictory());
                }
            }
            else
            {
                // หยุดการนับถอยหลังหากเงื่อนไขไม่เป็นจริง
                if (victoryCoroutine != null)
                {
                    StopCoroutine(victoryCoroutine);
                    victoryCoroutine = null;
                    Debug.Log("<color=red>Victory countdown stopped.</color>");
                }
            }

            IEnumerator WaitForVictory()
            {
                Debug.Log("<color=yellow>Victory condition met! Counting down 5 seconds...</color>");
                float countdown = 5f;

                while (countdown > 0)
                {
                    if (peopleInven.amount < 7 ) // หากเงื่อนไขเปลี่ยนระหว่างนับถอยหลัง
                    {
                        Debug.Log("<color=red>Victory condition failed during countdown.</color>");
                        victoryCoroutine = null; // รีเซ็ต Coroutine
                        yield break; // หยุด Coroutine
                    }

                    countdown -= Time.deltaTime;
                    Debug.Log($"<color=yellow>Countdown: {countdown:F1} seconds remaining...</color>");
                    yield return null; // รอ 1 เฟรม
                }

                SummaryView.Instance.OnVictory(); // เรียกชัยชนะเมื่อครบเวลา
                victoryCoroutine = null; // รีเซ็ต Coroutine
            }
        }

        public void SpwanPeopleAmount(int _amount)
        {
            if (_amount < 0) return;

            //ดักไม่ให้ peopleInven.amount > peopleMax
            if (peopleInven.amount + _amount > peopleMax)
            {
                _amount = peopleMax - peopleInven.amount; // ปรับ _amount ให้เพิ่มได้ไม่เกิน peopleMax
                Debug.Log("peopleMax: " + _amount);
            }
            peopleInven.IncreaseMax(_amount,peopleMax);
            UpdatePeopleList();
        }

        public void KillPeople()
        {
            //ใส่สูตรคนตาย
            int people = Random.Range(0, (int)Mathf.Floor(peopleInven.amount / 4f));
            if (people <= 0) return;
            KillPeopleAmount(people);
            Debug.Log($"<color=red>A person has died. Total people alive: {people}|{peopleInven.amount}</color>");
        }

        public void KillPeopleAmount(int _amount)
        {
            if (_amount < 0) return;

            //peopleInven.amount -= _amount;
            peopleInven.Decrease(_amount);
            if (!isStartGame)
            {
                if (peopleInven.amount <= 0)
                {
                    peopleInven.DecreaseMax(_amount,1);
                }
            }
            else
            {
                if (peopleInven.amount <= 0)
                {
                    peopleInven.amount = 0;
                    if (isStartGame) SummaryView.Instance.OnLose();
                }
            }
            UpdatePeopleList();
        }

        void UpdatePeopleList()
        {
            peopleList.ForEach(o => { o.SetActive(false); });
            for (int i = 0; i < peopleInven.amount; i++)
            {
                peopleList[i].SetActive(true); // Active ตามจำนวนประชากรที่มีชีวิต
            }
        }

        public void PeopleEatFood()
        {
            // จำนวนอาหารและจำนวนคนปัจจุบัน
            int food = currency.GetInventoryCost(CostType.FOOD).amount;
            int people = currency.GetInventoryCost(CostType.PEOPLE).amount;

            // ตรวจสอบว่าอาหารเพียงพอหรือไม่
            if (food >= people)
            {
                // กรณีอาหารเพียงพอ ลดอาหารตามจำนวนคน
                currency.GetInventoryCost(CostType.FOOD).Decrease(people);
                Debug.Log($"<color=yellow>eat People ate {people} foods. Remaining food: {food - people}</color>");
            }
            else
            {
                // กรณีอาหารไม่เพียงพอ ลดอาหารจนหมด และลดจำนวนคนตามอาหารที่มี
                currency.GetInventoryCost(CostType.FOOD).Decrease(food);
                int peopleDied = people - food;
                currency.GetInventoryCost(CostType.PEOPLE).Decrease(peopleDied);
                Debug.Log($"<color=red>eat Not enough food. People ate {food} foods. Remaining food: 0 peopleDied: {peopleDied}</color>");
            }
        }
    }
}

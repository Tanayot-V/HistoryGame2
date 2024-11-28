using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CityTycoon
{
    public class BallonEffect : MonoBehaviour
    {
        public Vector3 targetPosition; // ตำแหน่งที่ต้องการให้เคลื่อนที่ไป
        public float moveDuration = 1f; // ระยะเวลาที่ใช้ในการเคลื่อนที่
       // public float jumpPower = 2f;

        public void Start()
        {
            MoveToTargetAndDisappear();
        }

        public void MoveToTargetAndDisappear()
        {
            transform.DOMove(targetPosition, moveDuration).OnComplete(() =>
            {
                // เมื่อเคลื่อนที่ถึงตำแหน่งเป้าหมายแล้ว ทำให้วัตถุหายไป
                gameObject.SetActive(false);
            });
            /*
            // เคลื่อนที่ไปยังตำแหน่งเป้าหมาย
            transform.DOJump(targetPosition, jumpPower, 1, moveDuration).OnComplete(() =>
            {
                // เมื่อเคลื่อนไปยังตำแหน่งเป้าหมายแล้ว ทำให้วัตถุหายไป
                gameObject.SetActive(false);
            });*/
        }
    }
}

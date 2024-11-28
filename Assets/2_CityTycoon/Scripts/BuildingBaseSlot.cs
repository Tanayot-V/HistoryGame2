using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBaseSlot : MonoBehaviour
{
    private Color originalColor; // เก็บสีเดิมของ Sprite
    private SpriteRenderer spriteRenderer; // SpriteRenderer ของวัตถุ

    void Start()
    {
        // ดึง SpriteRenderer และเก็บสีเดิมของ Sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void OnMouseOver()
    {
        // เปลี่ยนสีเป็นสีเทาดำ
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f, 1f); // สีเทาเข้ม
        }
    }

    void OnMouseExit()
    {
        // คืนค่าสีเดิม
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}

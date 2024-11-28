using UnityEngine;

public class PinchZoomWebGL : MonoBehaviour
{
    public float zoomSpeed = 0.5f; // ความเร็วในการซูม
    public float minZoom = 2f;     // ระยะซูมต่ำสุด
    public float maxZoom = 10f;    // ระยะซูมสูงสุด

    private Camera cam;
    private bool isPinching = false;
    private float initialPinchDistance;
    private float initialZoom;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // ตรวจจับการสัมผัสสองจุด
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (!isPinching)
            {
                // เริ่มต้นการซูม
                isPinching = true;
                initialPinchDistance = Vector2.Distance(touchZero.position, touchOne.position);
                initialZoom = cam.orthographicSize;
            }
            else
            {
                // คำนวณระยะห่างปัจจุบัน
                float currentPinchDistance = Vector2.Distance(touchZero.position, touchOne.position);
                float pinchRatio = initialPinchDistance / currentPinchDistance;

                // ปรับขนาดกล้อง
                cam.orthographicSize = Mathf.Clamp(initialZoom * pinchRatio, minZoom, maxZoom);
            }
        }
        else
        {
            // รีเซ็ตการซูม
            isPinching = false;
        }
    }
}
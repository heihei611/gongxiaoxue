using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook1 : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // 拖拽 XR Rig 到这里

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // 如果没有手动指定，自动找到父级的 XR Rig
        if (playerBody == null)
            playerBody = transform.parent;
    }

    // 在MouseLook1类中添加这个方法
    void OnGUI()
    {
        // 简单的十字准星
        float size = 10f;
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);

        GUI.color = Color.white;
        GUI.DrawTexture(new Rect(center.x - 1, center.y - size, 2, size * 2), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(center.x - size, center.y - 1, size * 2, 2), Texture2D.whiteTexture);
    }

    void Update()
    {
        // 按 Escape 解锁鼠标
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // 鼠标左键重新锁定
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
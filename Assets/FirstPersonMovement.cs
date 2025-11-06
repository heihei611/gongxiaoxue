using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 2f;
    public float jumpForce = 10f;  // 降低跳跃高度
    [Header("视角参考 (若为空将自动使用 Main Camera)")]
    public Transform cameraTransform; // 新增：用于决定移动方向的视角

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        // 添加 Character Controller 组件
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
            controller.height = 0.4f;     // 降低角色高度：从1.8f改为1.4f
            controller.radius = 0.1f;     // 减小角色半径，更容易通过门
            controller.center = new Vector3(0, 0.2f, 0);  // 调整中心点
        }

        // 如果未指定 cameraTransform，则使用主摄像机（Main Camera）
        if (cameraTransform == null)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
            else
            {
                Camera anyCam = FindObjectOfType<Camera>();
                if (anyCam != null) cameraTransform = anyCam.transform;
            }
        }
    }

    void Update()
    {
        // 检测是否着地
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 获取输入
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 移动：以视角为参考（将摄像机前向投影到水平面，避免俯仰影响）
        Vector3 move;
        if (cameraTransform != null)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
            move = camRight * x + camForward * z;
        }
        else
        {
            // 回退到物体本身的朝向（兼容场景没有摄像机的情况）
            move = transform.right * x + transform.forward * z;
        }

        controller.Move(move * speed * Time.deltaTime);

        // 跳跃
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        // 重力
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }
}

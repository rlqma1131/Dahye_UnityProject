using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float jumpPower;
    public float moveSpeed;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;   // PlayerLayer를 제외한 나머지 레이어를 groundLayerMask로 설정

    [Header("Look")]
    public Transform cameraContain;
    public float minLookX;
    public float maxLookX;
    public float lookSensitivity;
    public bool canLook = true;    // 카메라 회전 가능 여부

    public Action inventory;
    private float camCurRotX;
    private Vector2 mouseDelta;


    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   //마우스 커서 숨기고 잠금
    }

    private void FixedUpdate()  // 물리연산은 FixedUpdate에서 처리하는것이 좋다.
    {
        Move();    // 이동 함수는 계속 호출
    }

    private void LateUpdate()
    {
        if(canLook)    // 카메라 회전 가능 여부에 따라 회전
            CameraLook();
    }


    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;    // 방향벡터에 동속도 곱해서 속도 벡터로 변환

        dir.y = rb.velocity.y;    // y축 속도 유지

        rb.velocity = dir;    // Rigidbody의 속도에 dir을 대입
    }

    private void CameraLook()
    {
        camCurRotX += mouseDelta.y * lookSensitivity;
        camCurRotX = Mathf.Clamp(camCurRotX, minLookX, maxLookX);    // 카메라 회전 각도 제한
        cameraContain.localEulerAngles = new Vector3(-camCurRotX, 0, 0);    // 카메라만 상하 회전

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);    // 플레이어 자체를 좌우 회전
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();    // 마우스 이동값을 읽어옴
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();    // W누르면 (0,1), A누르면 (-1,0) 반환하는 방식으로
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;    // 떼면 (0,0) 반환
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);    // 점프
        }
    }

    bool IsGrounded()
    {
        Ray[] ray = new Ray[4]  // 바닥에 닿는지 확인하기 위한 레이저 책상다리 4개
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), // 약간 위에서 아래로 쏘는
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], 0.1f, groundLayerMask))    // 0.1f길이의 레이저가 groundLayerMask에 닿으면 true 반환
            {
                return true;
            }
        }
        return false;    // 바닥에 닿지 않으면 false 반환
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();    // 인벤토리 열고 닫기
        }
    }

    void ToggleCursor()
    {
        bool CursorLocked = Cursor.lockState == CursorLockMode.Locked; // 인벤토리가 열려있는지 확인
        Cursor.lockState = CursorLocked ? CursorLockMode.None : CursorLockMode.Locked; // 인벤토리 열면 마우스 잠금 해제
        canLook = !CursorLocked;
    }
}

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
    public LayerMask groundLayerMask;   // PlayerLayer�� ������ ������ ���̾ groundLayerMask�� ����

    [Header("Look")]
    public Transform cameraContain;
    public float minLookX;
    public float maxLookX;
    public float lookSensitivity;
    public bool canLook = true;    // ī�޶� ȸ�� ���� ����

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
        Cursor.lockState = CursorLockMode.Locked;   //���콺 Ŀ�� ����� ���
    }

    private void FixedUpdate()  // ���������� FixedUpdate���� ó���ϴ°��� ����.
    {
        Move();    // �̵� �Լ��� ��� ȣ��
    }

    private void LateUpdate()
    {
        if(canLook)    // ī�޶� ȸ�� ���� ���ο� ���� ȸ��
            CameraLook();
    }


    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;    // ���⺤�Ϳ� ���ӵ� ���ؼ� �ӵ� ���ͷ� ��ȯ

        dir.y = rb.velocity.y;    // y�� �ӵ� ����

        rb.velocity = dir;    // Rigidbody�� �ӵ��� dir�� ����
    }

    private void CameraLook()
    {
        camCurRotX += mouseDelta.y * lookSensitivity;
        camCurRotX = Mathf.Clamp(camCurRotX, minLookX, maxLookX);    // ī�޶� ȸ�� ���� ����
        cameraContain.localEulerAngles = new Vector3(-camCurRotX, 0, 0);    // ī�޶� ���� ȸ��

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);    // �÷��̾� ��ü�� �¿� ȸ��
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();    // ���콺 �̵����� �о��
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();    // W������ (0,1), A������ (-1,0) ��ȯ�ϴ� �������
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;    // ���� (0,0) ��ȯ
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);    // ����
        }
    }

    bool IsGrounded()
    {
        Ray[] ray = new Ray[4]  // �ٴڿ� ����� Ȯ���ϱ� ���� ������ å��ٸ� 4��
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), // �ణ ������ �Ʒ��� ���
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], 0.1f, groundLayerMask))    // 0.1f������ �������� groundLayerMask�� ������ true ��ȯ
            {
                return true;
            }
        }
        return false;    // �ٴڿ� ���� ������ false ��ȯ
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();    // �κ��丮 ���� �ݱ�
        }
    }

    void ToggleCursor()
    {
        bool CursorLocked = Cursor.lockState == CursorLockMode.Locked; // �κ��丮�� �����ִ��� Ȯ��
        Cursor.lockState = CursorLocked ? CursorLockMode.None : CursorLockMode.Locked; // �κ��丮 ���� ���콺 ��� ����
        canLook = !CursorLocked;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private new Camera camera;
    private GameManager gameManager;

    public void Init(GameManager gamemManager) // GameManager�� �޾Ƽ� �ʱ�ȭ 
    {
        this.gameManager = gamemManager;
        camera = Camera.main;
    }

    protected override void HandleAction()
    {

    }

    public override void Death()
    {
        base.Death();
        gameManager.GameOver(); // ���ӸŴ����� ���ӿ��� ��û
    }

    void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition); // ��ũ���� ��ǥ�� ������ǥ�� �ٲ��ִ�
        lookDirection = (worldPos - (Vector2)transform.position); // ���콺��ġ�� �÷��̾� ���� �����������

        if (lookDirection.magnitude < 0.9f) // ���콺�� �ʹ� ������ ������
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (EventSystem.current.IsPointerOverGameObject()) // UI�� �����Ͱ� �������� �̻����� �����ʴ´�.
            return;

        isAttacking = inputValue.isPressed;
    }
}

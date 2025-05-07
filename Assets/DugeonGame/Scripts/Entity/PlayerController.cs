using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private new Camera camera;
    private GameManager gameManager;

    public void Init(GameManager gamemManager) // GameManager를 받아서 초기화 
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
        gameManager.GameOver(); // 게임매니저에 게임오버 요청
    }

    void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition); // 스크린상 좌표를 월드좌표로 바꿔주는
        lookDirection = (worldPos - (Vector2)transform.position); // 마우스위치가 플레이어 기준 어느방향인지

        if (lookDirection.magnitude < 0.9f) // 마우스가 너무 가까이 있으면
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
        if (EventSystem.current.IsPointerOverGameObject()) // UI에 포인터가 있을때는 미사일을 쏘지않는다.
            return;

        isAttacking = inputValue.isPressed;
    }
}

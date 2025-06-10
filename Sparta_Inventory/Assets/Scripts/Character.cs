using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] public Vector2 moveOutPos;
    [SerializeField] private Vector2 moveInPos = Vector2.zero;
    [SerializeField] private float moveDuration = 0.3f;

    [SerializeField] private float attackPower;
    [SerializeField] private float deffencePower;
    [SerializeField] private float maxHP;
    [SerializeField] private float curHP;
    [SerializeField] private float critical;

    public float AttackPower => attackPower;
    public float DeffencePower => deffencePower;
    public float MaxHP => maxHP;
    public float CurHP => curHP;
    public float Critical => critical;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void MoveIn()
    {
        rectTransform.DOAnchorPos(moveInPos, moveDuration);
    }

    public void MoveOut()
    {
        rectTransform.DOAnchorPos(moveOutPos, moveDuration);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Atack Info")]
    [SerializeField] private float delay = 1f;
    public float Delay { get => delay; set => delay = value; }

    [SerializeField] private float weaponSize = 1f;

    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    [SerializeField] private float power = 1f;
    public float Power { get => power; set => power = value; }

    [SerializeField] private float speed = 1f;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private float attackRange = 10f;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    public LayerMask target;

    [Header("Knock Back Info")]
    [SerializeField] private bool isOnKnokback = false;
    public bool IsOnknokback { get => isOnKnokback; set => isOnKnokback = value; }

    [SerializeField] private float knockbackPower = 0.1f;
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }

    [SerializeField] private float knockbackTime = 0.5f;
    public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }

    private static readonly int IsAttack = Animator.StringToHash("IsAttack"); // 트리거

    public BaseController controller { get; private set; }

    private Animator animator;
    private SpriteRenderer weaponRender;

    public AudioClip attackSoundClip;

    protected virtual void Awake()
    {
        controller = GetComponentInParent<BaseController>();
        animator = GetComponentInChildren<Animator>();
        weaponRender = GetComponentInChildren<SpriteRenderer>();

        animator.speed = 1.0f / delay; // 애니메이션 속도 조절
        transform.localScale = new Vector3(weaponSize, weaponSize, 1f); // 무기 크기 조절
    }

    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
        AttackAnimaion();
        if (attackSoundClip) 
        {
            SoundManager.PlayClip(attackSoundClip);
        }
    }

    public void AttackAnimaion()
    {
        animator.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        weaponRender.flipY = isLeft; // 무기 회전
    }




}

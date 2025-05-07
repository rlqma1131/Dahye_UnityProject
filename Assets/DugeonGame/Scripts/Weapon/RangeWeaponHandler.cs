using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Range Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float bulletSize = 1f;
    public float BulletSize { get {  return bulletSize; } }

    [SerializeField] private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] private float spread;
    public float Spread { get { return spread; } }

    [SerializeField] private int numberOfProjectilesPerShot; // �߻��� �Ѿ� ��
    public int NumberOfProjectilesPerShot { get { return numberOfProjectilesPerShot; } }

    [SerializeField] private float multipleProjectileAngle;
    public float MultipleProjectileAngle { get { return multipleProjectileAngle; } }

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } }

    private ProjectileManager projectileManager;

    protected override void Start()
    {
        base.Start();
        projectileManager = ProjectileManager.Instance;
    }
    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;
        int numberOfProjectilePerShot = numberOfProjectilesPerShot;

        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace; // �߻� ���� ������ �ּҰ�

        for(int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAngle + (i * projectileAngleSpace); // �߻� ���� ���
            float randomSpread = Random.Range(-spread, spread); // ���� �������� �� ���
            angle += randomSpread; // �߻� ������ �������� �߰�
            CreateProjectile(controller.LookDirection, angle); // �Ѿ� ����
        }
    }

    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {
        projectileManager.ShootBullet(
            this,
            projectileSpawnPosition.position,
            RotateVector2(_lookDirection, angle)
            );
    }

    private static Vector2 RotateVector2(Vector2 vector, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * vector;
    }
}

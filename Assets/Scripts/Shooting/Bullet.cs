using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Normal,
    AOE,
    Reflecting
}

[CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable Objects/Bullet")]
public class Bullet : ScriptableObject
{
    public BulletType bulletType;
    public float speed;
    public float damage;
    public GameObject bulletPrefab;
    public float lifeTime;

    private List<GameObject> _activeBullets = new List<GameObject>();
    //public float explosionRadius; // For AOE bullets
    //public int maxReflections; // For reflecting bullets

    public void Shoot(Transform origin, Vector3 direction)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, origin.position, origin.rotation);
        switch (bulletType)
        {
            case BulletType.Normal:
                NormalBulletBehavior normalBehavior = bulletInstance.AddComponent<NormalBulletBehavior>();
                normalBehavior.Initialize(speed, direction, lifeTime, damage);
                break;
            case BulletType.AOE:
                //AOEBulletBehavior aoeBehavior = bulletInstance.AddComponent<AOEBulletBehavior>();
                //aoeBehavior.Initialize(speed, damage, explosionRadius);
                break;
            case BulletType.Reflecting:
                //ReflectingBulletBehavior reflectingBehavior = bulletInstance.AddComponent<ReflectingBulletBehavior>();
                //reflectingBehavior.Initialize(speed, damage, maxReflections);
                break;
        }
    }
}
using UnityEngine;

public class AOEBulletBehavior : MonoBehaviour
{
    private float speed;
    private float damage;
    private float explosionRadius;

    public void Initialize(float speed, float damage, float explosionRadius)
    {
        this.speed = speed;
        this.damage = damage;
        this.explosionRadius = explosionRadius;
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
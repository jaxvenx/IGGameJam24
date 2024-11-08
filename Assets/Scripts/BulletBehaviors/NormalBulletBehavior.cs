using UnityEngine;

public class NormalBulletBehavior : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    private float lifeTime;
    private float damage;

    public void Initialize(float speed, Vector3 direction, float lifeTime, float damage)
    {
        this.speed = speed;
        this.direction = direction;
        this.lifeTime = lifeTime;
        this.damage = damage;
        Invoke(nameof(DestroyBullet), lifeTime);
    }

    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
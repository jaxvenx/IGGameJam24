using UnityEngine;

public class ReflectingBulletBehavior : MonoBehaviour
{
    private float speed;
    private float damage;
    private int maxReflections;
    private int reflections;

    public void Initialize(float speed, float damage, int maxReflections)
    {
        this.speed = speed;
        this.damage = damage;
        this.maxReflections = maxReflections;
        this.reflections = 0;
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
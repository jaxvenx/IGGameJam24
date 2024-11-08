using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private HealthBar healthBar;
    private float currentHealth;

    private void Start()
    {
        currentHealth = health;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth, health);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}

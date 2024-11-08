using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private HealthBar healthBar;
    private float currentHealth;

    [Header("Detection")]
    [SerializeField] private float sightRange = 10f;
    [SerializeField] protected Transform player;
    protected NavMeshAgent NavMeshAgent;

    private bool _hasSeenPlayer;

    protected virtual void Start()
    {
        currentHealth = health;
        NavMeshAgent = GetComponent<NavMeshAgent>();
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

    private void Update()
    {
        // Only rotate around the y-axis to face the player
        transform.rotation = Quaternion.LookRotation(new Vector3(player.position.x, 0, player.position.z) - new Vector3(transform.position.x, 0, transform.position.z));
        if (!_hasSeenPlayer)
            CanSeePlayer();
        else
            MoveTowardsPlayer();
    }

    private void CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < sightRange)
        {
            // Raycast to the player and check if tag is player
            if (Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, sightRange))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    _hasSeenPlayer = true;
                    return;
                }
            }
        }

        _hasSeenPlayer = false;
    }

    protected abstract void MoveTowardsPlayer();
    protected abstract void AttackPlayer();
}
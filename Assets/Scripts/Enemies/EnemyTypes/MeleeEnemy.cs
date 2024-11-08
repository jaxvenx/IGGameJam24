using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;

    protected override void MoveTowardsPlayer()
    {
        NavMeshAgent.SetDestination(player.position);
        Debug.Log("Status of the NavMeshAgent: " + NavMeshAgent.pathStatus);
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            AttackPlayer();
        }
    }

    protected override void AttackPlayer()
    {
        // Implement melee attack logic here
        Debug.Log("Melee attack on player with damage: " + attackDamage);
    }
}
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 2f;

    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float dashSpeed = 100f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 10f;
    private bool _canAttack = true;
    private bool _canDash = true;

    protected override void MoveTowardsPlayer()
    {
        NavMeshAgent.SetDestination(player.position);
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            if (_canAttack)
            {
                AttackPlayer();
                _canAttack = false;
                Invoke(nameof(ResetAttackCooldown), attackCooldown);
            }
        }
        else if (_canDash)
        {
            Dash();
            _canDash = false;
            Invoke(nameof(ResetDashCooldown), dashCooldown);
        }
    }

    protected override void AttackPlayer()
    {
        // Implement melee attack logic here
        _player.TakeDamage();
        Debug.Log("Melee attack on player");
    }

    private void Dash()
    {
        NavMeshAgent.speed = dashSpeed;
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash()
    {
        NavMeshAgent.speed = normalSpeed;
    }

    private void ResetAttackCooldown()
    {
        _canAttack = true;
    }

    private void ResetDashCooldown()
    {
        _canDash = true;
    }
}
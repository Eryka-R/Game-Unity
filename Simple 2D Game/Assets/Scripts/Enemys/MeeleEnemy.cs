using UnityEngine;

public abstract class MeleeEnemy : Enemy
{
    [Header("Sound Attack")]
    [SerializeField] protected AudioClip meleeAttackSound;

    protected void DamagePlayer()
    {
        if (PlayerInSight() && playerHealth != null)
            playerHealth.TakeDamage(damage);
    }
}
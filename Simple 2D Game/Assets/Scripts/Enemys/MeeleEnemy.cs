using UnityEngine;

public abstract class MeleeEnemy : Enemy
{
    protected void DamagePlayer()
    {
        if (PlayerInSight() && playerHealth != null)
            playerHealth.TakeDamage(damage);
    }
}
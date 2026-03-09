using UnityEngine;

public class SlashEnemy : MeleeEnemy
{
    protected override void PerformAttack()
    {
        SoundManager.instance.PlaySound(meleeAttackSound);
        if (enemyPatrol != null && enemyPatrol.enabled)
            anim.SetTrigger("runSlashAttack");
        else
            anim.SetTrigger("slashAttack");
    }
}
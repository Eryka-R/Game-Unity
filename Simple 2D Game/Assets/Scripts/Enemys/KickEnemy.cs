using UnityEngine;

public class KickEnemy : MeleeEnemy
{
    protected override void PerformAttack()
    {
        SoundManager.instance.PlaySound(meleeAttackSound);
        anim.SetTrigger("kickAttack");
    }
}
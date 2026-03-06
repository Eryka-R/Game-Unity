using UnityEngine;

public class KickEnemy : MeleeEnemy
{
    protected override void PerformAttack()
    {
        anim.SetTrigger("kickAttack");
    }
}
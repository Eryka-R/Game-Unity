using UnityEngine;

public class RangeEnemy : Enemy
{
    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Sound Attack")]
    [SerializeField] private AudioClip rangeAttackSound;

    protected override void PerformAttack()
    {
        SoundManager.instance.PlaySound(rangeAttackSound);
        anim.SetTrigger("rangeAttack");
    }

    protected override bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null){
            playerHealth = hit.collider.GetComponent<Health>();
            if (playerHealth == null){
                playerHealth = hit.collider.GetComponentInParent<Health>();
            }
        }
        else
        {
            playerHealth = null;
        }


        return playerHealth != null;
    }

    private void RangedAttack()
    { 
        cooldownTimer = 0f;
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
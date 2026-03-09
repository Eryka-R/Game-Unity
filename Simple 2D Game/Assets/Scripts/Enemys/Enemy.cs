using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Attack parameters")]
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float range;
    [SerializeField] protected int damage;

    [Header("Collider parameters")]
    [SerializeField] protected float colliderDistance;
    [SerializeField] protected BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] protected LayerMask playerLayer;
    protected float cooldownTimer = Mathf.Infinity;
    protected Animator anim;
    protected Health playerHealth;

    protected PatrolEnemy enemyPatrol;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<PatrolEnemy>();
    }


    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
        {
            cooldownTimer = 0f;
            PerformAttack();
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    protected abstract void PerformAttack();

    protected virtual bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.lossyScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0f,
            Vector2.left,
            0f,
            playerLayer
        );

        if (hit.collider != null)
            playerHealth = hit.collider.GetComponent<Health>();

        return hit.collider != null;
    }


    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.lossyScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }
}
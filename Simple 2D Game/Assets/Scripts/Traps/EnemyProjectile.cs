using UnityEngine;

public class EnemyProjectile : EnemyDamage
{

    [SerializeField] private float speed = 10;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;

    private bool hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }


    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("trap") || collision.tag.Contains("Trigger")){
            return;
        }
        hit = true;
        base.OnTriggerEnter2D (collision);
        coll.enabled = false;
        // print("Enemys Projectile hit: " + collision.gameObject.tag);
        if (!collision.CompareTag("Heart"))
        {
            if (anim != null)
            {
                anim.SetTrigger("explode");
            }
            else{
                gameObject.SetActive(false);
            }
        }
    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }
}
 
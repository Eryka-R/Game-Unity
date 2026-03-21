using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float lifetimeLimit = 3f;
    private float direction;
    private bool hit;
    private float lifetime;

    private BoxCollider2D boxCollider;
    private Animator anim;


    private void OnEnable()
    {
        lifetime = 0f;
        hit = false;
        if (boxCollider) boxCollider.enabled = true;
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > lifetimeLimit) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Door") || collision.name.Contains("trap") || collision.tag.Contains("Trigger"))
            return;

        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");

        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Health>() != null){
                collision.GetComponent<Health>().TakeDamage(1);
            }
        }
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

}

using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip coinSound;

    [Header("Movement")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float speed = 2f;

    private Animator anim;
    private SpriteRenderer spriteRend;
    private Collider2D col;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        col.enabled = false;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            col.enabled = true; // ✅ ya se puede recoger
        }
    }

    private void Start()
    {
        anim.SetTrigger("rotate");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // print("Coin hit: " + collision.gameObject.tag);
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            GameManager.Instance.AddCoins(1);
        }
    }

}

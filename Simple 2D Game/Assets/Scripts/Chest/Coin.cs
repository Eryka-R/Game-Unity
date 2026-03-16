using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip coinSound;

    private Animator anim;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        anim.SetTrigger("rotate");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Coin hit: " + collision.gameObject.tag);
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }

}

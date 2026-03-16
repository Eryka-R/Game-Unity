using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip chestSound;

    private GameObject coin;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        coin = transform.GetChild(0).gameObject;
        coin.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetTrigger("Open");
        }
    }

    public void appearCoin(){
        coin.SetActive(true);
    }
}

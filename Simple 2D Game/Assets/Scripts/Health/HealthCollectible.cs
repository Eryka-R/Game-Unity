using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healtValue;
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(collectSound);
            collision.GetComponent<Health>().AddHealth(healtValue);
            gameObject.SetActive(false);
        }
    }
}

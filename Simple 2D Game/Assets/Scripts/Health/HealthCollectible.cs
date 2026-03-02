using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healtValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().AddHealth(healtValue);
            gameObject.SetActive(false);
        }
    }
}

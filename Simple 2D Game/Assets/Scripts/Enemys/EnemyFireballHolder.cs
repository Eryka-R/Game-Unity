using UnityEngine;

public class EnemyFireballHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private void Update()
{
    transform.localScale = new Vector3(
        Mathf.Sign(enemy.localScale.x) * Mathf.Abs(transform.localScale.x),
        transform.localScale.y,
        transform.localScale.z
    );
}
}

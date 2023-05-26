using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float damage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI unit = other.gameObject.GetComponent<EnemyAI>();
        if (unit != null)
        {
            // unit.TakeDamage(damage);
            Debug.Log($"{gameObject} dealt {damage} damage to {other.gameObject}!");
        }
    }
}

using UnityEngine;

public class EnemyDamageableComponent : MonoBehaviour, IDamageable
{
    public int PlayerDamage = 30;
    public void TakeDamage(int damage)
    {
        StatsManager.Instance.DealDamageToEnemy(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerProjectile"))
        {
            TakeDamage(PlayerDamage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TakeDamage(PlayerDamage);
        }
    }
}
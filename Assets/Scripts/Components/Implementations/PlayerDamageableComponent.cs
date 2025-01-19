using System.Collections;
using UnityEngine;

public class PlayerDamageableComponent : MonoBehaviour, IDamageable
{
    public int EnemyDamage = 30;
    public int EnemyProjectileDamage = 10;
    public int EnemyLaserDamage = 1;

    bool isLaserDamage = false;

    private IEnumerator LaserDamageCO()
    {
        isLaserDamage = false;
        yield return new WaitForSeconds(0.1f);
        TakeDamage(EnemyLaserDamage);
        isLaserDamage = true;
    }

    private void Update()
    {
        if(isLaserDamage)
        {
            StartCoroutine(LaserDamageCO());
        }
    }

    public void TakeDamage(int damage)
    {
        StatsManager.Instance.DealDamageToPlayer(damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Boss"))
        {
            TakeDamage(EnemyDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            TakeDamage(EnemyProjectileDamage);
        }
    }
}

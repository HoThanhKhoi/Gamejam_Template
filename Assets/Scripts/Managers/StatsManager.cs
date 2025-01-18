using System;
using UnityEngine;
using Utils;

public class StatsManager : Singleton<StatsManager>
{
    public Stat EnemyHealth;
    public Stat PlayerHealth;

    public event Action OnEnemyDead;
    public event Action OnPlayerDead;

    public void DealDamageToEnemy(int damage)
    {
        EnemyHealth.AddModifier(-damage);

        if (EnemyHealth.GetValue() <= 0)
        {
            OnEnemyDead?.Invoke();
        }
    }

    public void DealDamageToPlayer(int damage)
    {
        PlayerHealth.AddModifier(-damage);

        if (PlayerHealth.GetValue() <= 0)
        {
            OnPlayerDead?.Invoke();
        }
    }
}

using System;
using UnityEngine;
using Utils;

public class StatsManager : Singleton<StatsManager>
{
	public int EnemyMaxHealth; // Initial value
	public int PlayerMaxHealth; // Initial value

	public int enemyCurrentHealth;
	public int playerCurrentHealth;

	public event Action<int, int> OnEnemyHealthChanged;
	public event Action<int, int> OnPlayerHealthChanged;

	public event Action OnEnemyDead;
	public event Action OnPlayerDead;

	private void OnEnable()
	{
		OnPlayerDead += GameOver;
		OnEnemyDead += Win;
	}

	private void Start()
	{
		enemyCurrentHealth = EnemyMaxHealth;
		playerCurrentHealth = PlayerMaxHealth;
	}

	public void DealDamageToEnemy(int damage)
	{
		enemyCurrentHealth -= damage;

		OnEnemyHealthChanged?.Invoke(enemyCurrentHealth, EnemyMaxHealth);

		if (enemyCurrentHealth <= 0)
		{
			OnEnemyDead?.Invoke();
		}
	}

	public void DealDamageToPlayer(int damage)
	{
		playerCurrentHealth -= damage;

		OnPlayerHealthChanged?.Invoke(playerCurrentHealth, PlayerMaxHealth);

		if (playerCurrentHealth <= 0)
		{
			OnPlayerDead?.Invoke();
		}
	}

	public int GetPlayerCurrentHealth()
	{
		return playerCurrentHealth;
	}

	public int GetPlayerMaxHealth()
	{
		return PlayerMaxHealth;
	}

	public int GetEnemyCurrentHealth()
	{
		return enemyCurrentHealth;
	}

	public int GetEnemyMaxHealth()
	{
		return EnemyMaxHealth;
	}

	public void GameOver()
	{
		ResetStats();
		SceneManagers.Instance.LoadScene(5);
	}

	public void Win()
	{
		ResetStats();
		SceneManagers.Instance.LoadScene(6);
	}	

	private void ResetStats()
	{
        enemyCurrentHealth = EnemyMaxHealth;
        playerCurrentHealth = PlayerMaxHealth;
		OnEnemyHealthChanged?.Invoke(enemyCurrentHealth, EnemyMaxHealth);
        OnPlayerHealthChanged?.Invoke(playerCurrentHealth, PlayerMaxHealth);
	}
}

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUpdate : MonoBehaviour
{
    public Image healthBar;
    private void OnEnable()
    {
		if (StatsManager.Instance == null)
		{
			Debug.LogWarning("StatsManager instance is missing!");
			return;
		}
		StatsManager.Instance.OnPlayerHealthChanged += UpdateHealthBar;

        int currentHealth = StatsManager.Instance.GetPlayerCurrentHealth();
        int maxHealth = StatsManager.Instance.GetPlayerMaxHealth();

        UpdateHealthBar(currentHealth, maxHealth);
    }

	private void OnDisable()
	{
		StatsManager.Instance.OnPlayerHealthChanged -= UpdateHealthBar;
	}

	public void UpdateHealthBar(int health, int maxHealth)
	{
		if (healthBar == null)
		{
			Debug.LogWarning("HealthBar reference is missing or destroyed!");
			return;
		}
		healthBar.fillAmount = (float) health / (float) maxHealth;
    }
}

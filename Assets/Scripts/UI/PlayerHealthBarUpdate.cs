using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUpdate : MonoBehaviour
{
    public Image healthBar;
    private void OnEnable()
    {
        StatsManager.Instance.OnPlayerHealthChanged += UpdateHealthBar;

        int currentHealth = StatsManager.Instance.GetPlayerCurrentHealth();
        int maxHealth = StatsManager.Instance.GetPlayerMaxHealth();
        UpdateHealthBar(currentHealth, maxHealth);
    }
    public void UpdateHealthBar(int health, int maxHealth)
    {
        healthBar.fillAmount = (float) health / (float) maxHealth;
    }
}

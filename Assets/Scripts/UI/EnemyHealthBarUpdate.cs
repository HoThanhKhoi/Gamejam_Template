using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUpdate : MonoBehaviour
{
    public Image healthBar;

    private void OnEnable()
    {
        StatsManager.Instance.OnEnemyHealthChanged += UpdateHealthBar;

        int currentHealth = StatsManager.Instance.GetEnemyCurrentHealth();
        int maxHealth = StatsManager.Instance.GetEnemyMaxHealth();
        UpdateHealthBar(currentHealth, maxHealth);
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnEnemyHealthChanged -= UpdateHealthBar;
    }
    public void UpdateHealthBar(int health, int maxHealth)
    {
        healthBar.fillAmount = (float)health / (float)maxHealth;
    }

}

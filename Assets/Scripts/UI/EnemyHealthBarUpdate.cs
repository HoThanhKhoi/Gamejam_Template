using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUpdate : MonoBehaviour
{
    public Image healthBar;

    private void OnEnable()
    {
        StatsManager.Instance.OnEnemyHealthChanged += UpdateHealthBar;
    }
    public void UpdateHealthBar(int health, int maxHealth)
    {
        healthBar.fillAmount = (float)health / (float)maxHealth;
    }

}

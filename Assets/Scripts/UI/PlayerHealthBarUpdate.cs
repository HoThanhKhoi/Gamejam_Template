using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUpdate : MonoBehaviour
{
    public Image healthBar;
    private void OnEnable()
    {
        StatsManager.Instance.OnPlayerHealthChanged += UpdateHealthBar;
    }
    public void UpdateHealthBar(int health, int maxHealth)
    {
        healthBar.fillAmount = (float) health / (float) maxHealth;
    }
}

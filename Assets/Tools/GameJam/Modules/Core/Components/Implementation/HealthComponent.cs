using System;
using UnityEngine;

namespace GameJam.Modules.Core
{
    public class HealthComponent : MonoBehaviour, IHealth
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int currentHealth;

        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                OnHealthChanged?.Invoke(currentHealth, maxHealth);
                if(currentHealth<=0) {
                    OnDead?.Invoke();
                }
            }
        }
        public int MaxHealth
        {
            get => maxHealth;
            set
            {
                maxHealth = Mathf.Max(0, value);
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            }
        }

        public event Action<int, int> OnHealthChanged;
        public event Action OnDead;


        private void Awake()
        {
            if(currentHealth == 0)
            {
                currentHealth = maxHealth;
            }
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
        }

        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
        }
    }
}
using System;
namespace GameJam.Modules.Core
{
    public interface IHealth
    {
        int CurrentHealth { get; }
        int MaxHealth { get; }
        void TakeDamage(int amount);
        void Heal(int amount);
        event Action<int, int> OnHealthChanged;
        event Action OnDead;
    }
}

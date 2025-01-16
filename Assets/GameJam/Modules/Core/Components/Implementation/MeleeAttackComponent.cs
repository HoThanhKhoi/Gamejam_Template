using UnityEngine;

namespace GameJam.Modules.Core
{
    public class MeleeAttackComponent : MonoBehaviour, IAttack
    {
        [field:SerializeField] public float AttackRange { get; set; } = 2f;
        [field:SerializeField] public int AttackDamage = 10;

        public void Attack()
        {
            
        }
    }
}
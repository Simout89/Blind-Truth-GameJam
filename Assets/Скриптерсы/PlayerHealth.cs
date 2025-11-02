using System;
using UnityEngine;

namespace Скриптерсы
{
    public class PlayerHealth: MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField] private CharacterController _characterController;
        private float currentHealth;

        private void Awake()
        {
            currentHealth = _characterController.CharacterControllerData.Health;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            currentHealth = Mathf.Max(currentHealth - damageInfo.Count, 0);
            Debug.Log($"Получил урон: {damageInfo.Count}");
            
            
            if (currentHealth <= 0)
            {
                Debug.Log("Смерть");
            }
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(currentHealth += amount, _characterController.CharacterControllerData.Health);
            Debug.Log($"Захилился: {amount}");
        }
    }

    public interface IDamageable
    {
        public void TakeDamage(DamageInfo damageInfo);
    }

    public interface IHealable
    {
        public void Heal(float amount);
    }
    
    public struct DamageInfo
    {
        public float Count;
        public readonly string DamageDealerName;

        public DamageInfo(float count, string damageDealerName)
        {
            Count = count;
            DamageDealerName = damageDealerName;
        }
    }
}
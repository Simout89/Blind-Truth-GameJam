using System;
using UnityEngine;

namespace Скриптерсы
{
    public class PlayerHealth: MonoBehaviour, IDamageable
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

            if (currentHealth <= 0)
            {
                Debug.Log("Смерть");
            }
        }
    }

    public interface IDamageable
    {
        public void TakeDamage(DamageInfo damageInfo);
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
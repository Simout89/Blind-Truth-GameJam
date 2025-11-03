using System;
using UnityEngine;
using Скриптерсы.Datas;

namespace Скриптерсы
{
    public class EnemyHealth: MonoBehaviour, IDamageable
    {
        [SerializeField] private BodyPart[] _bodyParts;
        private float currentHealth;
        public event Action<DamageInfo> OnTakeDamage;
        public event Action OnDeath;
        
        [SerializeField] private bool destroyOnDie;

        public void Init(EnemyData _enemyData)
        {
            currentHealth = _enemyData.Health;
        }

        private void OnEnable()
        {
            if(_bodyParts.Length == 0)
                return;

            foreach (var bodyPart in _bodyParts)
            {
                bodyPart.OnTakeDamage += HandleBodyPartTakeDamage;
            }
        }

        private void OnDisable()
        {
            if(_bodyParts.Length == 0)
                return;

            foreach (var bodyPart in _bodyParts)
            {
                bodyPart.OnTakeDamage -= HandleBodyPartTakeDamage;
            }
        }

        private void HandleBodyPartTakeDamage(DamageInfo obj)
        {
            TakeDamage(obj);
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnTakeDamage?.Invoke(damageInfo);
            
            currentHealth = Mathf.Max(currentHealth - damageInfo.Count, 0);

            if (currentHealth <= 0)
            {
                Debug.Log("Смерть");
                
                OnDeath?.Invoke();
                // Destroy(gameObject);

                if (destroyOnDie)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
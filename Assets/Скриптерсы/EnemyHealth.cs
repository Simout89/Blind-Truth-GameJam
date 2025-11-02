using System;
using UnityEngine;
using Скриптерсы.Datas;

namespace Скриптерсы
{
    public class EnemyHealth: MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private BodyPart[] _bodyParts;
        private float currentHealth;

        private void Awake()
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
            currentHealth = Mathf.Max(currentHealth - damageInfo.Count, 0);

            if (currentHealth <= 0)
            {
                Debug.Log("Смерть");
            }
        }
    }
}
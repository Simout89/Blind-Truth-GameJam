using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Скриптерсы.View;

namespace Скриптерсы
{
    public class PlayerHealth: MonoBehaviour, IDamageable, IHealable
    {
        [field: SerializeField] public CharacterController _characterController { get; private set; }
        [Inject] private DeathView _deathView;
        public event Action<float> OnHealthChanged;
        private float currentHealth;
        public float CurrentHealth => currentHealth;
        private Coroutine death;

        private void Awake()
        {
            currentHealth = _characterController.CharacterControllerData.Health;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            currentHealth = Mathf.Max(currentHealth - damageInfo.Count, 0);
            OnHealthChanged?.Invoke(-damageInfo.Count);
            RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_TakeDamage");

            Debug.Log($"Получил урон: {damageInfo.Count}");
            
            
            if (currentHealth <= 0)
            {
                Debug.Log("Смерть");

                if(death == null)
                    death =StartCoroutine(DeathHandler());
            }
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, _characterController.CharacterControllerData.Health);
            OnHealthChanged?.Invoke(amount);

            Debug.Log($"Захилился: {currentHealth}");
        }

        public IEnumerator DeathHandler()
        {
            _deathView.Die();
            RuntimeManager.PlayOneShot("event:/SFX/InGame/Enemy/e_Death", transform.position);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        public Transform Transform;

        public DamageInfo(float count, string damageDealerName, Transform transform)
        {
            Count = count;
            DamageDealerName = damageDealerName;
            Transform = transform;
        }
    }
}
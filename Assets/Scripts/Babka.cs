using System;
using FMODUnity;
using UnityEngine;
using Скриптерсы.Enemy;

namespace Скриптерсы
{
    public class Babka: MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private GameObject GameObject;

        private bool flag = true;

        private void OnEnable()
        {
            _enemyHealth.OnDeath += HandleDeath;

            _enemyHealth.OnTakeDamage += HandleTakeDamage;
        }
        private void OnDisable()
        {
            _enemyHealth.OnDeath -= HandleDeath;
            
            _enemyHealth.OnTakeDamage -= HandleTakeDamage;

        }

        private void HandleTakeDamage(DamageInfo obj)
        {
            if(flag)
                RuntimeManager.PlayOneShot("event:/SFX/InGame/Granny/gr_TakeDamage", transform.position);
            flag = false;
        }

        private void HandleDeath()
        {
            RuntimeManager.PlayOneShot("event:/SFX/InGame/Granny/gr_Death", transform.position);

            Destroy(GameObject);
        }

        private void Awake()
        {
            _enemyHealth.Init(2);
        }
    }
}
using System;
using UnityEngine;
using Скриптерсы.Enemy;

namespace Скриптерсы
{
    public class Babka: MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private GameObject GameObject;

        private void OnEnable()
        {
            _enemyHealth.OnDeath += HandleDeath;
        }
        private void OnDisable()
        {
            _enemyHealth.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            Destroy(GameObject);
        }

        private void Awake()
        {
            _enemyHealth.Init(1);
        }
    }
}
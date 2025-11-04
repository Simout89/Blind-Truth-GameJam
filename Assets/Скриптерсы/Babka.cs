using System;
using UnityEngine;
using Скриптерсы.Enemy;

namespace Скриптерсы
{
    public class Babka: MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;

        private void Awake()
        {
            _enemyHealth.Init(3);
        }
    }
}
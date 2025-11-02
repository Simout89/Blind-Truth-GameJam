using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/EnemyData")]
    public class EnemyData: ScriptableObject
    {
        [field: SerializeField] public float Health { get; private set; } = 3f;
        [field: SerializeField] public float MaxRangePursuit { get; private set; } = 15f;
        [field: SerializeField] public float DefaultSpeed { get; private set; } = 5f;
        [field: SerializeField] public float PursuitSpeed { get; private set; } = 15f;
        [field: SerializeField] public float AttackRange { get; private set; } = 1f;
        [field: SerializeField] public float DelayBeforeAttack { get; private set; } = 1f;
        [field: SerializeField] public float DelayAfterAttack { get; private set; } = 1f;
        [field: SerializeField] public float Damage { get; private set; } = 3f;
        [field: SerializeField] public float AttackZoneRadius { get; private set; } = 3f;
        [field: Header("Sounds")]
        [field: SerializeField] public string AttackSound { get; private set; }
        [field: SerializeField] public string DeathSound { get; private set; }
        [field: SerializeField] public string FootStepSound { get; private set; }
        [field: SerializeField] public string TakeDamageSound { get; private set; }


    }
}
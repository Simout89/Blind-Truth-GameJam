using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/EnemyData")]
    public class EnemyData: ScriptableObject
    {
        [field: SerializeField] public float Health { get; private set; } = 3f;
        [field: SerializeField] public float MaxRangePursuit { get; private set; } = 15f;
        [field: SerializeField] public float AttackRange { get; private set; } = 1f;
        [field: SerializeField] public float DelayBeforeAttack { get; private set; } = 1f;
        [field: SerializeField] public float Damage { get; private set; } = 3f;
        [field: SerializeField] public float AttackZoneRadius { get; private set; } = 3f;

    }
}
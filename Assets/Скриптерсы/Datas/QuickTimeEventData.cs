using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/QuickTimeEventData")]
    public class QuickTimeEventData: ScriptableObject
    {
        [field: SerializeField] public float TimeOnQTE { get; private set; } = 10f;
        [field: SerializeField] public float MaxValue { get; private set; } = 5f;
        [field: SerializeField] public float RateOfDecrease { get; private set; } = 1f;
        [field: SerializeField] public float RateOfIncrease { get; private set; } = 1f;

    }
}
using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/CharacterData")]
    public class CharacterControllerData: ScriptableObject
    {
        [field: Header("Base")]
        [field: SerializeField] public float Health { get; private set; } = 100f;
        [field: Header("Weapon")]
        [field: SerializeField] public float Damage { get; private set; } = 1;
        [field: SerializeField] public int MaxAmmoInClip { get; private set; } = 7;
        [field: SerializeField] public float DelayBetweenShots { get; private set; } = 1f;
        [field: Header("Move")]
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
        [field: SerializeField] public float Gravity { get; private set; } = -9.81f;
        [field: SerializeField] public float CrouchMultiplayer { get; private set; } = 0.5f;
        [field: Header("Sprint")]
        [field: SerializeField] public float SprintMultiplayer { get; private set; } = 1.2f;
        [field: SerializeField] public float MaxValue { get; private set; } = 100f;
        [field: SerializeField] public float RateOfDecrease { get; private set; } = 1.2f;
        [field: SerializeField] public float RateOfIncrease { get; private set; } = 1.2f;

    }
}
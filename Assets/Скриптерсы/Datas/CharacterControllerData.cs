using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/CharacterData")]
    public class CharacterControllerData: ScriptableObject
    {
        [field: SerializeField] public float Health { get; private set; } = 100f;
        [field: SerializeField] public float Damage { get; private set; } = 1;
        [Header("Move")]
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
        [field: SerializeField] public float Gravity { get; private set; } = -9.81f;
        [field: SerializeField] public float SprintMultiplayer { get; private set; } = 1.2f;
        [field: SerializeField] public float CrouchMultiplayer { get; private set; } = 0.5f;

    }
}
using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/AmmoBoxData")]
    public class AmmoBoxData: ScriptableObject
    {
        [field: SerializeField] public int AmmoCount { get; private set; } = 10;
    }
}
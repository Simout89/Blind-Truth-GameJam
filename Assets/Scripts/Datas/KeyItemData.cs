using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/KeyItemData")]
    public class KeyItemData: ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; } = "www";
        [field: SerializeField] public string PickUpSound { get; private set; }
        [field: SerializeField] public string textOnSubtitles { get; private set; }
    }
}
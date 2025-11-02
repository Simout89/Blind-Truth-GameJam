using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/NoteData")]
    public class NoteData: ScriptableObject
    {
        [field: SerializeField][field: TextArea] public string text { get; private set; }
    }
}
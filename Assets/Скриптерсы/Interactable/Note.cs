using UnityEngine;
using Zenject;
using Скриптерсы.Datas;
using Скриптерсы.View;

namespace Скриптерсы.Interactable
{
    public class Note: HighlightableObject, IClickable
    {
        [SerializeField] private NoteData _noteData;
        [Inject] private NoteView _noteView;
        
        public void Click()
        {
            _noteView.TryShowNote(_noteData);
        }

        public override void Highlight()
        {
            base.Highlight();
        }

        public override void RemoveHighlight()
        {
            base.RemoveHighlight();
        }
    }
}
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
        
        public ClickResult Click()
        {
            _noteView.TryShowNote(_noteData);

            return new ClickResult(false);
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
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Скриптерсы.Datas;
using Скриптерсы.View;

namespace Скриптерсы.Interactable
{
    public class Note: HighlightableObject, IClickable
    {
        [SerializeField] private NoteData _noteData;
        [Inject] private NoteView _noteView;

        [SerializeField] private bool hideClickableViewAfterUse;
        [SerializeField] private UnityEvent clickEvent;
        
        public ClickResult Click()
        {
            _noteView.TryShowNote(_noteData);
            
            clickEvent?.Invoke();

            return new ClickResult(hideClickableViewAfterUse);
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
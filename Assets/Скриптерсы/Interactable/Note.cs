using System.Collections;
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

            StartCoroutine(Delay());
            

            return new ClickResult(hideClickableViewAfterUse);
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(3f);
            
            clickEvent?.Invoke();
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
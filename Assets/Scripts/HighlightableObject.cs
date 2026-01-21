using UnityEngine;

namespace Скриптерсы
{
    [RequireComponent(typeof(Outline))]
    public class HighlightableObject: MonoBehaviour, IHighlightable
    {
        [SerializeField] private Color highlightColor = Color.white;
        [SerializeField] private float highlightWidth = 10f;
        
        private Outline _outline;

        private void Awake()
        {
            _outline = gameObject.GetComponent<Outline>();
            _outline.OutlineColor = highlightColor;
            _outline.OutlineWidth = highlightWidth;
            _outline.enabled = false;
        }
        
        public virtual void Highlight()
        {
            _outline.enabled = true;
        }

        public virtual void RemoveHighlight()
        {
            _outline.enabled = false;
        }
    }
    
    public interface IHighlightable
    {
        void Highlight();
        void RemoveHighlight();
    }

}
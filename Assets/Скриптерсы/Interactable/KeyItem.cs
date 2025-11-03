using UnityEngine;
using Zenject;
using Скриптерсы.Datas;

namespace Скриптерсы.Interactable
{
    public class KeyItem: HighlightableObject, IClickable
    {
        [SerializeField] private KeyItemData _keyItemData;
        [Inject] private PlayerInteraction _playerInteraction;
        public ClickResult Click()
        {
            _playerInteraction.AddKey(_keyItemData);
            
            Destroy(gameObject);
            
            return new ClickResult(true);
        }
    }
}
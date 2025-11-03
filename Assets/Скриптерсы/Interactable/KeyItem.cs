using FMODUnity;
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
            
            if(string.IsNullOrEmpty(_keyItemData.PickUpSound))
                RuntimeManager.PlayOneShot(_keyItemData.PickUpSound);
            
            Destroy(gameObject);
            
            return new ClickResult(true);
        }
    }
}
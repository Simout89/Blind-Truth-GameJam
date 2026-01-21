using FMODUnity;
using UnityEngine;
using Zenject;
using Скриптерсы.Datas;
using Скриптерсы.View;

namespace Скриптерсы.Interactable
{
    public class KeyItem: HighlightableObject, IClickable
    {
        [SerializeField] private KeyItemData _keyItemData;


        [Inject] private SubtitlesView _subtitlesView;
        [Inject] private PlayerInteraction _playerInteraction;
        public ClickResult Click()
        {
            _playerInteraction.AddKey(_keyItemData);
            
            if(!string.IsNullOrEmpty(_keyItemData.PickUpSound))
                RuntimeManager.PlayOneShot(_keyItemData.PickUpSound, transform.position);
            
            if(!string.IsNullOrEmpty(_keyItemData.textOnSubtitles))
                _subtitlesView.ShowText(_keyItemData.textOnSubtitles);
            
            Destroy(gameObject);
            
            return new ClickResult(true);
        }
    }
}
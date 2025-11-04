using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Скриптерсы.Datas;
using Скриптерсы.View;

namespace Скриптерсы.Interactable
{
    public class KeyReceiver: HighlightableObject, IClickable
    {
        [SerializeField] private KeyItemData[] _keyItemData;
        [Inject] private PlayerInteraction _playerInteraction;
        [SerializeField] public UnityEvent OnSolved;
        [SerializeField] private string UnCorrectEvent;
        [SerializeField] private string CorrectEvent;
        [SerializeField] private string AlreadySolved;
        private bool isSolved = false;
        [SerializeField] private bool deleteAfterSolved;

        [SerializeField] private string subtitlesTextUnCorrect;

        [Inject] private SubtitlesView _subtitlesView;
        
        public ClickResult Click()
        {
            if (isSolved)
            {
                if(AlreadySolved != "")
                    RuntimeManager.PlayOneShot(AlreadySolved);
                return new ClickResult(false, false);
            }
            
            for (int i = 0; i < _keyItemData.Length; i++)
            {
                bool flag = false;
                for (int j = 0; j < _playerInteraction.KeyItemDatas.Count; j++)
                {
                    if (_keyItemData[i] == _playerInteraction.KeyItemDatas[j])
                    {
                        flag = true;
                    }
                }

                if (!flag)
                {
                    if(UnCorrectEvent != "")
                        RuntimeManager.PlayOneShot(UnCorrectEvent);

                    if (subtitlesTextUnCorrect != "")
                    {
                        _subtitlesView.ShowText(subtitlesTextUnCorrect);
                    }
                    
                    return new ClickResult(false, false);
                }
            }
            isSolved = true;
            OnSolved?.Invoke();
            if(CorrectEvent != "")
                RuntimeManager.PlayOneShot(CorrectEvent);

            if (deleteAfterSolved)
            {
                Destroy(gameObject);
                return new ClickResult(true, false);
            }
            return new ClickResult(false, false);
        }
    }
}
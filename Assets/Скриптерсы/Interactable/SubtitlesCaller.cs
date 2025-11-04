using UnityEngine;
using Zenject;
using Скриптерсы.View;

namespace Скриптерсы.Interactable
{
    public class SubtitlesCaller: MonoBehaviour
    {
        [Inject] private SubtitlesView _subtitlesView;

        public void ShowSubtitles(string text)
        {
            _subtitlesView.ShowText(text);
        }
    }
}
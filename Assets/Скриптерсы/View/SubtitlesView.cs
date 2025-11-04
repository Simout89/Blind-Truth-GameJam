using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Скриптерсы.View
{
    public class SubtitlesView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float fadeDuration = 1f;     // время исчезновения
        [SerializeField] private float visibleAfterType = 1f; // сколько держится до затухания
        [SerializeField] private float charDelay = 0.03f;     // задержка между буквами

        private Tween _fadeTween;
        private Coroutine _typingCoroutine;

        public void ShowText(string text)
        {
            // отменяем все прошлые анимации
            _fadeTween?.Kill();
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);

            _canvasGroup.gameObject.SetActive(true);
            _canvasGroup.alpha = 1f;
            _text.text = "";

            // запускаем печать текста по буквам
            _typingCoroutine = StartCoroutine(TypeText(text));
        }

        private IEnumerator TypeText(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                _text.text += text[i];
                yield return new WaitForSeconds(charDelay);
            }

            // после завершения печати ждем и начинаем плавно скрывать
            yield return new WaitForSeconds(visibleAfterType);

            _fadeTween = _canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                _canvasGroup.gameObject.SetActive(false);
            });
        }
    }
}
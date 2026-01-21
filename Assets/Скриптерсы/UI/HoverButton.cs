using DG.Tweening;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.25f;
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private Ease easeType = Ease.OutSine;

    [Header("FMOD Events")]
    [SerializeField] private EventReference hoverSound;
    [SerializeField] private EventReference clickSound;

    private TextMeshProUGUI _text;
    private Vector3 _defaultScale;
    private Color _defaultColor;
    private bool _isInteractable = true;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        if (_text == null)
        {
            Debug.LogError("TextMeshProUGUI не найден в дочерних объектах кнопки!");
            return;
        }

        _defaultScale = _text.rectTransform.localScale;
        _defaultColor = _text.color;
    }

    private void OnDisable()
    {
        // При отключении кнопки сбрасываем состояние
        ResetButtonState();
    }

    /// <summary>
    /// Сброс масштаба и цвета текста в исходное состояние
    /// </summary>
    public void ResetButtonState()
    {
        _text.rectTransform.DOKill(true);
        _text.DOKill(true);

        _text.rectTransform.localScale = _defaultScale;
        _text.color = _defaultColor;
        _text.ForceMeshUpdate();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        _text.rectTransform.DOKill(true);
        _text.DOKill(true);

        // Масштаб
        _text.rectTransform.DOScale(_defaultScale * scaleMultiplier, animationDuration).SetEase(easeType);

        // Цвет через TMP
        _text.DOColor(Color.white, animationDuration).SetEase(easeType).OnUpdate(() =>
        {
            _text.ForceMeshUpdate(); // гарантируем обновление цвета вершин
        });

        if (hoverSound.IsNull == false)
            RuntimeManager.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        _text.rectTransform.DOKill(true);
        _text.DOKill(true);

        _text.rectTransform.DOScale(_defaultScale, animationDuration).SetEase(easeType);
        _text.DOColor(_defaultColor, animationDuration).SetEase(easeType).OnUpdate(() =>
        {
            _text.ForceMeshUpdate();
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        if (clickSound.IsNull == false)
            RuntimeManager.PlayOneShot(clickSound);
    }

    /// <summary>
    /// Включить или отключить интерактивность кнопки
    /// </summary>
    public void SetInteractable(bool value)
    {
        _isInteractable = value;
        if (!_isInteractable)
        {
            ResetButtonState();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using TMPro;
using UnityEngine.EventSystems;

public class SliderManager : MonoBehaviour, IPointerUpHandler
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private string busPath;

    [SerializeField]
    private EventReference previewSound; // звук, который проигрывается при отпускании

    private FMOD.Studio.Bus bus;

    private void Start()
    {
        if (!string.IsNullOrEmpty(busPath))
        {
            bus = RuntimeManager.GetBus(busPath);
            bus.getVolume(out float volume);
            slider.value = volume * slider.maxValue;
            UpdateSliderOutput();
        }

        // Можно обновлять текст при движении ползунка
        slider.onValueChanged.AddListener(delegate { UpdateSliderOutput(); });
    }

    public void UpdateSliderOutput()
    {
        if (text != null && slider != null)
        {
            float percentage = (slider.value / slider.maxValue) * 100f;
            text.text = $"{percentage:0}%";
            bus.setVolume(slider.value / slider.maxValue);
        }
    }

    // IPointerUpHandler — вызывается когда игрок отпустил ползунок
    public void OnPointerUp(PointerEventData eventData)
    {
        if (previewSound.IsNull == false)
            RuntimeManager.PlayOneShot(previewSound);
    }
}

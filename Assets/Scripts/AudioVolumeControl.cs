using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioVolumeControl : MonoBehaviour
{
    const string VolumeKey = "MasterVolume";

    public Slider slider;
    public TMP_Text valueLabel;

    void Start()
    {
        float saved = PlayerPrefs.GetFloat(VolumeKey, 1f);
        AudioListener.volume = saved;

        if (slider != null)
            slider.SetValueWithoutNotify(saved);

        UpdateLabel(saved);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VolumeKey, value);
        UpdateLabel(value);
    }

    void UpdateLabel(float value)
    {
        if (valueLabel != null)
            valueLabel.text = Mathf.RoundToInt(value * 100f).ToString();
    }
}

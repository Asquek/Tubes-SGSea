using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio Mixer Reference")]
    public AudioMixer myMixer;

    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Hubungkan fungsi perubahan slider saat game dimulai
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        // Mengubah nilai linear slider (0-1) menjadi desibel (-80 hingga 0)
        myMixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
    }

    public void SetBGMVolume(float value)
    {
        myMixer.SetFloat("BGMVol", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        myMixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
    }
}
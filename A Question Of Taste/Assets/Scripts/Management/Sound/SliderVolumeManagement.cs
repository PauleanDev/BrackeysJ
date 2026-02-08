using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEditor.Overlays;

public class SliderVolumeManagement : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] AudioMixer _mixer;

    // Modifiers
    float _masterVolume;
    float _musicVolume;
    float _sFXVolume;

    public void Awake()
    {
        LoadData();
    }

    public void SetMasterVolume(Slider slider)
    {
        _masterVolume = slider.value;
        _mixer.SetFloat("Master", _masterVolume);
    }

    public void SetMusicVolume(Slider slider)
    {
        _musicVolume = slider.value;
        _mixer.SetFloat("Music", _musicVolume);
    }

    public void SetSFXVolume(Slider slider)
    {
        _sFXVolume = slider.value;
        _mixer.SetFloat("SFX", _sFXVolume);
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", _sFXVolume);
    }

    public void LoadData()
    {
        _mixer.SetFloat("Master", PlayerPrefs.GetFloat("MasterVolume"));
        _mixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume"));
        _mixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFXVolume"));
    }
}

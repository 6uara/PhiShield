using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class OptionsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource musicaudioMixer;
    public AudioSource sfXaudioMixer;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    
    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] _resolutions;
    
    void Start()
    {
        InitializeAudioSettings();
        InitializeResolutionSettings();
    }
    
    private void InitializeAudioSettings()
    {
        // Configurar listeners para los sliders
        sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }
    
    private void InitializeResolutionSettings()
    {
        // Obtener resoluciones disponibles
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height + " @ " + _resolutions[i].refreshRateRatio + "Hz";
            options.Add(option);
            
            if (_resolutions[i].width == Screen.currentResolution.width && 
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        
        // Configurar listener para el dropdown
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }
    
    public void SetSfxVolume(float volume)
    {
        sfXaudioMixer.volume = volume;
        SaveSystem.Instance.SaveData();
    }

    public void SetMusicVolume(float volume)
    {
        musicaudioMixer.volume = volume;
        SaveSystem.Instance.SaveData();
    }
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        SaveSystem.Instance.SaveData();
    }
    
    // Método adicional para alternar pantalla completa (opcional)
    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SaveSystem.Instance.SaveData();
    }
}

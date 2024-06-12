using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    //[SerializeField] private SettingsStorage settingsStorage; 
    //[SerializeField] private AudioMixer audioMixer;
    private Resolution[] resolutions;
    [SerializeField] private Dropdown resolutionDropdown;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Toggle fullscreenToggle;

    // Start is called before the first frame update
    void Start()
    {
        //settingsStorage = FindAnyObjectByType<SettingsStorage>();
        if (SettingsStorage.Instance != null) {
            SetResolutionSettings(SettingsStorage.Instance.GetResolution().width, SettingsStorage.Instance.GetResolution().height, SettingsStorage.Instance.GetFullscreen());
            SettingsStorage.Instance.SetAllSettings();
            musicSlider.value = SettingsStorage.Instance.GetMusicVolume();
            fullscreenToggle.isOn = SettingsStorage.Instance.GetFullscreen();
        } else 
        {Debug.Log("No instance of SettingsStorage");}  
            
    }

    private void SetResolutionSettings (int width, int height, bool isFullscreen) {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions(); //clear dropdown options

        int currentResolutionIndex = 0;
        Screen.SetResolution(width, height, isFullscreen);

        //convert resolutions to string array
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height ) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMusicVolume (float volume) {
        if (SettingsStorage.Instance != null) 
            SettingsStorage.Instance.SetMusicVolume(volume);
    }

    public void SetResolution(int resolutionIndex) {
        if (SettingsStorage.Instance != null)
            SettingsStorage.Instance.SetResolution(resolutions[resolutionIndex]);
    }

    public void SetFullscreen(bool flag) {
        if (SettingsStorage.Instance != null)
            SettingsStorage.Instance.SetFullscreen(flag);
    }
}

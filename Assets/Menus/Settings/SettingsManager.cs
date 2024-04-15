using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private Resolution[] resolutions;
    [SerializeField] private Dropdown resolutionDropdown;

    private static Resolution currentResolution;
    private static float currentMusicVolume;

    void Start () {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions(); //clear dropdown options

        int currentResolutionIndex = 0;
        Screen.SetResolution(1920, 1080, Screen.fullScreen);

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
        audioMixer.SetFloat("Volume", volume);
    }
    public void SetSoundVolume (float volume) {
        
    }

    public void SetResolution(int resolutionIndex) {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool flag) {
        Screen.fullScreen = flag;
    }

}

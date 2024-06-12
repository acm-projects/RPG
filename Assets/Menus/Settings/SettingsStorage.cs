using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsStorage : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private Resolution[] resolutions;
    [SerializeField] private Dropdown resolutionDropdown;

    private static int currResWidth = 1920;
    private static int currResHeight = 1080;
    private static float currentMusicVolume = 0;
    private static bool isFullscreen = true;

    public float testVol;

    public static SettingsStorage Instance;

    private void Awake()
    {
        
        // if manager was already created, destroy
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Sets all setting values to the current recorded ones
    public void SetAllSettings () {
        audioMixer.SetFloat("Volume", currentMusicVolume);
        Screen.SetResolution(currResWidth, currResHeight, isFullscreen);
        Screen.fullScreen = isFullscreen;
    }

    private void Start()
    {
        //Instance = this;
        //SetResolutionSettings();
        SetMusicVolume(currentMusicVolume);
        SetFullscreen(isFullscreen);



        testVol = currentMusicVolume;
        //DontDestroyOnLoad(gameObject);

    }
    /*private void SetResolutionSettings () {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions(); //clear dropdown options

        int currentResolutionIndex = 0;
        Screen.SetResolution(currResWidth, currResHeight, Screen.fullScreen);

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
    }*/

    public void SetMusicVolume (float volume) {
        currentMusicVolume = volume;
        testVol = currentMusicVolume;
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetResolution(Resolution res) {
        currResWidth = res.width;
        currResHeight = res.height;
        Screen.SetResolution(currResWidth, currResHeight, isFullscreen);   
    }

    public void SetFullscreen(bool flag) {
        isFullscreen = flag;
        Screen.fullScreen = flag;
    }

    public Resolution GetResolution() {
        Resolution res = new Resolution();
        res.width = currResWidth;
        res.height = currResHeight;
        return res;
    }

    public float GetMusicVolume() {
        return currentMusicVolume;
    }

    public bool GetFullscreen() {
        return isFullscreen;
    }

}

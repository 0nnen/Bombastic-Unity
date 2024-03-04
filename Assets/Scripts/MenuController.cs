using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using System.Globalization;

public class MenuController : MonoBehaviour
{

/*--------------------------VOLUME------------------------------*/

    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float defaultVolume =1.0f;

/*--------------------------GAMEPLAY------------------------------*/

    [SerializeField] private TMP_Text controllerSenTextValue =null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;

    /*--------------------------Graphics------------------------------*/

    [SerializeField]private Slider brightnessSlider = null;
    [SerializeField]private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    public TMP_Dropdown resolutionDropdDown;
    private Resolution[] resolutions;

    [SerializeField] private TMP_Dropdown qualityDropDown;
    [SerializeField] private Toggle fullScreenToggle;


    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[1].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdDown.AddOptions(options);
        resolutionDropdDown.value = currentResolutionIndex;
        resolutionDropdDown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ExitGameDialogYes()
    {
        Application.Quit();
    }

    public void ExitButton()
        {
            Application.Quit();
        }



    /*--------------------------VOLUME------------------------------*/


    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume",AudioListener.volume);
        // StartCoroutine(ConfirmationBox());

    }

    /*--------------------------GAMEPLAY------------------------------*/

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen= Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");

    }

    public void GameplayApply()
    {
        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
        // StartCoroutine(ConfirmationBox());
    }

    /*--------------------------Graphics------------------------------*/

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscree",(_isFullScreen?1:0));
        Screen.fullScreen = _isFullScreen;

        // StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType) {
        if (MenuType =="Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if (MenuType =="Gameplay")
        {
            controllerSenTextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            GameplayApply();
        }

        if (MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropDown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;
        }
    }
   /* public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }*/
}


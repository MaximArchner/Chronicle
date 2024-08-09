using Cinemachine.PostFX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SettingsManager : MonoBehaviour
{
    public Button backButton;


    #region || Sounds & Music ||
    public Slider masterSlider;
    public GameObject masterValue;

    public Slider effectsSlider;
    public GameObject effectsValue;

    public Slider musicSlider;
    public GameObject musicValue;

    #endregion

    #region || Display Settings ||

    public Toggle fullscreenTog, vsyncTog;

    public List<ResItem> resolutions = new List<ResItem>();

    public TMP_Dropdown resolution;

    public int selectedResolution;

    #endregion
    public static SettingsManager Instance { get; set; }

    private void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        backButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
            print("Settings saved successfully...");
        });

        StartCoroutine(LoadAndApplySettings());
    }
    private IEnumerator LoadAndApplySettings()
    {
        LoadAndSetVolume();
        yield return new WaitForSeconds(0.1f);
    }
    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();
        masterSlider.value = volumeSettings.master;
        musicSlider.value = volumeSettings.music;
        effectsSlider.value = volumeSettings.effects;

        print("Volume Settings loaded successfully...");
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "%";
        effectsValue.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "%";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "%";

        ApplyGraphics();
    }

    public void ResChoice()
    {
        selectedResolution = resolution.value;
    }

    public void ApplyGraphics()
    {
        //Screen.fullScreen = fullscreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}

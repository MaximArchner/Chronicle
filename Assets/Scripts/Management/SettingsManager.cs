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

    public Slider masterSlider;
    public GameObject masterValue;

    public Slider effectsSlider;
    public GameObject effectsValue;

    public Slider musicSlider;
    public GameObject musicValue;
    public static SettingsManager Instance { get; set; }

    private void Start()
    {
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
    }
}

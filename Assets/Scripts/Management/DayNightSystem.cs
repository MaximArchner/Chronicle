using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{

    public Light directionalLight;

    public float dayDurationInSeconds = 24.0f; // Gunun uzunlugunu belirleme
    public int currentHour = 0;
    float currentTimeOfDay = 0.0f;

    public List<SkyBoxTimeMapping> timeMappings;
    public static DayNightSystem Instance { get; set; }

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

    void Update()
    {
        // Anlik oyun ici zamani hesapla
        currentTimeOfDay += (Time.deltaTime / dayDurationInSeconds);
        currentTimeOfDay %= 1;

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        // DirectionalLight'in gelis yonunu gunun saatine gore guncelleme
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));

        // SkyboxMaterial'i gunun saatine gore guncelleme
        UpdateSkyBox();
    }

    private void UpdateSkyBox()
    {
        // Mevcut saat icin iliskili skybox materyalini bul
        Material currentSkybox = null;
        foreach (SkyBoxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;
                break;
            }
        }

        if (currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }
    }
}

[System.Serializable]
public class SkyBoxTimeMapping
{
    public string phaseName;
    public int hour; // Gunun hangi saatindeyiz (0-23)
    public Material skyboxMaterial; // O saate karsilik gelecek gokyuzu
}

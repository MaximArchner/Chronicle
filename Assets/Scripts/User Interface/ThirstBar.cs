using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstBar : MonoBehaviour
{
    private Slider slider;
    public Text ThirstCounter;

    public GameObject playerState;

    private float currentThirstPercent, maxThirstPercent;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        currentThirstPercent = playerState.GetComponent<PlayerState>().currentThirstPercent;
        maxThirstPercent = playerState.GetComponent<PlayerState>().maxThirstPercent;

        float fillValue = currentThirstPercent / maxThirstPercent; // orantisal olarak 0 ile 1 arasinda olacak (slider component icin)
        slider.value = fillValue;

        ThirstCounter.text = currentThirstPercent + "%";
    }
}

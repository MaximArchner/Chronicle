using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    private Slider slider;
    public Text HungerCounter;

    public GameObject playerState;

    private float currentHunger, maxHunger;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        currentHunger = playerState.GetComponent<PlayerState>().currentHunger;
        maxHunger = playerState.GetComponent<PlayerState>().maxHunger;

        float fillValue = currentHunger / maxHunger; // orantisal olarak 0 ile 1 arasinda olacak (slider component icin)
        slider.value = fillValue;

        HungerCounter.text = currentHunger + "%";

    }
}

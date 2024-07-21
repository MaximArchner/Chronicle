using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class ResourceHealthBar : MonoBehaviour
{
    private Slider slider;
    private float currentHealth, maxHealth;

    public GameObject GlobalState;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        //currentHealth = playerState.GetComponent<GlobalState>().currentHealth;
        //maxHealth = playerState.GetComponent<GlobalState>().maxHealth;

        float fillValue = currentHealth / maxHealth; // orantisal olarak 0 ile 1 arasinda olacak (slider component icin)
        slider.value = fillValue;

    }
}

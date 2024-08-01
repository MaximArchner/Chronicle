using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider slider;
    public Text HealthCounter;

    public GameObject playerState;

    private float currentHealth, maxHealth;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth; // orantisal olarak 0 ile 1 arasinda olacak (slider component icin)
        slider.value = fillValue;

        HealthCounter.text = currentHealth + "/" + maxHealth; // burada asýl sayýlar gozukecek, 54/100 gibi
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ResourceHealthBar : MonoBehaviour
{
    private Slider slider;
    public float currentHealth, maxHealth;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // --- Oyuncu Cani --- //
    public float currentHealth;
    public float maxHealth;

    // --- Oyuncu Acligi --- //
    public float currentHunger;
    public float maxHunger;

    public float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    // --- Oyuncu Susamisligi --- //
    public float currentThirstPercent;
    public float maxThirstPercent;

    public bool isThirstActive;

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

    private void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirstPercent = maxThirstPercent;

        StartCoroutine(decreaseThirst());
    }

    IEnumerator decreaseThirst()
    {
        while (true)
        {
            currentThirstPercent -= 1;
            yield return new WaitForSeconds(5);
        }
    }
    
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentHunger -= 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHunger -= 0.05f * maxHunger;
        }
    }
}

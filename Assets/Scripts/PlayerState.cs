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

    IEnumerator decreaseThirst() // Coroutine'in hangi araliklarla gerceklesecegini belirliyor (su an 5 saniyede bir -1 veriyor Thirst'e)
    {
        while (true)
        {
            currentThirstPercent -= 1;
            yield return new WaitForSeconds(5);
        }
    }
    
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition); //ilk pozisyonumuz ile son pozisyonumuz arasindaki farki kaydedecek
        lastPosition = playerBody.transform.position; //son pozisyonumuzu, yukaridaki hesap icin ilk pozisyonumuz olarak sifirlayacak

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentHunger -= 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 0.05f * maxHealth;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            currentHealth += 0.05f * maxHealth;
        }
    }
}

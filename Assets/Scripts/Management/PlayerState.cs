using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject mainMapCamera;
    public GameObject mainMapCanvas;
    public bool mainIsOpen;

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
        if (!SceneManager.GetActiveScene().name.Contains("Island"))
        {
            mainMapCamera = null;
        }

        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirstPercent = maxThirstPercent;

        StartCoroutine(decreaseThirst());
        if (mainMapCamera != null)
        {
            mainMapCamera.SetActive(false);
        }
        mainIsOpen = false;

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

        if (distanceTravelled >= 50)
        {
            distanceTravelled = 0;
            currentHunger -= 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            currentHealth -= 0.05f * maxHealth;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            currentHealth += 0.05f * maxHealth;
        }

        if (SceneManager.GetActiveScene().name == "Island")
        {
            if (Input.GetKeyDown(KeyCode.M) && !mainIsOpen)
            {
                mainIsOpen = true;
                mainMapCamera.SetActive(true);
                mainMapCanvas.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.M) && mainIsOpen)
            {
                mainMapCamera.SetActive(false);
                mainMapCanvas.SetActive(false);
                mainIsOpen = false;
            }
        }
    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setHunger(float newHunger)
    {
        currentHunger = newHunger;
    }

    public void setThirst(float newThirst)
    {
        currentThirstPercent = newThirst;
    }
}

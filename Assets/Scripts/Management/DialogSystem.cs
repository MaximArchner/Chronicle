using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public GameObject dialogUI;
    public TextMeshProUGUI dialogText;
    public Button option1;
    public Button option2;
    public Button option3;
    public static DialogSystem instance { get; set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {

        dialogText = GetComponent<TextMeshProUGUI>();
        option1 = GetComponent<Button>();
        option2 = GetComponent<Button>();
        option3 = GetComponent<Button>();
    }
}

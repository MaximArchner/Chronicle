using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlertDialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public TextMeshProUGUI messageText;
    public Button proceedButton;
    public Button cancelButton;

    private System.Action<bool> responseCallback;

    public void Start()
    {
        dialogBox.SetActive(false);

        proceedButton.onClick.AddListener(() => HandleResponse(true));
        cancelButton.onClick.AddListener(() => HandleResponse(false));

    }

    public void ShowDialog(string message, System.Action<bool> callback)
    {
        responseCallback = callback;
        messageText.text = message;
        dialogBox.SetActive(true);
    }

    public void HandleResponse(bool response)
    {
        dialogBox.SetActive(false);
        responseCallback?.Invoke(response);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button LoadGameButton;
    public void NewGame()
    {
        SceneManager.LoadScene(sceneName: "Island");
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

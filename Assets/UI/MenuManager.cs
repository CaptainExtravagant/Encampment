using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject canvasMainMenu;
    public GameObject canvasOptionsMenu;

    private void Start()
    {
        LoadMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadOptionsMenu()
    {
        canvasMainMenu.SetActive(false);
        canvasOptionsMenu.SetActive(true);
    }

    public void LoadMainMenu()
    {
        canvasOptionsMenu.SetActive(false);
        canvasMainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

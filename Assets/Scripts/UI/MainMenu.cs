using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject TutoMenu;
    public GameObject CreditMenu;
    public string MainLevel;

    private void Start()
    {
        TutoMenu.SetActive(false);
        CreditMenu.SetActive(false);
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene(MainLevel, LoadSceneMode.Single);
    }

    public void OnTuto()
    {
        gameObject.SetActive(false);
        TutoMenu.SetActive(true);
    }

    public void OnCredits()
    {
        gameObject.SetActive(false);
        CreditMenu.SetActive(true);
    }
    public void OnQuit()
    {
        Application.Quit();
    }

    public void BackToMain()
    {
        gameObject.SetActive(true);
        TutoMenu.SetActive(false);
        CreditMenu.SetActive(false);
    }
}

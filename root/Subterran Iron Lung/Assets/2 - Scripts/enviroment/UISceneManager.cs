using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISceneManager : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayTheGame()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.LoadScene("LevelOne");
        }
    }

    public void Proceed()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void RetryLevel()
    {
        if (SceneManager.GetActiveScene().name == "LoseScreenOne")
        {
            SceneManager.LoadScene("LevelOne");
        }
        if (SceneManager.GetActiveScene().name == "LoseScreenTwo")
        {
            SceneManager.LoadScene("LevelTwo");
        }
        if (SceneManager.GetActiveScene().name == "LoseScreenThree")
        {
            SceneManager.LoadScene("LevelThree");
        }
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "WinSceneOne")
        {
            SceneManager.LoadScene("LevelTwo");
        }
        if (SceneManager.GetActiveScene().name == "WinSceneTwo")
        {
            SceneManager.LoadScene("LevelThree");
        }
    }

    public void Instructions()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.LoadScene("Instructions");
        }
    }

    public void MainMenu()
    {
        if (SceneManager.GetActiveScene().name == "Instructions")
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (SceneManager.GetActiveScene().name == "WinSceneOne")
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (SceneManager.GetActiveScene().name == "WinSceneTwo")
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (SceneManager.GetActiveScene().name == "WinSceneThree")
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (SceneManager.GetActiveScene().name == "LoseScreenOne")
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (SceneManager.GetActiveScene().name == "LoseScreenTwo")
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (SceneManager.GetActiveScene().name == "LoseScreenThree")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game has been Quit");
    }
}
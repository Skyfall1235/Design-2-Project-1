using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private SceneData[] scenes;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject optionsPanel;
    bool optionsIsOpen;
    bool creditsIsOpen;

    public void PlayTheGame()
    {
        Debug.Log("attempting to Play");
        Debug.Log(SceneManager.GetActiveScene().name == scenes[0].scene);
        if (SceneManager.GetActiveScene().name == scenes[0].scene)
        {
            SceneManager.LoadScene(scenes[1].scene);
            Debug.Log("Scene Should Load");
        }
    }

    public void Proceed()
    {
        if (SceneManager.GetActiveScene().name == "Title_Screen")
        {
            SceneManager.LoadScene(scenes[0].scene);
        }
    }

    public void RetryLevel()
    {
        if (SceneManager.GetActiveScene().name == "LoseScreenOne")
        {
            SceneManager.LoadScene(scenes[1].scene);
        }
        if (SceneManager.GetActiveScene().name == "LoseScreenTwo")
        {
            SceneManager.LoadScene(scenes[2].scene);
        }
        if (SceneManager.GetActiveScene().name == "LoseScreenThree")
        {
            SceneManager.LoadScene(scenes[3].scene);
        }
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "WinSceneOne")
        {
            SceneManager.LoadScene(scenes[2].scene);
        }
        if (SceneManager.GetActiveScene().name == "WinSceneTwo")
        {
            SceneManager.LoadScene(scenes[3].scene);
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
        if(SceneManager.GetActiveScene().name != scenes[0].scene)
        {
            SceneManager.LoadScene(scenes[0].scene);
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game has been Quit");
    }

    public void Options()
    {
        optionsPanel.SetActive(true);
        Debug.Log("options open");
        optionsIsOpen = true;
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
        creditsIsOpen = true;
    }

    public void ReturnToMenu()
    {
        if (optionsIsOpen == true)
        {
            optionsPanel.SetActive(false);
            optionsIsOpen = false;
        }
        if (creditsIsOpen == true)
        {
            creditsPanel.SetActive(false);
            creditsIsOpen = false;
        }
    }

    [System.Serializable]
    public class SceneData
    {
         public string scene;
        public bool isActive; // You might want to include additional scene-related data
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] public AsyncLoader loader;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject optionsPanel;
    bool optionsIsOpen;
    bool creditsIsOpen;


    public void PlayTheGame()
    {
        string sceneName = loader.sceneNames[3];
        loader.LoadSceneWithFade(sceneName, true);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{
    public List<string> sceneNames = new List<string>();
    public Image fadeImage;
    public GameObject canvas;
    public float fadeDuration = 1.0f;

    private bool isFading = false;

    private void Awake()
    {
        // Make sure the fadeImage is clear upon first load
        SetFadeAlpha(0.0f);
        StartCoroutine(LoadMainMenuAsync());
    }

    private IEnumerator LoadMainMenuAsync()
    {
        // Load the main menu scene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNames[1], LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Get the loaded main menu scene
        Scene mainMenuScene = SceneManager.GetSceneByName(sceneNames[1]);

        if (mainMenuScene.isLoaded)
        {
            // Find the Canvas within the loaded main menu scene
            MainMenuUI canvasInMainMenu = null;
            GameObject[] rootObjects = mainMenuScene.GetRootGameObjects();

            foreach (GameObject rootObject in rootObjects)
            {
                canvasInMainMenu = rootObject.GetComponentInChildren<MainMenuUI>();
                if (canvasInMainMenu != null)
                {
                    canvasInMainMenu.loader = this;

                    break;
                }
            }
            if (canvasInMainMenu != null)
            {
                // Do something with the Canvas
                Debug.Log("Found Canvas in the main menu scene.");
            }
            else
            {
                Debug.LogWarning("Canvas not found in the main menu scene.");
            }
        }
        else
        {
            Debug.LogError("Failed to load the main menu scene.");
        }
    }

    public void LoadSceneWithFade(string sceneName, bool includePlayer)
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndLoad(sceneName, includePlayer));
        }
    }

    private IEnumerator FadeOutAndLoad(string sceneName, bool includePlayer)
    {
        canvas.SetActive(true);
        isFading = true;
        // Fade out to black
        yield return StartCoroutine(Fade(0.0f, 1.0f));

        // Unload all scenes except for the Persistent Scene (if needed)
        UnloadAllScenesExceptPersistent();

        // Load the new scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if(includePlayer)
        {
            AsyncOperation playerLoad = SceneManager.LoadSceneAsync(sceneNames[0], LoadSceneMode.Additive);

            while (!playerLoad.isDone)
            {
                yield return null;
            }
        }
        
        // Set the new scene as the active scene
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(loadedScene);

        // Fade back in
        yield return StartCoroutine(Fade(1.0f, 0.0f));
        isFading = false;
        canvas.SetActive(false);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            SetFadeAlpha(alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha value is set
        SetFadeAlpha(endAlpha);
    }

    private void SetFadeAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }

    private void UnloadAllScenesExceptPersistent()
    {
        Debug.Log("Unloading all scenes except manager and those in sceneNames list.");

        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != "SceneManager")
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene);

                unloadOperation.completed += (operation) =>
                {
                    if (unloadOperation.isDone)
                    {
                        Debug.Log("Unloaded scene: " + scene.name);
                    }
                    else
                    {
                        Debug.LogError("Failed to unload scene: " + scene.name);
                    }
                };
            }
        }
    }

}



using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;
using System.ComponentModel;
using System.Reflection;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level List")]
    public List<string> levelNames = new List<string>(); // Add scene names here

    [SerializeField] public int currentLevelIndex = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }

        currentLevelIndex = levelNames.FindIndex(s => s.Contains(SceneManager.GetActiveScene().name));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ReloadCurrentLevel();
        if (Input.GetKeyDown(KeyCode.K))
            LoadNextLevel();
    }

    /// <summary>
    /// Load the next level in the list
    /// </summary>
    public void LoadNextLevel()
    {
        if (levelNames.Count == 0) return;

        if(currentLevelIndex == 4)
            StartCoroutine(TransitionToEnding());
        else if (currentLevelIndex < levelNames.Count) // loop around
            LoadLevel(currentLevelIndex+1);
            
    }

    IEnumerator TransitionToEnding()
    {
        GetComponent<AudioSource>().Play();

        Time.timeScale = 0f;

        GameObject player = GameObject.Find("DefaultCharacter");
        if (player != null && player.activeSelf)
            player.GetComponent<PlayerInput>().enabled = false;

        yield return new WaitForSecondsRealtime(0.5f);

        var fm = FadeManager.instance;
        fm.StartCoroutine(fm.FadeTo(1, 1, 0.7f));

        var cm = CameraManager.instance;
        cm.StartCoroutine(cm.EndLevelCamera());

        yield return new WaitForSecondsRealtime(1.8f);

        Time.timeScale = 1f;

        isTransitioning = false;

        GameManager.Instance.CreateUniqueManagers();

        GameObject temp = new GameObject("DDOL_Cleanup_Helper");
        DontDestroyOnLoad(temp);

        Scene ddolScene = temp.scene;

        foreach (GameObject root in ddolScene.GetRootGameObjects())
        {
            // Skip the helper itself
            if (root == temp) continue;

            Destroy(root);
        }

        Destroy(temp);

        SceneManager.LoadScene("Ending");
    }

    /// <summary>
    /// Load a specific level by index
    /// </summary>
    public void LoadLevel(int index, bool skipCameraTransition = false)
    {
        StartCoroutine(LevelTransition(index, skipCameraTransition));
    }

    private bool isTransitioning = false;
    private IEnumerator LevelTransition(int index, bool skipCameraTransition)
    {
        if (isTransitioning)
            yield break;

        isTransitioning = true;

        if(skipCameraTransition)
        {
            var fm = FadeManager.instance;
            fm.StartCoroutine(fm.FadeTo(1, 1f));

            yield return new WaitForSecondsRealtime(1.2f);

            if (index < 0 || index >= levelNames.Count)
            {
                Debug.LogWarning("Level index out of range!");
                yield break;
            }

            currentLevelIndex = index;
            SceneManager.LoadScene(levelNames[index]);

            isTransitioning = false;

            GameManager.Instance.CreateUniqueManagers();
        }

        else
        {
            GetComponent<AudioSource>().Play();

            Time.timeScale = 0f;

            GameObject player = GameObject.Find("DefaultCharacter");
            if (player != null && player.activeSelf)
                player.GetComponent<PlayerInput>().enabled = false;

            yield return new WaitForSecondsRealtime(0.5f);

            var fm = FadeManager.instance;
            fm.StartCoroutine(fm.FadeTo(1, 1, 0.7f));

            var cm = CameraManager.instance;
            cm.StartCoroutine(cm.EndLevelCamera());

            yield return new WaitForSecondsRealtime(1.8f);

            if (index < 0 || index >= levelNames.Count)
            {
                Debug.LogWarning("Level index out of range!");
                yield break;
            }

            Time.timeScale = 1f;

            currentLevelIndex = index;
            SceneManager.LoadScene(levelNames[index]);

            isTransitioning = false;

            GameManager.Instance.CreateUniqueManagers();
        }

        
    }

    /// <summary>
    /// Load the current level again (restart)
    /// </summary>
    public void ReloadCurrentLevel()
    {
        LoadLevel(currentLevelIndex, true);
    }
}
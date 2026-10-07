using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject levelManagerPrefab;
    public GameObject cursorManagerPrefab;
    public GameObject cameraManagerPrefab;
    public GameObject fadeManagerPrefab;
    public GameObject musicManagerPrefab;
    public GameObject enemiesManagerPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            GameObject levelManager = Instantiate(levelManagerPrefab);
            levelManager.transform.parent = transform;

            GameObject musicManager = Instantiate(musicManagerPrefab);
            musicManager.transform.parent = transform;

            //CreateUniqueManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnEnable()
    {
        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CreateUniqueManagers();
    }

    public void CreateUniqueManagers() // Are remade every scene change.
    {
        GameObject cursorManager = Instantiate(cursorManagerPrefab);
        //cursorManager.transform.parent = transform;

        GameObject cameraManager = Instantiate(cameraManagerPrefab);
        //cameraManager.transform.parent = transform;

        GameObject fadeManager = Instantiate(fadeManagerPrefab);
        //fadeManager.transform.parent = transform;

        GameObject enemiesManager = Instantiate(enemiesManagerPrefab);
        //enemiesManager.transform.parent = transform;
    }
}

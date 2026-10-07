using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sourceA;
    public AudioSource sourceB;

    public AudioClip clipA;
    public AudioClip clipB;
    public AudioClip clipC;
    public AudioClip clipD;

    public static MusicManager Instance = null;

    [Header("Crossfade Settings")]
    public float crossfadeDuration = 0.5f; // Duration of the crossfade in seconds

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
        CheckRightTracks();
    }

    private void Start()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else {
            Destroy(gameObject);
            return;
        }

        CheckRightTracks();

        if (sourceA != null && sourceB != null)
        {
            sourceA.loop = true;
            sourceB.loop = true;

            sourceA.Play();
            sourceB.Play();

            // Start with trackA audible
            sourceA.volume = 1f;
            sourceB.volume = 0f;
        }
        else {
            Debug.LogError("Assign both AudioSources in the inspector.");
        }
    }
    public void CheckRightTracks()
    {
        if (LevelManager.Instance.currentLevelIndex < 3)
        {
            if (sourceA.clip != clipA || sourceB.clip != clipB)
            {
                sourceA.clip = clipA;
                sourceB.clip = clipB;

                sourceA.Stop(); sourceB.Stop();
                sourceA.Play(); sourceB.Play();
            }
        }
        else if (LevelManager.Instance.currentLevelIndex >= 3 && LevelManager.Instance.currentLevelIndex < 5)
        {

            if (sourceA.clip != clipC || sourceB.clip != clipD)
            {
                sourceA.clip = clipC;
                sourceB.clip = clipD;

                sourceA.Stop(); sourceB.Stop();
                sourceA.Play(); sourceB.Play();
            }
        }

        else if (LevelManager.Instance.currentLevelIndex >= 5)
        {
            sourceA.mute = true;
            sourceB.mute = true;

            clipA = null; clipB = null; clipC = null; clipD = null;
        }
    }

    public void TransitionToTrack(int num)
    {
        if (num == 1 && sourceA.volume == 1f)
            return;

        if (num == 2 && sourceB.volume == 1f)
            return;

        if (num == 1)
            StartCoroutine(Crossfade(sourceB, sourceA));
        else
            StartCoroutine(Crossfade(sourceA, sourceB));
    }

    public IEnumerator Crossfade(AudioSource from, AudioSource to)
    {
        float time = 0f;

        while (time < crossfadeDuration)
        {
            time += Time.deltaTime;
            float t = time / crossfadeDuration;

            from.volume = Mathf.Lerp(1f, 0f, t);
            to.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // Ensure volumes are exact at the end
        from.volume = 0f;
        to.volume = 1f;
    }
}
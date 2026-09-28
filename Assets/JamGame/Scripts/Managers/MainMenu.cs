using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        FadeManager.instance.FadeTo(0, 2);
    }
    public void PlayGame()
    {
        audioSource.Play();
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        //FadeManager.instance.StopAllCoroutines();

        FadeManager.instance.StartCoroutine(FadeManager.instance.FadeTo(1, 2, 0.5f));
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        audioSource.Play();
        Application.Quit();
    }

    public GameObject info;
    public void Info()
    {
        audioSource.Play();
        info.SetActive(!info.activeSelf);
    }
}

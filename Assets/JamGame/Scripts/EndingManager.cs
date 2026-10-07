using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Ending());
    }

    private bool canInput = false;
    private bool isEnding = false;
    private void Update()
    {
        if (canInput && !isEnding)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) { 
                StartCoroutine(QuitToMainMenu());
                isEnding = true;
            }
        }
    }

    private IEnumerator QuitToMainMenu()
    {
        //FadeManager.instance.StopAllCoroutines();
        //FadeManager.instance.FadeTo(1, 3);

        //yield return new WaitForSeconds(4);

        SceneManager.LoadScene("Main Menu");

        yield break;
    }

    private IEnumerator Ending()
    {
        GameObject.Find("DefaultCharacter").GetComponent<PlayerInput>().enabled = false;

        FadeManager.instance.StopAllCoroutines();
        
        yield return new WaitForSeconds(5f);

        var textTrigger = GameObject.Find("Text Trigger");
        textTrigger.GetComponent<BoxCollider>().enabled = true; yield return new WaitForSeconds(7f);

        StartCoroutine(FadeOutSmooth(textTrigger.GetComponent<TypewriterOnTrigger>().textComponent, 2f)); yield return new WaitForSeconds(2f);

        yield return new WaitForSeconds(1f);


        FadeManager.instance.StartCoroutine(FadeManager.instance.FadeTo(0, 3));

        yield return new WaitForSeconds(4f);

        GameObject.Find("Starting Camera").GetComponent<CinemachineVirtualCamera>().Priority = 0;
        GameObject.Find("Normal Camera").GetComponent<CinemachineVirtualCamera>().Priority = 20;

        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(2f);

        canInput = true;
    }

    public static IEnumerator FadeOutSmooth(TextMeshProUGUI text, float duration)
    {
        float time = 0f;

        // Get initial color
        Color startColor = text.color;
        float startAlpha = startColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // SmoothStep interpolation (ease in-out)
            float alpha = Mathf.SmoothStep(startAlpha, 0f, t);

            text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        // Ensure fully transparent at the end
        text.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField] GameObject fadeObjectPrefab;
    GameObject fadeObject;
    SpriteRenderer spriteRenderer;

    public static FadeManager instance;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        fadeObject = Instantiate(fadeObjectPrefab);
        fadeObject.transform.parent = transform;

        spriteRenderer = fadeObject.GetComponent<SpriteRenderer>();

        spriteRenderer.gameObject.SetActive(true);

        StartCoroutine(FadeTo(0, 1.0f, 0.5f, false));
    }

    public IEnumerator FadeTo(float targetAlpha, float fadeDuration, float delay = 0f, bool realTime = true)
    {
        print("Got a fade request");

        if(realTime)
            yield return new WaitForSecondsRealtime(delay);
        else
            yield return new WaitForSeconds(delay);

        float startAlpha = spriteRenderer.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / fadeDuration;

            float alpha = Mathf.SmoothStep(startAlpha, targetAlpha, t);

            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;

            yield return null;
        }

        // ensure exact final value
        Color finalColor = spriteRenderer.color;
        finalColor.a = targetAlpha;
        spriteRenderer.color = finalColor;
    }
}

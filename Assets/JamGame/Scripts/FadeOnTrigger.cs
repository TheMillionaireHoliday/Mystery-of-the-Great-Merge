using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOnTrigger : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    public float insideAlpha = 0.0f; // transparent when inside
    public float outsideAlpha = 0.5f;

    public bool isTop = true;

    private SpriteRenderer spriteRenderer;
    private Coroutine fadeRoutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetDefaultValues();
    }

    void SetDefaultValues()
    {
        BoxCollider triggerCollider = GetComponent<BoxCollider>();

        // Use physics overlap to detect initial state
        Collider[] hits = Physics.OverlapBox(
            triggerCollider.bounds.center,
            triggerCollider.bounds.extents,
            triggerCollider.transform.rotation
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Color _c = spriteRenderer.color;
                _c.a = insideAlpha;
                spriteRenderer.color = _c;

                return;
            }
        }

        Color c = spriteRenderer.color;
        c.a = outsideAlpha;
        spriteRenderer.color = c;

        return;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade(insideAlpha); // fade OUT (transparent)

            int num = isTop ? 1 : 2;

            MusicManager.Instance.TransitionToTrack(num);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade(outsideAlpha); // fade IN (opaque)
        }
    }

    void StartFade(float targetAlpha)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeTo(targetAlpha));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = spriteRenderer.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
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

        fadeRoutine = null;
    }
}
using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterOnTrigger : MonoBehaviour
{
    [Header("Text Settings")]
    public TextMeshProUGUI textComponent;
    [HideInInspector] public string message;
    public float typingSpeed = 0.05f;

    private bool hasTriggered = false;
    private Coroutine typingCoroutine;

    public AudioSource audioSource;

    private void Start()
    {
        textComponent.text = textComponent.text;

        textComponent.maxVisibleCharacters = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(RevealText());
        }
    }

    IEnumerator RevealText()
    {
        textComponent.ForceMeshUpdate(); // Ensure text info is up to date
        int totalCharacters = textComponent.textInfo.characterCount;

        textComponent.maxVisibleCharacters = 0;

        audioSource.Play();

        for (int i = 0; i <= totalCharacters; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }

        audioSource.Stop();
    }
}
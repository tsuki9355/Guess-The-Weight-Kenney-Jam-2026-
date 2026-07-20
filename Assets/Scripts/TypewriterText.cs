using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    public TMP_Text targetText;
    public float charactersPerSecond = 30f;

    [Header("Optional: a soft tick per character")]
    public AudioClip typeSound;
    [Range(0f, 1f)] public float typeSoundVolume = 0.3f;

    AudioSource audioSource;
    Coroutine typingCoroutine;
    string fullText;

    void Awake()
    {
        if (typeSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void StartTyping(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        fullText = text;
        typingCoroutine = StartCoroutine(TypeRoutine());
    }

    public void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        targetText.text = fullText;
    }

    IEnumerator TypeRoutine()
    {
        targetText.text = "";
        float delay = 1f / charactersPerSecond;

        for (int i = 0; i < fullText.Length; i++)
        {
            targetText.text += fullText[i];

            if (audioSource != null && typeSound != null && !char.IsWhiteSpace(fullText[i]))
                audioSource.PlayOneShot(typeSound, typeSoundVolume);

            yield return new WaitForSeconds(delay);
        }

        typingCoroutine = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    // Reference to the persistent audio source object
    public AudioSource audioSource;

    // Volume of the audio
    public float volume = 0.5f;

    // Fade duration in seconds
    public float fadeDuration = 1f;

    // Start playing the audio
    public void StartAudio()
    {
        audioSource.volume = volume;
        audioSource.Play();
    }

    // Stop playing the audio
    public void StopAudio()
    {
        audioSource.Stop();
    }

    // Fade in the audio over the specified duration
    public void FadeInAudio()
    {
        StartCoroutine(FadeAudio(audioSource.volume, volume, fadeDuration));
    }

    // Fade out the audio over the specified duration
    public void FadeOutAudio()
    {
        StartCoroutine(FadeAudio(audioSource.volume, 0f, fadeDuration));
    }

    // Coroutine to fade the audio in or out
    private IEnumerator
    FadeAudio(float startVolume, float endVolume, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, t);
            yield return null;
        }

        audioSource.volume = endVolume;
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class
ButtonHoverSound
: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioClip hoverSound;

    private AudioSource audioSource;

    public float volume = 0.5f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = hoverSound;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot (hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Additional logic if needed when the pointer exits the button
    }
}

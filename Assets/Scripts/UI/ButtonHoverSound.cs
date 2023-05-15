using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioClip hoverSound;
    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = hoverSound;
        audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData) {
        // Additional logic if needed when the pointer exits the button
    }
}

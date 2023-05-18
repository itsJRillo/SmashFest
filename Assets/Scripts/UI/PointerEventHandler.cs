using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    public Image image;
    public Sprite hoverSprite;
    private Sprite originalSprite;

    void Start(){
        originalSprite = image.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = originalSprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyTraining : MonoBehaviour {
    public Sprite[] sprites;
    public KeyCode[] keys;
    public Image scrollViewContent;
    public float spriteDuration = 2f;
    public float scrollSpeed = 50f;

    private Image currentSprite;
    private float spriteTimer;

    private void Update()
    {
        // Iterar sobre todas las teclas
        for (int i = 0; i < keys.Length; i++)
        {
            // Si se presiona la tecla correspondiente
            if (Input.GetKeyDown(keys[i]))
            {
                // Crear un nuevo sprite en el ScrollView
                CreateSprite(i, 17, 10f, 10f);

                // Reiniciar el temporizador
                spriteTimer = spriteDuration;
            }
        }

        // Actualizar el temporizador y comprobar si debe ocultarse el sprite actual
        spriteTimer -= Time.deltaTime;
        if (spriteTimer <= 0f && currentSprite != null)
        {
            Destroy(currentSprite.gameObject);
            currentSprite = null;
        }

        // Desplazar el contenido del ScrollView
        scrollViewContent.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }

    private void CreateSprite(int index, int layerOrder, float width, float height)
    {
        // Crear un nuevo GameObject con la imagen del sprite
        GameObject spriteObject = new GameObject("Key");
        spriteObject.transform.SetParent(scrollViewContent.transform);
        currentSprite = spriteObject.AddComponent<Image>();
        currentSprite.sprite = sprites[index];

        currentSprite.canvas.sortingOrder = layerOrder;

        // Posicionar el sprite en el ScrollView
        RectTransform rectTransform = currentSprite.rectTransform;
        rectTransform.localPosition = Vector3.zero;

        // Establecer el tamaño del sprite
        rectTransform.sizeDelta = new Vector2(width, height); // Ajusta el tamaño según tus necesidades

        // Opcional: Puedes ajustar otras propiedades, como el color, la escala, etc.
        // currentSprite.color = Color.white;
        // spriteObject.transform.localScale = Vector3.one;
    }
}

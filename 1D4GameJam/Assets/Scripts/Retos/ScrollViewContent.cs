using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewContent : MonoBehaviour
{
    public RectTransform buttonParent;
    public RectTransform content;

    private int attempts = 0; // Variable para contar los intentos

    void Start()
    {
        buttonParent.GetComponent<TransformObserver>().OnChildChanged += UpdateContentHeight;

        // Llamar a UpdateContentHeight al inicio para asegurar el tamaño correcto del Content
        UpdateContentHeight();
    }

    public void UpdateContentHeight()
    {
        // Verificar si hay al menos 5 hijos en el buttonParent
        if (buttonParent.childCount < 5)
        {
            // Incrementar el número de intentos
            attempts++;

            // Si el número de intentos es menor a un cierto límite, reintentar después de un breve tiempo
            if (attempts < 10) // Cambia el número según la cantidad de intentos que desees
            {
                StartCoroutine(WaitAndRetry());
                return;
            }
        }

        // Reiniciar el contador de intentos
        attempts = 0;

        // Obtener la posición vertical del primer y último hijo
        float firstChildPosY = buttonParent.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y;
        float lastChildPosY = buttonParent.GetChild(buttonParent.childCount - 1).GetComponent<RectTransform>().anchoredPosition.y;

        // Calcular la altura total como la diferencia entre las posiciones verticales del primer y último hijo
        float totalHeight = Mathf.Abs(lastChildPosY - firstChildPosY) + 80f;

        // Ajustar la altura del Content
        Vector2 sizeDelta = content.sizeDelta;
        sizeDelta.y = totalHeight;
        content.sizeDelta = sizeDelta;
    }

    IEnumerator WaitAndRetry()
    {
        // Esperar un breve tiempo antes de reintentar
        yield return new WaitForSeconds(0.2f); // Cambia el tiempo según sea necesario

        // Llamar a UpdateContentHeight para reintentar
        UpdateContentHeight();
    }
}

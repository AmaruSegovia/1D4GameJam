using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class MoverBoton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float moveSpeed = 5f; // Velocidad de movimiento del botón
    public float moveAmount = 25f; // Cuánto quieres mover el botón cuando se pasa el ratón por encima
    private Vector2 originalAnchoredPosition; // La posición original del botón
    private Vector2 targetAnchoredPosition; // La posición objetivo del botón

    void Start()
    {
        originalAnchoredPosition = GetComponent<RectTransform>().anchoredPosition; // Almacena la posición original del botón
        targetAnchoredPosition = originalAnchoredPosition; // La posición objetivo inicial es la original
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Calcular la nueva posición objetivo cuando el ratón entra en el botón
        targetAnchoredPosition = originalAnchoredPosition - Vector2.right * moveAmount;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaurar la posición original del botón cuando el ratón sale de él
        targetAnchoredPosition = originalAnchoredPosition;
    }

    void Update()
    {
        // Mover el botón gradualmente hacia la posición objetivo
        GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition, targetAnchoredPosition, Time.deltaTime * moveSpeed);
    }
}

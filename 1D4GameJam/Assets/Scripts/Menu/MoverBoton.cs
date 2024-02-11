using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class MoverBoton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float moveSpeed = 5f; // Velocidad de movimiento del bot�n
    public float moveAmount = 25f; // Cu�nto quieres mover el bot�n cuando se pasa el rat�n por encima
    private Vector2 originalAnchoredPosition; // La posici�n original del bot�n
    private Vector2 targetAnchoredPosition; // La posici�n objetivo del bot�n

    void Start()
    {
        originalAnchoredPosition = GetComponent<RectTransform>().anchoredPosition; // Almacena la posici�n original del bot�n
        targetAnchoredPosition = originalAnchoredPosition; // La posici�n objetivo inicial es la original
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Calcular la nueva posici�n objetivo cuando el rat�n entra en el bot�n
        targetAnchoredPosition = originalAnchoredPosition - Vector2.right * moveAmount;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaurar la posici�n original del bot�n cuando el rat�n sale de �l
        targetAnchoredPosition = originalAnchoredPosition;
    }

    void Update()
    {
        // Mover el bot�n gradualmente hacia la posici�n objetivo
        GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition, targetAnchoredPosition, Time.deltaTime * moveSpeed);
    }
}

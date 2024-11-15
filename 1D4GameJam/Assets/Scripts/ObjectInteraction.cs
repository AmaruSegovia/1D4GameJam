
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite nuevoSprite; 

    private void Start()
    {
        // Encuentra el GameManager en la escena
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
    }

    private void OnMouseDown()
    {
        // El objeto es presionado
        HandleInteraction();
    }

    private void OnMouseUp()
    {
        // El objeto es liberado
        HandleRelease();
    }

    public void OnTouchDown()
    {
        // El objeto es tocado
        HandleInteraction();
    }

    public void OnTouchUp()
    {
        // El objeto es liberado
        HandleRelease();
    }

    private void HandleInteraction()
    {
        // Llamar al método ObjectPressed del GameManager y pasar la referencia del objeto
        gameManager.ObjectPressed(gameObject);
        spriteRenderer.sprite = nuevoSprite;
    }

    private void HandleRelease()
    {
        // Llamar al método ObjectReleased del GameManager y pasar la referencia del objeto
        gameManager.ObjectReleased(gameObject);
        spriteRenderer.sprite = null;
    }
}

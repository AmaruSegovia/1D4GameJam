using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputHandler : MonoBehaviour
{
    private void Update()
    {
        // Manejar toques en cada frame
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                HandleTouchDown(touch.position);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                HandleTouchUp(touch.position);
            }
        }

        // Manejar clic de ratón
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouchDown(Input.mousePosition);
        }

        // Manejar liberación del ratón
        if (Input.GetMouseButtonUp(0))
        {
            HandleTouchUp(Input.mousePosition);
        }
    }

    private void HandleTouchDown(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && hit.collider.CompareTag("Area"))
        {
            // Obtener el componente ObjectInteraction y manejar el toque
            ObjectInteraction objectInteraction = hit.collider.GetComponent<ObjectInteraction>();
            objectInteraction.OnTouchDown();
        }
    }

    private void HandleTouchUp(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && hit.collider.CompareTag("Area"))
        {
            // Obtener el componente ObjectInteraction y manejar la liberación
            ObjectInteraction objectInteraction = hit.collider.GetComponent<ObjectInteraction>();
            objectInteraction.OnTouchUp();
        }
    }
}
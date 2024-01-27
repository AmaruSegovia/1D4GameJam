using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string playerTag;
    private bool canMove = false;
    private Rigidbody2D rb;
    private Transform boundaryHolder;
    private Boundary playerBoundary;

    public bool PuedeMoverse { get => canMove; set => canMove = value; }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Intenta encontrar el objeto BoundaryHolder
        GameObject foundObject = GameObject.Find(gameObject.name + "BoundaryHolder");

        // Si se encuentra el objeto BoundaryHolder, obtenemos su transform y almacenamos sus valores en playerBoundary
        if (foundObject != null)
        {
            boundaryHolder = foundObject.transform;
            playerBoundary = new Boundary(
                boundaryHolder.GetChild(0).position.y,
                boundaryHolder.GetChild(1).position.y,
                boundaryHolder.GetChild(2).position.x,
                boundaryHolder.GetChild(3).position.x
            );
        }
        else
        {
            // Si no se encuentra el objeto, mostramos un mensaje de error
            Debug.LogError("No se encontró el objeto BoundaryHolder para el jugador " + gameObject.name);
        }
    }

    private void Update()
    {
        if (canMove)
        {
            MoverJugadorConEntradaTactil();
        }
    }

    private void MoverJugadorConEntradaTactil()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch tactil = Input.GetTouch(i);
            Vector2 newPosition = Camera.main.ScreenToWorldPoint(tactil.position);

            // Limitador de espacio a tocar
            if (newPosition.x >= playerBoundary.Left - 1.5 && newPosition.x <= playerBoundary.Right + 1.5 &&
                newPosition.y >= -5 && newPosition.y <= 5)
            {
                LimitarMovimiento(newPosition);
            }
        }
    }

    private void LimitarMovimiento(Vector2 newPosition)
    {
        // Mathf.clamp = restringir el valor, ej: newPosition.x entre playerBoundary.Left y playerBoundary.Right.
        Vector2 clampedMousePos = new Vector2(Mathf.Clamp(newPosition.x, playerBoundary.Left, playerBoundary.Right), 
                                              Mathf.Clamp(newPosition.y, playerBoundary.Down, playerBoundary.Up));
        rb.MovePosition(clampedMousePos); // Mueve el objeto en la posicion restringida
    }
}

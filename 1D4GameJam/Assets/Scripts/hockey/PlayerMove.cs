using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform BoundaryHolder;
    Boundary playerBoundary;

    struct Boundary
    {
        public float Up, Down, Left, Right;
        public Boundary(float up, float down, float left, float right)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        BoundaryHolder = GameObject.Find("BoundaryHolder").transform;
        playerBoundary = new Boundary(BoundaryHolder.GetChild(0).position.y,
                                      BoundaryHolder.GetChild(1).position.y,
                                      BoundaryHolder.GetChild(2).position.x,
                                      BoundaryHolder.GetChild(3).position.x );
    }

    private void OnMouseDrag()
    {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Limitador de espacio a tocar
        if (newPosition.x >= -10f && newPosition.x <= 10f && newPosition.y >= -5.4f && newPosition.y <= 5.4f)
        {
            // Limitador de movimiento
            Vector2 clamedMousePos = new Vector2(Mathf.Clamp(newPosition.x,playerBoundary.Left, playerBoundary.Right),
                                                Mathf.Clamp(newPosition.y,playerBoundary.Down, playerBoundary.Up));
            rb.MovePosition(clamedMousePos);
        }

    }

}

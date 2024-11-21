using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool drag;
    private Vector2 MousePos;
    public string ingredientID;

    private void Update()
    {
        if (drag)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(MousePos);
        }
    }

    private void OnMouseDown()
    {
        drag = true;
    }

    private void OnMouseUp()
    {
        drag = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Potion"))
        {
            Debug.Log($"Ingrediente {ingredientID} colisionó con la poción");
            PotionManager.Instance.CheckIngredient(ingredientID);
            drag = false;
            gameObject.SetActive(false); // Desactivar en lugar de destruir
        }
    }
}

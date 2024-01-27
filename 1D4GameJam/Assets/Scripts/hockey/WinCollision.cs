using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollision : MonoBehaviour
{
    public UIController UI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Llego");
        if (other.gameObject.name == "Ball")
        {
            // Activar el Canvas de felicitaciones
            UI.ActivateWinnerPanel();

            // Desactivar la bola
            other.gameObject.SetActive(false);

            GameObject.Find("Player1").GetComponent<PlayerMove>().PuedeMoverse = false;
            GameObject.Find("Player2").GetComponent<PlayerMove>().PuedeMoverse = false;
        }
    }
}

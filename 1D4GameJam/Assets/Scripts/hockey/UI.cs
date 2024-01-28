using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject ActivateWinnerPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("entrtooooooo");
        if (other.gameObject.name == "Ball")
        {
            // Activar el Canvas de felicitaciones
            ActivateWinnerPanel.gameObject.SetActive(true);

            // Desactivar la bola
            other.gameObject.SetActive(false);

            //Invoke("ReiniciarJuego", 2f);
        }
    }
}

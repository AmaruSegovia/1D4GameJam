using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManos : MonoBehaviour
{
    //Marcar en el editor para el objeto de la izquierda
    [SerializeField] private bool isLeftPlayer;
    private JuegoManosManager manager;
    private GameObject zonaIzq, zonaDer;

    private void Start()
    {
        manager = FindObjectOfType<JuegoManosManager>();
        //zonaIzq = GameObject.Find("ZonaIzq");
        //zonaDer = GameObject.Find("ZonaDer");
    }

    public void inputJugadorIzq()
    {
        if (manager.isPlayable)
        {
            if (manager.isCoinOnTable)
            {
                manager.moneda.SetActive(false);
                Debug.Log("Gan� el jugador de la izquierda");
                Debug.Log("Ac� va el reto que el jugador de la derecha debe cumplir");
            } else
            {
                Debug.Log("El jugador de la izquierda toc� demasiado pronto");
                Debug.Log("Gan� el jugador de la derecha");
                Debug.Log("Ac� va el reto que el jugador de la izquierda debe cumplir");
            }
            manager.isPlayable = false;
        }
    }

    public void inputJugadorDer()
    {
        if (manager.isPlayable)
        {
            if (manager.isCoinOnTable)
            {
                manager.moneda.SetActive(false);
                Debug.Log("Gan� el jugador de la derecha");
                Debug.Log("Ac� va el reto que el jugador de la izquierda debe cumplir");
            }
            else
            {
                Debug.Log("El jugador de la derecha toc� demasiado pronto");
                Debug.Log("Gan� el jugador de la izquierda");
                Debug.Log("Ac� va el reto que el jugador de la derecha debe cumplir");
            }
            manager.isPlayable = false;
        }

    }
}

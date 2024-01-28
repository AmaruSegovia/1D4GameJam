#if UNITY_EDITOR

using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManos : MonoBehaviour
{
    //Marcar en el editor para el objeto de la izquierda
    [SerializeField] private bool isLeftPlayer;
    private JuegoManosManager manager;
    private GameObject pataIzq, pataDer, textoBlanco, textoNegro;

    private void Start()
    {
        manager = FindObjectOfType<JuegoManosManager>();

        pataIzq = GameObject.Find("PataGatoIzq");
        pataDer = GameObject.Find("PataGatoDer");
        textoBlanco = GameObject.Find("TextoBlanco");
        textoNegro = GameObject.Find("TextoNegro");

        textoBlanco.SetActive(false);
        textoNegro.SetActive(false);
    }

    public void inputJugadorIzq()
    {
        if (manager.isPlayable)
        {
            MostrarTextos();
            pataIzq.transform.position = new Vector2(-5, 0);

            if (manager.isCoinOnTable)
            {
                manager.moneda.SetActive(false);
                textoNegro.GetComponent<TextMeshProUGUI>().text = "¡Has ganado :D!";
                textoBlanco.GetComponent<TextMeshProUGUI>().text = "No tocaste a tiempo :(";
            } else
            {
                textoNegro.GetComponent<TextMeshProUGUI>().text = "¡Tocaste demasiado pronto!";
                textoBlanco.GetComponent<TextMeshProUGUI>().text = "¡Has ganado :D!";
            }

            manager.isPlayable = false;
        }

        ////DEBUG
        //else
        //{
        //    Reiniciar();
        //}
    }

    public void inputJugadorDer()
    {
        if (manager.isPlayable)
        {
            MostrarTextos();
            pataDer.transform.position = new Vector2(5, 0);

            if (manager.isCoinOnTable)
            {
                manager.moneda.SetActive(false);
                textoBlanco.GetComponent<TextMeshProUGUI>().text = "¡Has ganado :D!";
                textoNegro.GetComponent<TextMeshProUGUI>().text = "No tocaste a tiempo :(";
            }
            else
            {
                textoBlanco.GetComponent<TextMeshProUGUI>().text = "¡Tocaste demasiado pronto!";
                textoNegro.GetComponent<TextMeshProUGUI>().text = "¡Has ganado :D!";
            }

            manager.isPlayable = false;
        }

        ////DEBUG
        //else
        //{
        //    Reiniciar();
        //}
    }

    public void MostrarTextos()
    {
        textoBlanco.SetActive(true);
        textoNegro.SetActive(true);
    }

    //DEBUG, método para reiniciar el juego sin salir
    public void Reiniciar()
    {
        //Reseteo de las variables del manager
        manager.moneda.SetActive(true);
        manager.isPlayable = true;
        manager.isCoinOnTable = true;

        //Reseteo de los textos
        textoBlanco.GetComponent<TextMeshProUGUI>().text = "";
        textoNegro.GetComponent<TextMeshProUGUI>().text = "";

        //Reseteo de las posiciones de las patas
        pataIzq.transform.position = new Vector2(-10, 0);
        pataDer.transform.position = new Vector2(10, 0);

        //Reinicio de la corrutina para arrancar el juego
        StartCoroutine(manager.SpawnCoin());
    }
}

#endif
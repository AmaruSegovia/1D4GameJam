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
    }

    public void MostrarTextos()
    {
        textoBlanco.SetActive(true);
        textoNegro.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Contador : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contador;
    [SerializeField] private int cont = 3;

    void Start()
    {
        StartCoroutine(ConteoRegresivo());
    }
    IEnumerator ConteoRegresivo()
    {
        while (cont > 0)
        {
            contador.text = cont.ToString();
            cont--;
            yield return new WaitForSeconds(1f);
            ConteoRegresivo();
        }
        contador.text = "YA";
        yield return new WaitForSeconds(1f);
        GameObject.Find("Conteo").SetActive(false);
        GameObject.Find("Player1").GetComponent<PlayerMove>().PuedeMoverse = true;
        GameObject.Find("Player2").GetComponent<PlayerMove>().PuedeMoverse = true;
    }
}

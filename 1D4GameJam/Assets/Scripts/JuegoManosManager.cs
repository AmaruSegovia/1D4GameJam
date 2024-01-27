using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuegoManosManager : MonoBehaviour
{
    public bool isPlayable;
    public bool isCoinOnTable;
    public GameObject moneda;


    private void Start()
    {
        moneda = GameObject.Find("Moneda");

        isPlayable = true;
        isCoinOnTable = false;
        moneda.SetActive(false);

        StartCoroutine(SpawnCoin());
        Debug.Log("Se leyó la línea para iniciar la corrutina");
    }

    public IEnumerator SpawnCoin()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2.2f));
        Debug.Log("Inicia la corrutina");
        if (isPlayable) moneda.SetActive(true);
        isCoinOnTable = true;
    }
}

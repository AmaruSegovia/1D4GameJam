using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JuegoManosManager : MonoBehaviour
{
    public bool isPlayable;
    public bool isCoinOnTable;
    public bool gameFinished;
    public GameObject moneda, textoReto;

    private List<string> challenges;


    private void Start()
    {
        moneda = GameObject.Find("Moneda");
        textoReto = GameObject.Find("TextoReto");

        gameFinished = false;
        isPlayable = true;
        isCoinOnTable = false;
        moneda.SetActive(false);
        textoReto.SetActive(false);

        StartCoroutine(SpawnCoin());
        Debug.Log("Se leyó la línea para iniciar la corrutina");
    }

    public void Update()
    {
        if (gameFinished && !textoReto.activeSelf)
        {
            TextAsset challengesJson = Resources.Load<TextAsset>("Challenges");
            ChallengesData challengesData = JsonUtility.FromJson<ChallengesData>(challengesJson.text);
            challenges = challengesData.Challenges;

            textoReto.GetComponent<TextMeshProUGUI>().text += challenges[Random.Range(0, challenges.Count)];
            textoReto.SetActive(true);
        }
    }

    public IEnumerator SpawnCoin()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2.2f));
        Debug.Log("Inicia la corrutina");
        if (isPlayable) moneda.SetActive(true);
        isCoinOnTable = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChallengesData
{
    public List<string> Challenges;
}

public class GameManager : MonoBehaviour
{
    private HashSet<GameObject> pressedObjects = new HashSet<GameObject>();
    private int counter = 0;
    private bool countdownStarted = false;
    private List<string> challenges;
    private List<GameObject> areaObjects; // Lista de objetos "Area"

    private void Start()
    {
        // Cargar los desafíos desde el archivo JSON
        LoadChallenges();

        // Obtener todos los objetos "Area" y agregarlos a la lista
        areaObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Area"));
    }

    private void LoadChallenges()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Challenges");
        ChallengesData challengesData = JsonUtility.FromJson<ChallengesData>(jsonFile.text);
        challenges = challengesData.Challenges;
    }

    public void ObjectPressed(GameObject pressedObject)
    {
        if (!pressedObjects.Contains(pressedObject))
        {
            pressedObjects.Add(pressedObject);

            // Aumentar el contador
            counter++;

            // Mostrar por consola que el objeto fue presionado y el contador actual
            Debug.Log(pressedObject.name + " presionado. Contador: " + counter);

            // Verificar si todos los objetos están presionados
            if (counter == GetTotalObjects())
            {
                StartCountdown();
            }
        }
    }

    public void ObjectReleased(GameObject releasedObject)
    {
        if (pressedObjects.Contains(releasedObject))
        {
            pressedObjects.Remove(releasedObject);

            // Decrementar el contador
            counter--;

            // Mostrar por consola que el objeto fue liberado y el contador actual
            Debug.Log(releasedObject.name + " liberado. Contador: " + counter);
        }
    }

    private int GetTotalObjects()
    {
        // Obtener el número total de objetos "Area" en la escena
        return areaObjects.Count;
    }

    private void StartCountdown()
    {
        if (!countdownStarted)
        {
            countdownStarted = true;
            Debug.Log("Comenzando contador de 3 segundos...");

            // Iniciar la corrutina para esperar 3 segundos antes de mostrar el desafío
            StartCoroutine(ShowChallengeAfterCountdown());
        }
    }

    private string lastChallenge;
    private GameObject lastAreaObject; // Último objeto "Area" seleccionado

    private IEnumerator ShowChallengeAfterCountdown()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // Seleccionar un objeto "Area" aleatorio diferente al último
        GameObject randomAreaObject = GetRandomAreaObject();

        // Verificar si es diferente al último objeto "Area"
        while (randomAreaObject == lastAreaObject)
        {
            randomAreaObject = GetRandomAreaObject();
        }

        lastAreaObject = randomAreaObject;

        // Mostrar un desafío aleatorio
        string randomChallenge = GetRandomChallenge();

        // Asignar el desafío al objeto "Area"
        AssignChallengeToArea(randomChallenge, randomAreaObject);
        countdownStarted = false;
    }

    private GameObject GetRandomAreaObject()
    {
        // Obtener un objeto "Area" aleatorio de la lista
        int randomIndex = Random.Range(0, areaObjects.Count);
        return areaObjects[randomIndex];
    }

    private string GetRandomChallenge()
    {
        // Obtener un desafío aleatorio de la lista
        int randomIndex = Random.Range(0, challenges.Count);
        return challenges[randomIndex];
    }

    private void AssignChallengeToArea(string challenge, GameObject areaObject)
    {
        // Aquí puedes implementar la lógica para asignar el desafío al objeto "Area".
        // Puedes tener un componente específico para manejar el desafío en cada objeto "Area" o
        // usar un diccionario para mapear el objeto "Area" con su desafío correspondiente.
        // Ejemplo: dictionaryAreaChallenge[areaObject] = challenge;
        Debug.Log($"Reto N° '{challenge}' para {areaObject.name}");
    }
}
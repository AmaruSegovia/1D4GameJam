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
        // Cargar los desaf�os desde el archivo JSON
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

            // Verificar si todos los objetos est�n presionados
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
        // Obtener el n�mero total de objetos "Area" en la escena
        return areaObjects.Count;
    }

    private void StartCountdown()
    {
        if (!countdownStarted)
        {
            countdownStarted = true;
            Debug.Log("Comenzando contador de 3 segundos...");

            // Iniciar la corrutina para esperar 3 segundos antes de mostrar el desaf�o
            StartCoroutine(ShowChallengeAfterCountdown());
        }
    }

    private string lastChallenge;
    private GameObject lastAreaObject; // �ltimo objeto "Area" seleccionado

    private IEnumerator ShowChallengeAfterCountdown()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // Seleccionar un objeto "Area" aleatorio diferente al �ltimo
        GameObject randomAreaObject = GetRandomAreaObject();

        // Verificar si es diferente al �ltimo objeto "Area"
        while (randomAreaObject == lastAreaObject)
        {
            randomAreaObject = GetRandomAreaObject();
        }

        lastAreaObject = randomAreaObject;

        // Mostrar un desaf�o aleatorio
        string randomChallenge = GetRandomChallenge();

        // Asignar el desaf�o al objeto "Area"
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
        // Obtener un desaf�o aleatorio de la lista
        int randomIndex = Random.Range(0, challenges.Count);
        return challenges[randomIndex];
    }

    private void AssignChallengeToArea(string challenge, GameObject areaObject)
    {
        // Aqu� puedes implementar la l�gica para asignar el desaf�o al objeto "Area".
        // Puedes tener un componente espec�fico para manejar el desaf�o en cada objeto "Area" o
        // usar un diccionario para mapear el objeto "Area" con su desaf�o correspondiente.
        // Ejemplo: dictionaryAreaChallenge[areaObject] = challenge;
        Debug.Log($"Reto N� '{challenge}' para {areaObject.name}");
    }
}
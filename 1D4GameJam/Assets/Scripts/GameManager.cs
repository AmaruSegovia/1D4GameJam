using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable]
public class ChallengesData
{
    public List<string> Challenges;
}

public class GameManager : MonoBehaviour
{
    public GameObject PanelReto;
    public Text infoText;//Referencia al objeto de texto en la escena
    private HashSet<GameObject> pressedObjects = new HashSet<GameObject>();
    private int counter = 0;
    private bool countdownStarted = false;
    private List<string> challenges;
    private List<GameObject> areaObjects; // Lista de objetos "Area"
    public string escenaAnterior;



    private void Start()
    {
        // Cargar los desaf�os desde el archivo JSON
        LoadChallenges();

        // Obtener todos los objetos "Area" y agregarlos a la lista
        areaObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Area"));

        //Inicializar el objeto de texto
        infoText.text = "Presiona los objetos 'Area'";


    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Menu")
        {
            SceneManager.LoadScene(escenaAnterior);
        }
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
        //Actualizar el texto en la escena
        infoText.text = pressedObject.name + "presionado, Contador: " + counter;
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
            StartCoroutine(ChooseNextActionAfterCountdown());
        }
    }

    private IEnumerator ChooseNextActionAfterCountdown()
    {
        // Esperando 3 seg. antes de tomar una accion
        yield return new WaitForSeconds(3f);

        // Elejir aleatoriamente entre 2 acciones
        int actionChoice = Random.Range(0, 2); // 0: desafio, 1: minijuego

        if (actionChoice == 0)
        {
            // Continuar con el desafío
            StartCoroutine(ShowChallengeAfterCountdown());
        }
        else
        {
            // Minijuegos
            StartCoroutine(SwitchRandomScene());
        }
    }

    private string lastChallenge;
    private GameObject lastAreaObject; // �ltimo objeto "Area" seleccionado

    private IEnumerator SwitchRandomScene()
    {
        // Seleccionar dos "jugadoresArea" aleatorios distintos
        GameObject firstAreaObject = GetRandomAreaObject();
        GameObject secondAreaObject = GetRandomAreaObject();

        // Asegurarse de que sean distintos
        while (firstAreaObject == secondAreaObject)
        {
            secondAreaObject = GetRandomAreaObject();
        }

        // Almacenar los objetos seleccionados
        lastAreaObject = firstAreaObject;

        // Mostrar los nombres de los objetos en la consola
        Debug.Log($"Jugador 1: Sr. {firstAreaObject.name}");
        Debug.Log($"Jugador 2: Sr. {secondAreaObject.name}");

        // Mostrar el texto de la UI con el nombre de la escena
        string sceneName = GetRandomSceneName();
        infoText.text = $"Minijuego: {sceneName}";

        // Esperando 3 seg. para cambiar de escena
        yield return new WaitForSeconds(3f);

        // Cambiando de escena 
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    // Elegir aleatoriamente entre las escenas
    private string GetRandomSceneName()
    {
        string[] scenes = { "JuegoHockey", "JuegoManos", "JuegoBarman", "JuegoRuletaRusa" };
        int randomIndex = Random.Range(0, scenes.Length);
        return scenes[randomIndex];
    }

    private IEnumerator ShowChallengeAfterCountdown()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        PanelReto.SetActive(true);
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
        // Mostrar un desaf�o aleatorio en la escena
        //infoText.text = $"Reto N� '{randomChallenge}' para {randomAreaObject.name}";
        // Mostrar un desaf�o aleatorio con animaci�n en la escena
        StartCoroutine(AnimateTextReveal($"Reto: '{randomChallenge}' para el Sr. {randomAreaObject.name}", infoText, 0.06f));
    }

    public void ShowChallenge(string jugador)
    {

        PanelReto.SetActive(true);
        
        // Mostrar un desaf�o aleatorio
        string randomChallenge = GetRandomChallenge();

        // Mostrar un desaf�o aleatorio con animaci�n en la escena
        StartCoroutine(AnimateTextReveal($"Reto: {randomChallenge} para {jugador}", infoText, 0.06f));
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

    IEnumerator AnimateTextReveal(string textReveal, Text targetText, float revealSpeed)
    {
        targetText.text = "";
        foreach (char character in textReveal)
        {
            targetText.text += character;
            yield return new WaitForSeconds(revealSpeed);
        }
    }
}

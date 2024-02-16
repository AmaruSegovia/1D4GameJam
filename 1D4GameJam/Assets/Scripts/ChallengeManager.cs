using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ChallengeManager : MonoBehaviour
{
    public TextMeshProUGUI challengeText;
    public TMP_InputField editChallengeInput;
    public Button editButtonPrefab;
    public Button deleteButtonPrefab;
    public GameObject container; // Nuevo contenedor principal

    private List<string> challenges;
    private int currentChallengeIndex = 0;
    private List<Button> editButtons = new List<Button>();
    private List<Button> deleteButtons = new List<Button>();

    private string challengesFilePath;

    void Start()
    {
        challengesFilePath = Path.Combine(Application.dataPath, "Resources/Challenges.json");
        LoadChallenges();
        DisplayCurrentChallenge();
        DisplayChallengesList();
    }

    void LoadChallenges()
    {
        if (File.Exists(challengesFilePath))
        {
            string json = File.ReadAllText(challengesFilePath);
            ChallengesData challengesData = JsonUtility.FromJson<ChallengesData>(json);
            challenges = new List<string>(challengesData.Challenges); // Convertir array a lista
        }
        else
        {
            Debug.LogError("Challenges file not found!");
        }
    }

    void DisplayCurrentChallenge()
    {
        challengeText.text = challenges[currentChallengeIndex];
    }

    void DisplayChallengesList()
    {
        // Limpiar los botones y textos existentes
        foreach (Button button in editButtons)
        {
            Destroy(button.gameObject);
        }
        editButtons.Clear();

        foreach (Button button in deleteButtons)
        {
            Destroy(button.gameObject);
        }
        deleteButtons.Clear();

        // Limpiar el contenedor principal
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }

        // Calcular la posición inicial
        Vector2 startPosition = Vector2.zero;

        // Espaciado entre cada elemento
        float spacing = 5f;

        // Desplazamiento horizontal de los botones
        float editButtonOffsetX = 185f; // Desplazamiento para el botón de editar
        float deleteButtonOffsetX = 130f; // Desplazamiento para el botón de eliminar

        // Instanciar los botones y textos uno por uno
        for (int i = 0; i < challenges.Count; i++)
        {
            int currentIndex = i; // Capturar el valor actual de 'i' para cada iteración

            // Instanciar el texto del reto
            TextMeshProUGUI challengeTextInstance = Instantiate(challengeText, container.transform);
            challengeTextInstance.text = challenges[i];
            RectTransform challengeTextTransform = challengeTextInstance.GetComponent<RectTransform>();
            challengeTextTransform.anchoredPosition = startPosition - new Vector2(0f, i * (challengeTextTransform.sizeDelta.y + spacing));
            challengeTextTransform.pivot = new Vector2(0.5f, 1f); // Centrar verticalmente respecto a la parte superior

            // Instanciar el botón de editar
            Button editButton = Instantiate(editButtonPrefab, container.transform);
            editButton.onClick.AddListener(() => OnEditButtonClicked(currentIndex)); // Pasar el índice capturado
            RectTransform editButtonTransform = editButton.GetComponent<RectTransform>();
            editButtonTransform.anchoredPosition = challengeTextTransform.anchoredPosition + new Vector2(editButtonOffsetX, 0f); // Desplazar hacia la derecha
            editButtons.Add(editButton);

            // Alinear verticalmente con el texto
            editButtonTransform.pivot = new Vector2(0, 1);
            editButtonTransform.anchoredPosition += new Vector2(0, -(challengeTextTransform.rect.height - editButtonTransform.rect.height) / 2f);

            // Instanciar el botón de eliminar
            Button deleteButton = Instantiate(deleteButtonPrefab, container.transform);
            deleteButton.onClick.AddListener(() => OnDeleteButtonClicked(currentIndex)); // Pasar el índice capturado
            RectTransform deleteButtonTransform = deleteButton.GetComponent<RectTransform>();
            deleteButtonTransform.anchoredPosition = challengeTextTransform.anchoredPosition + new Vector2(deleteButtonOffsetX, 0f); // Desplazar hacia la derecha
            deleteButtons.Add(deleteButton);

            // Alinear verticalmente con el texto
            deleteButtonTransform.pivot = new Vector2(0, 1);
            deleteButtonTransform.anchoredPosition += new Vector2(0, -(challengeTextTransform.rect.height - deleteButtonTransform.rect.height) / 2f);
            
            UpdateContentHeight();
        }
    }

    void UpdateContentHeight()
    {
        // Buscar el componente ScrollViewContent en el objeto Content y llamar a su método UpdateContentHeight
        ScrollViewContent scrollViewContent = container.GetComponent<ScrollViewContent>();
        if (scrollViewContent != null)
        {
            scrollViewContent.UpdateContentHeight();
        }
    }

    void OnEditButtonClicked(int index)
    {
        editChallengeInput.text = challenges[index];
        currentChallengeIndex = index;
    }

    void OnDeleteButtonClicked(int index)
    {
        challenges.RemoveAt(index);
        SaveChallenges();
        DisplayChallengesList();
        UpdateContentHeight();
    }

    public void SaveChallenges()
    {
        ChallengesData data = new ChallengesData();
        data.Challenges = challenges.ToArray();

        string json = JsonUtility.ToJson(data, true); // Agregar el segundo parámetro para formatear el JSON
        File.WriteAllText(challengesFilePath, json);
    }

    public void AddChallenge(string newChallenge)
    {
        challenges.Add(newChallenge);
        SaveChallenges();
        DisplayChallengesList();
    }

    public void EditChallenge(string editedChallenge)
    {
        challenges[currentChallengeIndex] = editedChallenge;
        SaveChallenges();
        DisplayCurrentChallenge();
        DisplayChallengesList();
    }

    [System.Serializable]
    public class ChallengesData
    {
        public string[] Challenges;
    }
}

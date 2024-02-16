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
    public Transform buttonParent;

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

        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // Calcular la posición inicial
        Vector2 startPosition = buttonParent.position;

        // Espaciado entre cada elemento
        float spacing = 10f;

        // Instanciar los botones y textos uno por uno
        for (int i = 0; i < challenges.Count; i++)
        {
            int currentIndex = i; // Capturar el valor actual de 'i' para cada iteración

            // Instanciar el texto del reto
            TextMeshProUGUI challengeTextInstance = Instantiate(challengeText, buttonParent);
            challengeTextInstance.text = challenges[i];
            RectTransform challengeTextTransform = challengeTextInstance.GetComponent<RectTransform>();
            challengeTextTransform.anchoredPosition = startPosition - new Vector2(0f, i * (challengeTextTransform.sizeDelta.y + spacing));
            challengeTextTransform.pivot = new Vector2(0.5f, 1f); // Centrar verticalmente respecto a la parte superior

            // Instanciar el botón de editar
            Button editButton = Instantiate(editButtonPrefab, buttonParent);
            editButton.onClick.AddListener(() => OnEditButtonClicked(currentIndex)); // Pasar el índice capturado
            RectTransform editButtonTransform = editButton.GetComponent<RectTransform>();
            editButtonTransform.anchoredPosition = new Vector2(challengeTextTransform.rect.width + spacing, challengeTextTransform.anchoredPosition.y - challengeTextTransform.rect.height / 2f); // A la derecha del texto
            editButtons.Add(editButton);

            // Instanciar el botón de eliminar
            Button deleteButton = Instantiate(deleteButtonPrefab, buttonParent);
            deleteButton.onClick.AddListener(() => OnDeleteButtonClicked(currentIndex)); // Pasar el índice capturado
            RectTransform deleteButtonTransform = deleteButton.GetComponent<RectTransform>();
            deleteButtonTransform.anchoredPosition = new Vector2(editButtonTransform.anchoredPosition.x + editButtonTransform.rect.width + spacing, challengeTextTransform.anchoredPosition.y - challengeTextTransform.rect.height / 2f); // A la derecha del botón de editar
            deleteButtons.Add(deleteButton);
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

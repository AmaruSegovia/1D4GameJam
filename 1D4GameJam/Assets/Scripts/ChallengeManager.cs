using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO; // Añadido para trabajar con archivos

public class ChallengeManager : MonoBehaviour
{
    public TextMeshProUGUI challengeText;
    public TMP_InputField newChallengeInput;
    public TextMeshProUGUI challengesListText;
    public TMP_InputField editChallengeInput;

    private int currentChallengeIndex = 0;
    private ChallengesData challengesData;

    [System.Serializable]
    public class ChallengesData
    {
        public string[] Challenges;
    }

    private string challengesFilePath = "Assets/Resources/Challenges.json"; // Ruta al archivo JSON

    void Start()
    {
        LoadChallenges();

        if (challengesData != null)
        {
            DisplayCurrentChallenge();
            DisplayChallengesList();
        }
    }

    void LoadChallenges()
    {
        string json = Resources.Load<TextAsset>("Challenges").text;
        challengesData = JsonUtility.FromJson<ChallengesData>(json);
        if (challengesData == null)
        {
            Debug.LogError("Error loading challenges. Check if the 'Challenges' JSON file exists and is correctly formatted.");
        }
    }

    void SaveChallenges()
    {
        // Convertir el objeto challengesData a formato JSON
        string json = JsonUtility.ToJson(challengesData, true);

        // Escribir el JSON en el archivo
        File.WriteAllText(challengesFilePath, json);
    }

    void DisplayCurrentChallenge()
    {
        if (challengesData != null && challengesData.Challenges != null && challengeText != null)
        {
            if (currentChallengeIndex >= 0 && currentChallengeIndex < challengesData.Challenges.Length)
            {
                challengeText.text = challengesData.Challenges[currentChallengeIndex];
            }
            else
            {
                Debug.LogError("Invalid currentChallengeIndex");
            }
        }

        if (challengesListText != null)
        {
            DisplayChallengesList();
        }
    }

    void DisplayChallengesList()
    {
        if (challengesData != null && challengesListText != null && challengesData.Challenges != null)
        {
            challengesListText.text = "";
            for (int i = 0; i < challengesData.Challenges.Length; i++)
            {
                challengesListText.text += $"{i + 1}. {challengesData.Challenges[i]}\n";
            }
        }
        else
        {
            Debug.LogError("Error displaying challenges list. Check if the variables are assigned and challengesData.Challenges is not null.");
        }
    }

    public void NextChallenge()
    {
        currentChallengeIndex = (currentChallengeIndex + 1) % challengesData.Challenges.Length;
        DisplayCurrentChallenge();
    }

    public void AddChallenge()
    {
        string newChallenge = newChallengeInput.text;
        if (!string.IsNullOrEmpty(newChallenge))
        {
            List<string> challengeList = new List<string>(challengesData.Challenges);
            challengeList.Add(newChallenge);
            challengesData.Challenges = challengeList.ToArray();

            newChallengeInput.text = "";
            DisplayChallengesList();
            SaveChallenges(); // Guardar los cambios en el archivo
        }
    }

    public void RemoveChallenge()
    {
        if (challengesData.Challenges.Length > 0)
        {
            List<string> challengeList = new List<string>(challengesData.Challenges);
            challengeList.RemoveAt(challengeList.Count - 1);
            challengesData.Challenges = challengeList.ToArray();

            DisplayChallengesList();
            DisplayCurrentChallenge();
            SaveChallenges(); // Guardar los cambios en el archivo
        }
    }

    public void EditChallenge()
    {
        string editedChallenge = editChallengeInput.text;
        if (!string.IsNullOrEmpty(editedChallenge) && currentChallengeIndex < challengesData.Challenges.Length)
        {
            challengesData.Challenges[currentChallengeIndex] = editedChallenge;
            DisplayChallengesList();
            DisplayCurrentChallenge();
            editChallengeInput.text = "";
            SaveChallenges(); // Guardar los cambios en el archivo
        }
    }
}
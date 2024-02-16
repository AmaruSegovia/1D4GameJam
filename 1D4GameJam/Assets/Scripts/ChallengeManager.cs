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

    void Start()
    {
        LoadChallenges();
        DisplayCurrentChallenge();
        DisplayChallengesList();
    }

    void LoadChallenges()
    {
        try
        {
            string json = Resources.Load<TextAsset>("Challenges").text;
            challengesData = JsonUtility.FromJson<ChallengesData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading challenges: " + e.Message);
        }
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
            Debug.LogWarning("Error displaying challenges list.");
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
            var challengeList = new List<string>(challengesData.Challenges);
            challengeList.Add(newChallenge);
            challengesData.Challenges = challengeList.ToArray();

            newChallengeInput.text = "";
            SaveChallenges();
            DisplayChallengesList();
        }
    }

    public void RemoveChallenge()
    {
        if (challengesData.Challenges.Length > 0)
        {
            var challengeList = new List<string>(challengesData.Challenges);
            challengeList.RemoveAt(challengeList.Count - 1);
            challengesData.Challenges = challengeList.ToArray();

            DisplayChallengesList();
            DisplayCurrentChallenge();
            SaveChallenges();
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
            SaveChallenges();
        }
    }

    void SaveChallenges()
    {
        string json = JsonUtility.ToJson(challengesData, true);
        var path = System.IO.Path.Combine(Application.dataPath, "Resources/Challenges.json");
        System.IO.File.WriteAllText(path, json);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void StartChallenges()
    {
        SceneManager.LoadScene("Retos");
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
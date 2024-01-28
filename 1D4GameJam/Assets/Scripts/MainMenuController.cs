using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
   public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
        Debug.Log("Juego comenzado");
    }
    public void StarChallenges()
    {
        SceneManager.LoadScene("Retos");
    }
}

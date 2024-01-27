using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject winnerPanel;
    [SerializeField] Text challenge;

    public void ActivateWinnerPanel(string challenge)
    {
        winnerPanel.SetActive(true);
    }
    public void RestartCurrentScene()
    {
        SceneManager.LoadScene("SampleScene");
        Debug.Log("Se recargo la escena");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Menu principal");
    }
}

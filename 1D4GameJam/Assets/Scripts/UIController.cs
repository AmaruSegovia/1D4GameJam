using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject winnerPanel;

    public void ActivateWinnerPanel()
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

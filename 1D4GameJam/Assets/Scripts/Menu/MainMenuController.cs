using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Tablero");
    }

    public void StartChallenges()
    {
        SceneManager.LoadScene("Retos");
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StarScenes()
    {
        SceneManager.LoadScene("Escenas");
    }

    public void StarPociones()
    {
        SceneManager.LoadScene("JuegoBarman");
    }

    public void StarHockey()
    {
        SceneManager.LoadScene("JuegoHockey");
    }
    public void StarLinterna()
    {
        SceneManager.LoadScene("JuegoRuletaRusa");
    }
    public void StarManos()
    {
        SceneManager.LoadScene("JuegoManos");
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
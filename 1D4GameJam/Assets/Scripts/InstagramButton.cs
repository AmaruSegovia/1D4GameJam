using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstagramButton : MonoBehaviour
{
    public string instagramPageURL = "https://www.instagram.com/purple_maru_/";

    // Método llamado al hacer clic en el botón
    public void OpenInstagramPage()
    {
        Application.OpenURL(instagramPageURL);
    }
}

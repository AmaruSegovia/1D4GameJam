using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstagramButton : MonoBehaviour
{
    public string instagramPageURL = "https://www.instagram.com/purple_maru_/";

    // M�todo llamado al hacer clic en el bot�n
    public void OpenInstagramPage()
    {
        Application.OpenURL(instagramPageURL);
    }
}

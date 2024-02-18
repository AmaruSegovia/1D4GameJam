using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panel; // Referencia al Canvas que quieres activar/desactivar

    public void ToggleCanvas()
    {
        panel.SetActive(!panel.activeSelf); // Activa o desactiva el Canvas dependiendo de su estado actual
    }
}

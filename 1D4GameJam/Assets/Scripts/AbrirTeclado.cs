using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbrirTeclado : MonoBehaviour
{
    public TMP_InputField inputField;  // El área de texto (InputField)
    private TouchScreenKeyboard keyboard;  // Referencia al teclado
    private bool isKeyboardOpen = false;

    void Start()
    {
        // Asegúrate de que el inputField esté asignado
        if (inputField == null)
        {
            Debug.LogError("El TMP_InputField no está asignado en el Inspector.");
            return;
        }

        // Conectar los eventos
        inputField.onSelect.AddListener(OnSelect);         // Cuando el inputField sea seleccionado
        inputField.onEndEdit.AddListener(OnEndEdit);      // Cuando termine la edición
        inputField.onValueChanged.AddListener(OnValueChanged); // Cuando el valor cambie
        inputField.onDeselect.AddListener(OnDeselect);   // Cuando el inputField pierda el foco
    }

    // Evento cuando el inputField es seleccionado
    private void OnSelect(string text)
    {
        if (!isKeyboardOpen)
        {
            Debug.Log("El teclado se abrió");
            OpenKeyboard();
            isKeyboardOpen = true;
        }
    }

    // Abre el teclado virtual en dispositivos móviles
    private void OpenKeyboard()
    {
        // Solo intentamos abrir el teclado en plataformas móviles
        if (Application.isMobilePlatform)
        {
            // Abre el teclado utilizando TouchScreenKeyboard en dispositivos móviles
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
        }
        else
        {
            Debug.LogWarning("El teclado solo se puede abrir en plataformas móviles.");
        }
    }

    // Evento cuando se termina la edición (cuando se presiona Enter o se toca fuera)
    private void OnEndEdit(string text)
    {
        Debug.Log("Edición terminada");
        CloseKeyboard();
    }

    // Evento cuando el valor del input cambia
    private void OnValueChanged(string text)
    {
        Debug.Log("El valor cambió: " + text);
    }

    // Evento cuando el inputField es deseleccionado
    private void OnDeselect(string text)
    {
        Debug.Log("El inputField fue deseleccionado");
        CloseKeyboard();
    }

    // Método para cerrar el teclado
    private void CloseKeyboard()
    {
        if (keyboard != null && keyboard.active)
        {
            keyboard.active = false;
            isKeyboardOpen = false;
            Debug.Log("El teclado se cerró");
        }
    }
}

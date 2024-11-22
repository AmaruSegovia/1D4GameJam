using UnityEngine;
using UnityEngine.UI;

public class TextObjectActivator : MonoBehaviour
{
    public Text inputText; // Campo para el objeto de texto UI
    public GameObject[] objectsToToggle; // Lista de objetos que se pueden activar/desactivar
    void Start()
    {
        if (inputText == null)
        {
            Debug.LogError("El campo inputText no está asignado.");
        }
        UpdateObjects(); // Actualizar al inicio en caso de que el texto ya tenga un valor
    }


   public void UpdateObjects()
    {
        string currentText = inputText.text.Trim().ToLower(); // Normaliza el texto para evitar errores
        bool found = false;

        foreach (GameObject obj in objectsToToggle)
        {
            if (obj.name.ToLower() == currentText)
            {
                obj.SetActive(true);
                found = true;
            }
            else
            {
                obj.SetActive(false);
            }
        }

        if (!found)
        {
            Debug.Log($"No se encontró ningún objeto con el nombre '{currentText}'.");
        }
    }
}

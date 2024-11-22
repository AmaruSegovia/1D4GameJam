using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public GameObject[] ingredientPrefabs;
    private List<GameObject> pooledObjects = new List<GameObject>();
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Inicializar el pool con los ingredientes a
    public void InitializePool(Vector3 startPosition, int rows, int columns, float gapX, float gapY)
    {
        pooledObjects.Clear();
        originalPositions.Clear();

        int ingredientIndex = 0; // Para iterar sobre los prefabs

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (ingredientIndex >= ingredientPrefabs.Length)
                    return; // Si ya no hay más prefabs, terminamos.

                // Calcular la posición de cada ingrediente
                Vector3 spawnPosition = new Vector3(
                    startPosition.x + column * gapX,
                    startPosition.y - row * gapY,
                    startPosition.z
                );

                // Instanciar el ingrediente
                GameObject ingredient = Instantiate(ingredientPrefabs[ingredientIndex], spawnPosition, Quaternion.identity);
                ingredient.SetActive(false);

                // Guardar posición original y añadir al pool
                originalPositions.Add(ingredient, spawnPosition);
                pooledObjects.Add(ingredient);

                ingredientIndex++; // Siguiente ingrediente
            }
        }
    }


    // Obtener un ingrediente del pool
    public GameObject GetPooledObject(string ingredientID)
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (obj.name.Contains(ingredientID) && !obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }

    // Resetear el pool al iniciar un nuevo juego
    public void ResetPool()
    {
        foreach (GameObject obj in pooledObjects)
        {
            // Mover el objeto a su posición original antes de activarlo
            if (originalPositions.ContainsKey(obj))
            {
                obj.transform.position = originalPositions[obj];
            }
            obj.SetActive(true);
        }
    }
}

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
    public void InitializePool(float startX, float gap)
    {
        pooledObjects.Clear();
        originalPositions.Clear();

        for (int i = 0; i < ingredientPrefabs.Length; i++)
        {
            Vector3 spawnPosition = new Vector3(startX + i * gap, -4f, 0);
            GameObject ingredient = Instantiate(ingredientPrefabs[i], spawnPosition, Quaternion.identity);
            ingredient.SetActive(false);

            // Almacenar la posición original
            originalPositions.Add(ingredient, spawnPosition);
            pooledObjects.Add(ingredient);
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

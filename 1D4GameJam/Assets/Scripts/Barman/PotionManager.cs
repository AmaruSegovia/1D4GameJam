using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour
{
    public static PotionManager Instance;
    public GameObject potionPrefab;
    public Slider timerSlider;
    public float gameDuration = 30f;

    // Lista de colores objetivos
    private List<Color> colorTargets = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta };
    private Color currentTargetColor;
    public float colorTolerance = 0.1f; // Margen de tolerancia para el color

    private List<string> ingredientOrder = new List<string>();
    private int currentIndex = 0;
    private float timeRemaining;
    private bool isGameOver = false;
    private bool isShakingPhase = false;

    public Transform posicionInicio;

    public float gap = -1.5f;

    private PotionColorShaker potionShaker;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Screen.autorotateToPortrait = true;

        Screen.autorotateToPortraitUpsideDown = true;

        Screen.autorotateToLandscapeLeft = false;

        Screen.autorotateToLandscapeRight = false;

        Screen.orientation = ScreenOrientation.AutoRotation;

        ObjectPool.Instance.InitializePool(posicionInicio.position.x, gap);

        // Instanciar la poción y obtener el script de agitación
        GameObject potion = Instantiate(potionPrefab, Vector3.zero, Quaternion.identity);
        potionShaker = potion.GetComponent<PotionColorShaker>();

        StartNewGame();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            UpdateTimer();
        }

        // Si estamos en la fase de agitar, comprobar el color
        if (isShakingPhase)
        {
            CheckPotionColor();
        }
    }

    private void StartNewGame()
    {
        Debug.Log("Iniciando un nuevo juego...");
        ObjectPool.Instance.ResetPool();

        currentIndex = 0;
        ingredientOrder.Clear();
        GenerateRandomOrder();
        ShowSequence();

        // Seleccionar un color objetivo aleatorio
        currentTargetColor = colorTargets[Random.Range(0, colorTargets.Count)];
        Debug.Log($"Color objetivo: {ColorUtility.ToHtmlStringRGB(currentTargetColor)}");

        timeRemaining = gameDuration;
        timerSlider.maxValue = gameDuration;
        timerSlider.value = gameDuration;
        isGameOver = false;
        isShakingPhase = false;
    }

    private void GenerateRandomOrder()
    {
        List<string> ingredientNames = new List<string>();
        foreach (var ingredient in ObjectPool.Instance.ingredientPrefabs)
        {
            ingredientNames.Add(ingredient.name);
        }

        while (ingredientNames.Count > 0)
        {
            int randomIndex = Random.Range(0, ingredientNames.Count);
            ingredientOrder.Add(ingredientNames[randomIndex]);
            ingredientNames.RemoveAt(randomIndex);
        }
    }

    private void ShowSequence()
    {
        Debug.Log("Secuencia a seguir:");
        foreach (var name in ingredientOrder)
        {
            Debug.Log(name);
        }
    }

    public bool CheckIngredient(string ingredientID)
    {
        if (isGameOver) return false;

        Debug.Log($"Verificando ingrediente: {ingredientID}");
        Debug.Log($"Ingrediente esperado: {ingredientOrder[currentIndex]}");

        if (ingredientID == ingredientOrder[currentIndex])
        {
            Debug.Log("¡Ingrediente correcto!");
            currentIndex++;

            if (currentIndex >= ingredientOrder.Count)
            {
                Debug.Log("Todos los ingredientes añadidos. Ahora agita la poción hasta el color correcto.");
                isShakingPhase = true;
            }

            return true;
        }
        else
        {
            LoseGame();
            return false;
        }
    }

    private void CheckPotionColor()
    {
        // Verificar si ya se ganó o si el juego ha terminado
        if (isGameOver) return;

        // Obtener el hue actual de la poción
        float currentHue = potionShaker.GetCurrentHue();
        float targetHue;
        Color.RGBToHSV(currentTargetColor, out targetHue, out _, out _);

        // Comprobar si el hue actual está dentro del margen de tolerancia
        if (Mathf.Abs(currentHue - targetHue) < colorTolerance)
        {
            Debug.Log("¡Color correcto! Has ganado.");
            WinGame();
        }
    }


    private void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        timerSlider.value = timeRemaining;

        if (timeRemaining <= 0)
        {
            LoseGame();
        }
    }

    private void WinGame()
    {
        if (isGameOver) return; // Asegurarnos de que no se llame varias veces
        isGameOver = true;

        Debug.Log("¡Has ganado!");

        // Reiniciar el juego después de un breve retraso
        Invoke("StartNewGame", 2f);
    }


    private void LoseGame()
    {
        Debug.Log("¡Perdiste!");
        isGameOver = true;
        Invoke("StartNewGame", 2f);
    }
}

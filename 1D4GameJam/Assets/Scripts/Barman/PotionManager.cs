using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class PotionManager : MonoBehaviour
{
    public static PotionManager Instance;
    public GameObject potionPrefab;
    public Slider timerSlider;
    private float gameDuration = 25f;
    [SerializeField] TextObjectActivator textActivator;
    // UI Elements
    [Header("UI Elements")]
    public Text ingredientListText;         // Texto para mostrar la lista de ingredientes
    public Image colorTargetImage;          // Imagen para mostrar el color objetivo
    public Text currentPlayerText;          // Texto para mostrar el jugador actual
    public GameObject losePanel;            // Panel que se muestra al perder

    // Players
    private List<string> players = new List<string> { "Carpincho", "Gato", "Pingüino", "Zorro" };
    private int currentPlayerIndex = 0;

    // Color Objective
    private List<Color> colorTargets = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta };
    private Color currentTargetColor;
    public float colorTolerance = 0.1f;

    // Ingredients
    private List<string> ingredientOrder = new List<string>();
    private int currentIndex = 0;

    private float timeRemaining;
    private bool isGameOver = false;
    private bool isShakingPhase = false;

    private PotionColorShaker potionShaker;

    public Transform posicionInicio;

    public Transform posicionInicioPocion;
    public float gap = -2f;

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
        int rows = 2;     // Número de filas
        int columns = 3;  // Número de columnas
        float gapX = 3.5f; // Espaciado horizontal entre ingredientes
        float gapY = 2.75f; // Espaciado vertical entre ingredientes

        ObjectPool.Instance.InitializePool(posicionInicio.position, rows, columns, gapX, gapY);

        GameObject potion = Instantiate(potionPrefab, posicionInicioPocion);

        potionShaker = potion.GetComponent<PotionColorShaker>();

        StartNewGame();
    }



    private void Update()
    {
        if (!isGameOver)
        {
            UpdateTimer();
        }

        if (isShakingPhase)
        {
            CheckPotionColor();
        }
    }



    private void StartNewGame()
    {
        Debug.Log("Iniciando un nuevo juego...");
        // Cambiar al siguiente jugador
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        string currentPlayerName = players[currentPlayerIndex];
        currentPlayerText.text = currentPlayerName;
        if (textActivator != null)
        {
            textActivator.inputText.text = currentPlayerName;
            textActivator.UpdateObjects(); // Activar/Desactivar imágenes
        }

        // Reiniciar los ingredientes
        ObjectPool.Instance.ResetPool();
        currentIndex = 0;
        ingredientOrder.Clear();


        // Generar ingredientes aleatorios y color objetivo
        GenerateRandomOrder();
        ShowIngredientList();
        ShowSequence();

        currentTargetColor = colorTargets[Random.Range(0, colorTargets.Count)];
        colorTargetImage.color = currentTargetColor;
        Debug.Log($"Color objetivo: {ColorUtility.ToHtmlStringRGB(currentTargetColor)}");

        timeRemaining = gameDuration;
        timerSlider.maxValue = gameDuration;
        timerSlider.value = gameDuration;
        isGameOver = false;
        isShakingPhase = false;

        losePanel.SetActive(false);
        isGameOver = false;
        isShakingPhase = false;
    }

    private void GenerateRandomOrder()
    {
        List<string> ingredientNames = new List<string>();

        // Agregar todos los nombres de los ingredientes del pool
        foreach (var ingredient in ObjectPool.Instance.ingredientPrefabs)
        {
            ingredientNames.Add(ingredient.name);
        }

        // Asegurarnos de que seleccionamos un máximo de 3 ingredientes
        int ingredientsToPick = Mathf.Min(3, ingredientNames.Count);

        ingredientOrder.Clear(); // Reinicia la lista para el nuevo juego

        for (int i = 0; i < ingredientsToPick; i++)
        {
            int randomIndex = Random.Range(0, ingredientNames.Count);
            ingredientOrder.Add(ingredientNames[randomIndex]);
            ingredientNames.RemoveAt(randomIndex); // Evitar duplicados
        }

        Debug.Log("Ingredientes generados aleatoriamente:");
        foreach (var ingredient in ingredientOrder)
        {
            Debug.Log(ingredient);
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
    private void ShowIngredientList()
    {
        ingredientListText.text = "";
        foreach (var name in ingredientOrder)
        {
            ingredientListText.text += $"{name}\n";
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
        if (isGameOver) return;

        float currentHue = potionShaker.GetCurrentHue();
        float targetHue;
        Color.RGBToHSV(currentTargetColor, out targetHue, out _, out _);

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
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("¡Has ganado!");
        gameDuration -= 0.75f;
        Debug.Log("Nuevo tiempo de duracion: " + gameDuration);
        Invoke("StartNewGame", 2f);
    }

    private void LoseGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("¡Perdiste!");
        losePanel.SetActive(true);
    }
}

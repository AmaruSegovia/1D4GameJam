using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PotionColorShaker : MonoBehaviour
{
    public SpriteRenderer potionRenderer;
    private Light2D luzSprite;
    [SerializeField] Light2D luzPocion;
    public float shakeThreshold = 3.0f;
    public float stopShakeThreshold = 2.5f;
    public float baseHueChangeSpeed = 0.1f; // Velocidad base de cambio de color
    private float currentHueChangeSpeed;    // Velocidad ajustada dinámicamente
    private Animator animator;
    private float currentHue = 0f;
    private float shakeCooldown = 0.4f;
    private float lastShakeTime;
    private bool isShaking = false;
    public float minAnimationSpeed = 0.5f; // Velocidad mínima de la animación
    public float maxAnimationSpeed = 4f;  // Velocidad máxima de la animación
    public float maxHueChangeSpeed = 1.0f; // Velocidad máxima de cambio de color

    private List<Color> predefinedColors = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta };
    void Start()
    {

        animator = GetComponent<Animator>();
        if (potionRenderer == null)
        {
            potionRenderer = GetComponent<SpriteRenderer>();
        }
        luzSprite = GetComponent<Light2D>();
    }

    void Update()
    {
        if (Time.time - lastShakeTime < shakeCooldown)
            return;

        Vector3 acceleration = Input.acceleration;
        float shakeIntensity = acceleration.magnitude;

        if (shakeIntensity > shakeThreshold)
        {
            lastShakeTime = Time.time;
            isShaking = true;
            animator.SetBool("estaAgitandose", true);

            // Normalizar la intensidad de la sacudida para usarla en velocidad de animación y color
            float normalizedIntensity = Mathf.InverseLerp(shakeThreshold, 10.0f, shakeIntensity);

            animator.speed = Mathf.Lerp(minAnimationSpeed, maxAnimationSpeed, normalizedIntensity);
            // Ajustar velocidad de cambio de color
            currentHueChangeSpeed = Mathf.Lerp(baseHueChangeSpeed, maxHueChangeSpeed, normalizedIntensity);
        }
        else
        {
            isShaking = false;
            animator.SetBool("estaAgitandose", false);

            // Restablecer velocidades a valores predeterminados
            animator.speed = 1.0f;
            currentHueChangeSpeed = baseHueChangeSpeed;
        }

        if (isShaking)
        {
            ChangePotionColor();
            luzSprite.color = Color.HSVToRGB(currentHue, 1f, 1f);
            luzPocion.color = Color.HSVToRGB(currentHue, 1f, 1f);
        }
    }

    private void ChangePotionColor()
    {
        currentHue += currentHueChangeSpeed * Time.deltaTime;
        if (currentHue > 1f) currentHue = 0f;
        potionRenderer.color = Color.HSVToRGB(currentHue, 1f, 1f);
    }

    public float GetCurrentHue()
    {
        return currentHue;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingrediente"))
        {
            ChangeRandomColorPotion();
        }
    }
    public void ChangeRandomColorPotion()
    {
        // Seleccionar un color aleatorio de la lista
        Color randomColor = predefinedColors[Random.Range(0, predefinedColors.Count)];

        // Aplicar el color a los diferentes componentes
        luzSprite.color = randomColor;
        potionRenderer.color = randomColor;
        luzPocion.color = randomColor;

        // Convertir el color RGB a HSV para obtener el valor de hue
        Color.RGBToHSV(randomColor, out currentHue, out _, out _);
        Debug.Log($"Nuevo color de la poción: {ColorUtility.ToHtmlStringRGB(randomColor)} con Hue: {currentHue}");
    }


}

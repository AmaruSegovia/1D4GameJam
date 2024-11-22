using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PotionColorShaker : MonoBehaviour
{
    public SpriteRenderer potionRenderer;
    private Light2D luzSprite;
    [SerializeField] Light2D luzPocion;
    public float shakeThreshold = 2.0f;
    public float hueChangeSpeed = 0.1f;

    private float currentHue = 0f;

    void Start()
    {
        if (potionRenderer == null)
        {
            potionRenderer = GetComponent<SpriteRenderer>();
        }
        luzSprite = GetComponent<Light2D>();
    }

    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        if (acceleration.magnitude > shakeThreshold)
        {
            ChangePotionColor();
            luzSprite.color = Color.HSVToRGB(currentHue,1f,1f);
            luzPocion.color = Color.HSVToRGB(currentHue, 1f, 1f);
        }
    }

    private void ChangePotionColor()
    {
        currentHue += hueChangeSpeed * Time.deltaTime;
        if (currentHue > 1f) currentHue = 0f;
        potionRenderer.color = Color.HSVToRGB(currentHue, 1f, 1f);
    }

    public float GetCurrentHue()
    {
        return currentHue;
    }
}

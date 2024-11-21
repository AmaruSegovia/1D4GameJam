using UnityEngine;

public class PotionColorShaker : MonoBehaviour
{
    public SpriteRenderer potionRenderer;
    public float shakeThreshold = 2.0f;
    public float hueChangeSpeed = 0.1f;

    private float currentHue = 0f;

    void Start()
    {
        if (potionRenderer == null)
        {
            potionRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        if (acceleration.magnitude > shakeThreshold)
        {
            ChangePotionColor();
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

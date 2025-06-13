using UnityEngine;

public class LightingController : MonoBehaviour
{
    public Light directionalLight;
    private GameManager gameManager;

    private float dayLength;
    private float segmentLength;

    private float[] intensities = { 0.8f, 1f, 0.9f, 0.5f };
    private Color[] colors;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dayLength = gameManager.dayTimer; // initial full day duration
        segmentLength = dayLength / 3f;   // 3 segments: morning, afternoon, evening

        colors = new Color[] {
            Hex("#FFFFFE"),  // Start (before transition begins)
            Hex("#FFF4D6"),  // Morning
            Hex("#EED695"),  // Afternoon
            Hex("#7B6F4F")   // Evening / End
        };

        if (directionalLight == null)
            Debug.LogError("Directional Light not assigned!");
    }

    void Update()
    {
        if (gameManager == null) return;

        float timeRemaining = gameManager.dayTimer;
        float timePassed = dayLength - timeRemaining;

        int currentSegment = Mathf.FloorToInt(timePassed / segmentLength);
        currentSegment = Mathf.Clamp(currentSegment, 0, 2); // only 3 transitions (0,1,2)

        int fromIndex = currentSegment;
        int toIndex = currentSegment + 1;

        float segmentStart = currentSegment * segmentLength;
        float segmentEnd = segmentStart + segmentLength;
        float segmentT = Mathf.InverseLerp(segmentStart, segmentEnd, timePassed);

        // Lerp between from → to
        directionalLight.intensity = Mathf.Lerp(intensities[fromIndex], intensities[toIndex], segmentT);
        directionalLight.color = Color.Lerp(colors[fromIndex], colors[toIndex], segmentT);
        RenderSettings.ambientLight = Color.Lerp(colors[fromIndex], colors[toIndex], segmentT);
    }

    private Color Hex(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#" + hex.TrimStart('#'), out color))
            return color;
        else
        {
            Debug.LogWarning("Invalid hex: " + hex);
            return Color.white;
        }
    }
}

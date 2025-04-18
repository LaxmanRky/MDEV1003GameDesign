using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    // UI References
    public Slider speedSlider;
    public Slider gravitySlider;
    public TextMeshProUGUI speedValueText;
    public TextMeshProUGUI gravityValueText;

    // Default values
    private const float DEFAULT_SPEED = 12f;  // Changed from 7f to middle of new range
    private const float DEFAULT_GRAVITY = 5f; // Changed from 2.5f to middle of new range

    // Value ranges
    private const float MIN_SPEED = 10f;     // Changed from 5f to 10f
    private const float MAX_SPEED = 15f;     // Changed from 12f to 15f
    private const float MIN_GRAVITY = 3f;    // Changed from 1f to 3f
    private const float MAX_GRAVITY = 7f;    // Changed from 5f to 7f

    void Start()
    {
        // Clear existing PlayerPrefs to ensure new ranges take effect
        PlayerPrefs.DeleteKey("ThrustPower");
        PlayerPrefs.DeleteKey("Gravity");
        
        LoadSettings();
        InitializeSliders();
    }

    void InitializeSliders()
    {
        // Setup speed slider
        if (speedSlider != null)
        {
            speedSlider.minValue = MIN_SPEED;
            speedSlider.maxValue = MAX_SPEED;
            speedSlider.value = PlayerPrefs.GetFloat("ThrustPower", DEFAULT_SPEED);
            speedSlider.onValueChanged.AddListener(OnSpeedChanged);
            UpdateSpeedText(speedSlider.value);
        }

        // Setup gravity slider
        if (gravitySlider != null)
        {
            gravitySlider.minValue = MIN_GRAVITY;
            gravitySlider.maxValue = MAX_GRAVITY;
            gravitySlider.value = PlayerPrefs.GetFloat("Gravity", DEFAULT_GRAVITY);
            gravitySlider.onValueChanged.AddListener(OnGravityChanged);
            UpdateGravityText(gravitySlider.value);
        }
    }

    void OnSpeedChanged(float value)
    {
        PlayerPrefs.SetFloat("ThrustPower", value);
        PlayerPrefs.Save();
        UpdateSpeedText(value);
    }

    void OnGravityChanged(float value)
    {
        PlayerPrefs.SetFloat("Gravity", value);
        PlayerPrefs.Save();
        UpdateGravityText(value);
    }

    void UpdateSpeedText(float value)
    {
        if (speedValueText != null)
        {
            speedValueText.text = $"Speed: {value:F1}";
        }
    }

    void UpdateGravityText(float value)
    {
        if (gravityValueText != null)
        {
            gravityValueText.text = $"Gravity: {value:F1}";
        }
    }

    void LoadSettings()
    {
        // Load saved values or use defaults
        float savedSpeed = PlayerPrefs.GetFloat("ThrustPower", DEFAULT_SPEED);
        float savedGravity = PlayerPrefs.GetFloat("Gravity", DEFAULT_GRAVITY);

        // Clamp values within valid ranges
        savedSpeed = Mathf.Clamp(savedSpeed, MIN_SPEED, MAX_SPEED);
        savedGravity = Mathf.Clamp(savedGravity, MIN_GRAVITY, MAX_GRAVITY);

        // Save clamped values
        PlayerPrefs.SetFloat("ThrustPower", savedSpeed);
        PlayerPrefs.SetFloat("Gravity", savedGravity);
        PlayerPrefs.Save();
    }

    public void ResetToDefaults()
    {
        if (speedSlider != null)
        {
            speedSlider.value = DEFAULT_SPEED;
            UpdateSpeedText(DEFAULT_SPEED);
        }
        
        if (gravitySlider != null)
        {
            gravitySlider.value = DEFAULT_GRAVITY;
            UpdateGravityText(DEFAULT_GRAVITY);
        }
        
        PlayerPrefs.SetFloat("ThrustPower", DEFAULT_SPEED);
        PlayerPrefs.SetFloat("Gravity", DEFAULT_GRAVITY);
        PlayerPrefs.Save();

        Debug.Log("Settings reset to defaults"); // Debug log to verify the method is called
    }
}

using UnityEngine;
using UnityEngine.UIElements;

public class TopMenu1 : MonoBehaviour
{
    private Label waveLabel;
    private Label creditsLabel;
    private Label healthLabel;
    private Button startWaveButton;

    private void OnEnable()
    {
        // Obtain the root element from UIDocument
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Query for the UI elements
        waveLabel = root.Q<Label>("golf");
        creditsLabel = root.Q<Label>("coin");
        healthLabel = root.Q<Label>("hp");
        startWaveButton = root.Q<Button>("startwave");

        // Add callback to button
        startWaveButton.clicked += OnStartWaveClicked;
    }

    private void OnDisable()
    {
        // Remove callback to prevent memory leaks
        startWaveButton.clicked -= OnStartWaveClicked;
    }

    private void OnStartWaveClicked()
    {
        // Start the wave logic here
    }

    public void SetWaveLabel(string text)
    {
        waveLabel.text = text;
    }

    public void SetCreditsLabel(string text)
    {
        creditsLabel.text = text;
    }

    public void SetHealthLabel(string text)
    {
        healthLabel.text = text;
    }
    // Add this method to TopMenu1.cs to update all labels at once
    public void UpdateTopMenuLabels(int credits, int health, int currentWave)
    {
        SetWaveLabel("Wave: " + currentWave);
        SetCreditsLabel("Credits: " + credits);
        SetHealthLabel("Health: " + health);
    }
    // Add this method to TopMenu1.cs if you need to enable a wave start button
    public void EnableWaveButton()
    {
        if (startWaveButton != null)
        {
            startWaveButton.SetEnabled(true);
        }
    }

}

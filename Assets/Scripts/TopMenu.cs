using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
    public Text waveLabel;
    public Text creditsLabel;
    public Text healthLabel;
    public Button startWaveButton;
    public void UpdateTopMenuLabels(int credits, int health, int currentWave)
    {
        Debug.Log("Updating top menu labels: Credits: " + credits + ", Health: " + health + ", Wave: " + currentWave);
        creditsLabel.text = "Credits: " + credits;
        healthLabel.text = "Health: " + health;
        waveLabel.text = "Wave: " + currentWave;
    }

    private void Start()
    {
        // Voeg een luisteraar toe aan de StartWaveButton
        startWaveButton.onClick.AddListener(OnStartWaveButtonClicked);
    }

    private void OnDestroy()
    {
        // Verwijder de luisteraar om geheugenlekken te voorkomen
        startWaveButton.onClick.RemoveListener(OnStartWaveButtonClicked);
    }

    // Functie om de wave-label bij te werken
    public void SetWaveLabel(string text)
    {
        waveLabel.text = text;
    }

    // Functie om de credits-label bij te werken
    public void SetCreditsLabel(string text)
    {
        creditsLabel.text = text;
    }

    // Functie om de health-label bij te werken
    public void SetHealthLabel(string text)
    {
        healthLabel.text = text;
    }

    // Functie die wordt aangeroepen wanneer de StartWaveButton wordt geklikt
    private void OnStartWaveButtonClicked()
    {
        // Roep de StartWave-methode aan van GameManager om de volgende wave te starten
        GameManger.Instance.StartWave(GameManger.Instance.GetCurrentWaveIndex() + 1); // Start de volgende wave
    }

    public void EnableWaveButton()
    {
        startWaveButton.interactable = true;
    }

}
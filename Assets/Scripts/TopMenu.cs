using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
    public Text waveLabel;
    public Text creditsLabel;
    public Text healthLabel;
    public Button startWaveButton;
    private GameManger gameManager;

    public void UpdateTopMenuLabels(int credits, int health, int wave)
    {
        Debug.Log("Updating top menu labels: Credits: " + credits + ", Health: " + health + ", Wave: " + wave);
        creditsLabel.text = "Credits: " + credits; // Update de creditslabel met de juiste waarde
        healthLabel.text = "Health: " + health;
        waveLabel.text = "Wave: " + wave;

        // Controleer of de gezondheid nul is en activeer 'game over' indien nodig
        if (health <= 0)
        {
            HighScoreManager.Instance.GameIsWon = true; // Verlies de game
            HighScoreManager.Instance.AddHighScore("Player", credits);
            SceneManager.LoadScene("HighScoreScene");
        }
    }

    private void Start()
    {
        gameManager = GameManger.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        // Verkrijg de HighScoreManager referentie van GameManager.Instance
        HighScoreManager highScoreManager = gameManager.GetComponent<HighScoreManager>();
        if (highScoreManager == null)
        {
            Debug.LogError("HighScoreManager component not found on GameManager!");
            return;
        }

        HighScoreManager.Instance = highScoreManager;

        // Voeg een luisteraar toe aan de StartWaveButton
        startWaveButton.onClick.AddListener(OnStartWaveButtonClicked);

        // Update de topmenu-labels
        UpdateTopMenuLabels(gameManager.GetCredits(), gameManager.GetHealth(), gameManager.GetCurrentWaveIndex());
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
        // Incrementeer de huidige golfindex voordat de volgende golf wordt gestart
        int currentWaveIndex = gameManager.GetCurrentWaveIndex();
        int nextWaveIndex = currentWaveIndex + 1;
        // Start de volgende golf
        gameManager.StartWave(nextWaveIndex);
    }

    public void EnableWaveButton()
    {
        startWaveButton.interactable = true;
    }
}

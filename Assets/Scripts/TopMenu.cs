using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class TopMenu : MonoBehaviour
{
    public static TopMenu Instance;
    private Label waveLabel;
    private Label creditsLabel;
    private Label healthLabel;
    private Button startWaveButton;
    private GameManger gameManager;
    public static event Action<TopMenu> OnTopMenuReady;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        waveLabel = root.Q<Label>("waveLabel"); // Zorg ervoor dat "waveLabel" overeenkomt met de ID in je UXML
        creditsLabel = root.Q<Label>("creditsLabel"); // Zorg ervoor dat "creditsLabel" overeenkomt
        healthLabel = root.Q<Label>("healthLabel"); // Zorg ervoor dat "healthLabel" overeenkomt
        startWaveButton = root.Q<Button>("startWaveButton"); // Zorg ervoor dat dit overeenkomt

        startWaveButton.clicked += OnStartWaveButtonClicked;
        UpdateTopMenuLabels(GameManger.Instance.GetCredits(), GameManger.Instance.GetHealth(), GameManger.Instance.GetCurrentWaveIndex());
    }


    private void OnDisable()
    {
        startWaveButton.clicked -= OnStartWaveButtonClicked; // Clean up the event listener
    }

    public void UpdateTopMenuLabels(int credits, int health, int wave)
    {
        Debug.Log($"Updating UI: Credits = {credits}, Health = {health}, Wave = {wave}");
        if (waveLabel != null) waveLabel.text = "Wave: " + wave;
        if (creditsLabel != null) creditsLabel.text = "Credits: " + credits;
        if (healthLabel != null) healthLabel.text = "Health: " + health;

        if (health <= 0)
        {
            HighScoreManager.Instance.GameIsWon = false;
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
        // Update de labels met de huidige waarden
        UpdateTopMenuLabels(gameManager.GetCredits(), gameManager.GetHealth(), gameManager.GetCurrentWaveIndex());
    }

    // Methode om te worden aangeroepen wanneer de GameManager gereed is
    private void HandleTopMenuReady(TopMenu topMenu)
    {
        // Update de labels met de huidige waarden
        UpdateTopMenuLabels(gameManager.GetCredits(), gameManager.GetHealth(), gameManager.GetCurrentWaveIndex());
    }

    private void OnStartWaveButtonClicked()
    {
        int currentWaveIndex = gameManager.GetCurrentWaveIndex();
        gameManager.StartWave(currentWaveIndex + 1);

        // Notify the GameManager that the wave button has been pressed
        gameManager.OnWaveButtonPressed();
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
    public void EnableWaveButton()
    {
        if (startWaveButton != null)
        {
            startWaveButton.SetEnabled(true);
        }
    }
    private void OnDestroy()
    {
        // Correctly detach the event handler to prevent memory leaks
        if (startWaveButton != null)
        {
            startWaveButton.clicked -= OnStartWaveButtonClicked;
        }
    }
}

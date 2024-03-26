using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance; // Singleton instance
    // Lists of tower prefabs
    public List<GameObject> Archers;
    public List<GameObject> Swords;
    public List<GameObject> Wizards;
    private Dictionary<TowerType, List<int>> towerPrefabCosts = new Dictionary<TowerType, List<int>>()
    {
        { TowerType.Archer, new List<int> { 50, 100, 150 } }, // Kosten voor Archer-torens op niveau 0, 1 en 2
        { TowerType.Sword, new List<int> { 75, 125, 175 } }, // Kosten voor Sword-torens op niveau 0, 1 en 2
        { TowerType.Wizard, new List<int> { 100, 150, 200 } } // Kosten voor Wizard-torens op niveau 0, 1 en 2
    };
    public WaveInfo[] waves = new WaveInfo[]
    {
        new WaveInfo(20, 1.0f),  // Golf 1: 5 vijanden met een sterkte van 1.0
        new WaveInfo(15, 1.2f),  // Golf 2: 7 vijanden met een sterkte van 1.2
        new WaveInfo(20, 1.5f),  // Golf 3: 10 vijanden met een sterkte van 1.5
        new WaveInfo(25, 2.0f)  // Golf 3: 10 vijanden met een sterkte van 1.5
    };
    private int enemiesRemaining;
    private int currentCredits = 225;
    private int currentHealth = 5;
    private int currentWave = 1;
    private TopMenu topMenu;
    public GameObject TopMenu;
    public GameObject TowerMenu;

    private bool isWaveButtonPressed = false;
    private TowerMenu towerMenu;

    private ConstructionSite selectedSite;
    private int enemyInGameCounter = 0;
    private bool waveActive = false;
    private HighScoreManager highScoreManager;

    public int CurrentWaveIndex { get { return currentWave; } }

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeGame();
        currentCredits = 225;
        currentHealth = 5;
        currentWave = 1;
        towerMenu = TowerMenu.GetComponent<TowerMenu>();

        // Use FindObjectOfType only if TopMenu is not assigned via the inspector.
        if (topMenu != null)
        {
            topMenu.GetComponent<TopMenu>().SetCreditsLabel("Credits: " + currentCredits);
            topMenu.GetComponent<TopMenu>().SetHealthLabel("Gate Health: " + currentHealth);
            topMenu.GetComponent<TopMenu>().SetWaveLabel("Wave: " + currentWave);
        }
        else
        {
            Debug.LogError("TopMenu is niet toegewezen in de Inspector!");
        }
        highScoreManager = FindObjectOfType<HighScoreManager>(); // Find the HighScoreManager in the scene.
        StartNextWave(); // Start the first wave of enemies.
    }

    public void AddInGameEnemy()
    {
        enemyInGameCounter++;
    }
    // Function to select a construction site
    public void SelectSite(ConstructionSite site)
    {
        // Remember the selected site
        selectedSite = site;

        // Pass the selected site to the TowerMenu
        towerMenu.SetSite(site);
    }
    public void RemoveInGameEnemy()
    {
        enemyInGameCounter--;

        if (!waveActive && enemyInGameCounter <= 0)
        {
            if (currentWave == waves.Length - 1 && enemiesRemaining <= 0)
            {
                // Set GameIsWon in the HighScoreManager to true
                highScoreManager.GameIsWon = true;
                // Pass the current credits to AddHighScore function
                highScoreManager.AddHighScore("Player", currentCredits);
                // Load the HighScoreScene
                SceneManager.LoadScene("HighScoreScene");
            }
            else
            {
                if (topMenu != null)
                {
                    topMenu.EnableWaveButton();
                }
                else
                {
                    Debug.LogWarning("TopMenu is not assigned to GameManger.");
                }
            }
        }
    }
    public void StartNextWave()
    {
        if (currentWave < waves.Length && enemiesRemaining <= 0)
        {
            WaveInfo nextWave = waves[currentWave]; // Get the wave info based on the current wave index
            EnemySpawner.Instance.SpawnWave(nextWave);
            currentWave++;
            if (topMenu != null)
            {
                topMenu.UpdateTopMenuLabels(currentCredits, currentHealth, currentWave);
            }
            else
            {
                Debug.LogWarning("TopMenu reference is not set in GameManger.");
            }

        }
        else if (currentWave >= waves.Length)
        {
            Debug.LogWarning("Alle golven zijn voltooid.");
        }
        else
        {
            Debug.Log("Kan de volgende golf niet starten omdat er nog vijanden zijn.");
        }
    }


    public void DecreaseEnemyCount()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            StartNextWave(); // Start de volgende golf als er geen vijanden meer zijn
        }
    }


    // Function to build a tower
    public void Build(TowerType type, SiteLevel level)
    {
        if (selectedSite == null)
        {
            return;
        }
        int towerCost = GetCost(type, level);
        if (currentCredits < towerCost)
        {
            Debug.LogWarning("Onvoldoende credits om de toren te bouwen.");
            return;
        }
                RemoveCredits(towerCost); // Update credits en UI

        // Selecteer de juiste lijst op basis van het torentype
        List<GameObject> towerList = null;
        switch (type)
        {
            case TowerType.Archer:
                towerList = Archers;
                break;
            case TowerType.Sword:
                towerList = Swords;
                break;
            case TowerType.Wizard:
                towerList = Wizards;
                break;
        }

        // Bouw de toren en update de UI
        GameObject towerPrefab = towerList[(int)level];
        Vector3 buildPosition = selectedSite.GetBuildPosition();
        GameObject towerInstance = Instantiate(towerPrefab, buildPosition, Quaternion.identity);

        selectedSite.SetTower(towerInstance, level, type);
        towerMenu.SetSite(null);
        UpdateUI();
    }

    //public void StartGame()
    //{
    //    credits = 225;
    //    health = 5;
    //    currentWave = 0;
    //    InitializeGame();
    //    StartCoroutine(UpdateUIAfterFrame());
    //}
    // In GameManger.cs


    public void InitializeGame()
    {
       
        StartCoroutine(DelayedUIUpdate());
    }

    private IEnumerator DelayedUIUpdate()
    {
        // Wait until the end of the frame to ensure all UI components are initialized
        yield return new WaitForEndOfFrame();

        // Now update the UI with the initial game state
        if (topMenu != null)
        {
            topMenu.UpdateTopMenuLabels(currentCredits, currentHealth, currentWave);
        }
        else
        {
            Debug.LogWarning("TopMenu reference is not set in GameManger.");
        }
    }
    public void UpdateUI()
    {
        if (topMenu != null)
        {
            topMenu.UpdateTopMenuLabels(currentCredits, currentHealth, currentWave);
        }
        else
        {
            Debug.LogWarning("TopMenu instance is not found.");
        }
    }




    public void OnWaveButtonPressed()
    {
        isWaveButtonPressed = true;
    }
    public void AttackGate(Path path)
    {
        // Assuming path is an enum or similar for different gates
        if (path == Path.Path1 || path == Path.Path2)
        {
            currentHealth--;
            if (topMenu != null)
            {
                topMenu.UpdateTopMenuLabels(currentCredits, currentHealth, currentWave);
            }
            else
            {
                Debug.LogWarning("TopMenu reference is not set in GameManger.");
            }

            if (currentHealth <= 0)
            {
                StartCoroutine(CheckEnemiesAndEndGame());
            }
        }
        else
        {
            Debug.LogWarning("Unknown path: " + path);
        }
    }
    private IEnumerator CheckEnemiesAndEndGame()
    {
        // Wait until the wave button has been pressed
        yield return new WaitUntil(() => isWaveButtonPressed);

        // Then wait until all enemies are destroyed
        yield return new WaitUntil(() => enemyInGameCounter <= 0);

        // Now go to the high score scene
        highScoreManager.GameIsWon = false; // Update this based on actual win/loss conditions
        highScoreManager.AddHighScore("Player", currentCredits);
        SceneManager.LoadScene("HighScoreScene");
    }


    public void AddCredits(int amount)
    {
        currentCredits += amount;
        UpdateUI(); // Roep deze methode aan om de UI bij te werken
    }

    public void RemoveCredits(int amount)
    {
        currentCredits -= amount;
        UpdateUI(); // Zorgt ervoor dat de UI direct wordt bijgewerkt
    }
    public int GetCredits()
    {
        // Return het huidige aantal credits
        return currentCredits;
    }
    public int GetHealth()
    {
        return currentHealth; // 'health' is de variabele die de huidige gezondheidswaarde bijhoudt
    }
    public int GetCost(TowerType type, SiteLevel level, bool selling = false)
    {
        // Bepaal de kosten voor het bouwen of verkopen van een toren op basis van het type, niveau en of het gaat om verkopen
        int cost = 0;

        // Als het gaat om verkopen, halveer de kosten
        if (selling)
        {
            cost = towerPrefabCosts[type][(int)level] / 2;
        }
        else
        {
            cost = towerPrefabCosts[type][(int)level];
        }

        return cost;
    }
    public void StartWave(int waveIndex)
    {
        enemyInGameCounter = 0; // Reset de counter bij het starten van een nieuwe golf

        if (waveIndex < waves.Length)
        {
            WaveInfo wave = waves[waveIndex];
            Debug.Log("Starting wave " + (waveIndex + 1) + " with " + wave.enemyCount + " enemies of strength " + wave.enemyStrength);
            EnemySpawner.Instance.SpawnWave(wave);
            currentWave = waveIndex;
            topMenu.SetWaveLabel("Wave: " + currentWave);
            topMenu.UpdateTopMenuLabels(GetCredits(), GetHealth(), GetCurrentWaveIndex());
        }
        else
        {
            Debug.LogWarning("Wave index out of range: " + waveIndex);
        }
    }

    public int GetCurrentWaveIndex()
    {
        return currentWave - 1; // Geef de huidige golfindex terug
    }
   
    public void AddCreditsOnWaveCompletion()
    {
        AddCredits(400); // Voeg 400 credits toe bij het voltooien van een golf
    }
    // Voeg een methode toe om credits toe te voegen wanneer een vijand wordt vernietigd
    public void AddCreditsOnEnemyDestroy()
    {
        AddCredits(10); // Voeg 10 credits toe wanneer een vijand wordt vernietigd
    }

    // In de EndGame-methode of op een geschikt punt waar het spel eindigt
    public void EndGame()
    {
        // Voeg de score toe aan de HighScoreManager
        highScoreManager.AddHighScore("Player", currentCredits); // Hier gaat "Player" de naam van de speler zijn
                                                          // Laad de HighScoreScene
        SceneManager.LoadScene("HighScoreScene");
    }


    public class WaveInfo
    {
        public int enemyCount;
        public float enemyStrength;

        public WaveInfo(int count, float strength)
        {
            enemyCount = count;
            enemyStrength = strength;
        }
    }
}
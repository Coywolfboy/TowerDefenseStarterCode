using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        new WaveInfo(10, 1.0f),  // Golf 1: 5 vijanden met een sterkte van 1.0
        new WaveInfo(15, 1.2f),  // Golf 2: 7 vijanden met een sterkte van 1.2
        new WaveInfo(20, 1.5f),  // Golf 3: 10 vijanden met een sterkte van 1.5
        new WaveInfo(25, 2.0f)  // Golf 3: 10 vijanden met een sterkte van 1.5
    };
    private int enemiesRemaining;
    private int credits;
    private int health;
    private int currentWave;
    public TopMenu topMenu;
    public GameObject TowerMenu;

    private TowerMenu towerMenu;

    private ConstructionSite selectedSite;
    private int enemyInGameCounter = 0;
    private bool waveActive = false;
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
        towerMenu = TowerMenu.GetComponent<TowerMenu>();
        StartGame(); // Start het spel, inclusief het initialiseren van golfinformatie, credits, enz.
        currentWave = 0; // Zorg ervoor dat currentWave correct is geïnitialiseerd voordat de golven worden gestart
        StartNextWave(); // Start de eerste golf van vijanden
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
                EndGame();
            }
            else
            {
                // Controleer of topMenu niet null is voordat je de methode aanroept
                if (topMenu != null)
                {
                    topMenu.EnableWaveButton(); // Roep de juiste methode aan op het topMenu-object
                }
                else
                {
                    Debug.LogWarning("TopMenu is niet toegewezen aan GameManger.");
                }
            }
        }
    }
    public void StartNextWave()
    {
        if (currentWave < waves.Length && enemiesRemaining <= 0)
        {
            WaveInfo nextWave = waves[currentWave];
            EnemySpawner.Instance.SpawnWave(nextWave);
            topMenu.UpdateTopMenuLabels(credits, health, currentWave + 1); // Update the labels with the correct wave index
            currentWave++; // Increment the wave index after each completed wave
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
        // Je kunt niet bouwen als er geen site is geselecteerd
        if (selectedSite == null)
        {
            return;
        }

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

        // Gebruik een switch met het niveau om een GameObject-toren te maken
        GameObject towerPrefab = towerList[(int)level];

        // Haal de positie van de ConstructionSite op
        Vector3 buildPosition = selectedSite.GetBuildPosition();

        GameObject towerInstance = Instantiate(towerPrefab, buildPosition, Quaternion.identity);

        // Configureer de geselecteerde site om de toren in te stellen
        selectedSite.SetTower(towerInstance, level, type); // Voeg level en type toe als
        towerMenu.SetSite(null);
    }
    public void StartGame()
    {
        // Stel de startwaarden in
        credits = 500;
        health = 10;
        currentWave = 0; // Initialize with 0 to start with the first wave
        topMenu.UpdateTopMenuLabels(credits, health, currentWave + 1); // Update the labels with the correct wave index
    }



    public void AttackGate()
    {
        // Verminder de gezondheid met 1
        health--;
        topMenu.SetHealthLabel("Health: " + health);
    }

    public void AddCredits(int amount)
    {
        // Voeg credits toe
        credits += amount;
        topMenu.SetCreditsLabel("Credits: " + credits);
    }

    public void RemoveCredits(int amount)
    {
        // Verminder credits
        credits -= amount;
        topMenu.SetCreditsLabel("Credits: " + credits);
    }

    public int GetCredits()
    {
        // Return het huidige aantal credits
        return credits;
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

    public void EndGame()
    {
        // Voeg hier eventuele extra logica toe die moet worden uitgevoerd voordat de game wordt gestopt

        // Stop de game
        Debug.Log("Game Over!");
        Application.Quit(); // Sluit de game af
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
using UnityEngine;
using UnityEngine.UIElements;

public class TowerMenu : MonoBehaviour
{
    private Button archerButton;
    private Button swordButton;
    private Button wizardButton;
    private Button updateButton;
    private Button destroyButton;

    private VisualElement root;

    private ConstructionSite selectedSite;

    public static TowerMenu Instance;

    void Awake()
    {
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
        root = GetComponent<UIDocument>().rootVisualElement;

        archerButton = root.Q<Button>("archer");
        swordButton = root.Q<Button>("sword");
        wizardButton = root.Q<Button>("wizard");
        updateButton = root.Q<Button>("button");
        destroyButton = root.Q<Button>("button");

        if (archerButton != null)
        {
            archerButton.clicked += OnArcherButtonClicked;
        }

        if (swordButton != null)
        {
            swordButton.clicked += OnSwordButtonClicked;
        }

        if (wizardButton != null)
        {
            wizardButton.clicked += OnWizardButtonClicked;
        }

        if (updateButton != null)
        {
            updateButton.clicked += OnUpdateButtonClicked;
        }

        if (destroyButton != null)
        {
            destroyButton.clicked += OnDestroyButtonClicked;
        }

        root.visible = false;
    }

    public void SetSite(ConstructionSite site)
    {
        selectedSite = site;
        EvaluateMenu();
    }

    public void EvaluateMenu()
    {
        if (selectedSite == null)
        {
            root.visible = false;
            return;
        }

        // Haal de beschikbare credits op van de GameManager
        int availableCredits = GameManager.Instance.GetCredits();

        // Gebruik de beschikbare credits om de menuknoppen in of uit te schakelen
        if (selectedSite.Level == SiteLevel.Level1)
        {
            // Voor Level1: Archers beschikbaar, anderen niet
            archerButton.SetEnabled(true);
            swordButton.SetEnabled(false);
            wizardButton.SetEnabled(false);
            updateButton.SetEnabled(false);
            destroyButton.SetEnabled(true);
        }
        else if (selectedSite.Level == SiteLevel.Level2)
        {
            // Voor Level2: Archers en Swords beschikbaar, Wizard en Update niet
            archerButton.SetEnabled(availableCredits >= GameManager.Instance.GetCost(TowerType.Archer, selectedSite.Level));
            swordButton.SetEnabled(availableCredits >= GameManager.Instance.GetCost(TowerType.Sword, selectedSite.Level));
            wizardButton.SetEnabled(false);
            updateButton.SetEnabled(false);
            destroyButton.SetEnabled(true);
        }
        else if (selectedSite.Level == SiteLevel.Level3)
        {
            // Voor Level3: Alle torens en Upgrade beschikbaar
            archerButton.SetEnabled(availableCredits >= GameManager.Instance.GetCost(TowerType.Archer, selectedSite.Level));
            swordButton.SetEnabled(availableCredits >= GameManager.Instance.GetCost(TowerType.Sword, selectedSite.Level));
            wizardButton.SetEnabled(availableCredits >= GameManager.Instance.GetCost(TowerType.Wizard, selectedSite.Level));
            updateButton.SetEnabled(availableCredits >= GameManager.Instance.GetCost(selectedSite.TowerType, selectedSite.Level + 1));
            destroyButton.SetEnabled(true);
        }
    }



    private void OnArcherButtonClicked()
    {
        GameManager.Instance.Build(TowerType.Archer, SiteLevel.Level1);
    }

    private void OnSwordButtonClicked()
    {
        GameManager.Instance.Build(TowerType.Sword, SiteLevel.Level1);
    }

    private void OnWizardButtonClicked()
    {
        GameManager.Instance.Build(TowerType.Wizard, SiteLevel.Level1);
    }

    private void OnUpdateButtonClicked()
    {
        // Check of selectedSite niet null is
        if (selectedSite != null)
        {
            // Haal het huidige niveau van de geselecteerde site op
            SiteLevel currentLevel = selectedSite.GetLevel();

            // Controleer of het huidige niveau minder is dan Level3
            if (currentLevel < SiteLevel.Level3)
            {
                // Verhoog het niveau met één
                SiteLevel newLevel = currentLevel + 1;

                // Stel het nieuwe niveau in voor de geselecteerde site
                selectedSite.SetLevel(newLevel);

                // Evalueer het menu opnieuw
                EvaluateMenu();
            }
        }
    }

    private void OnDestroyButtonClicked()
    {
        // Controleer of de geselecteerde site niet null is
        if (selectedSite != null)
        {
            // Stel het niveau van de geselecteerde site in op Level0
            selectedSite.SetLevel(SiteLevel.Level0);

            // Evalueer het menu opnieuw
            EvaluateMenu();
        }
    }


    private void OnDestroy()
    {
        // Controleer of de knopobjecten niet null zijn om NullReferenceException te voorkomen
        if (archerButton != null)
        {
            archerButton.clicked -= OnArcherButtonClicked;
        }

        if (swordButton != null)
        {
            swordButton.clicked -= OnSwordButtonClicked;
        }

        if (wizardButton != null)
        {
            wizardButton.clicked -= OnWizardButtonClicked;
        }

        if (updateButton != null)
        {
            updateButton.clicked -= OnUpdateButtonClicked;
        }

        if (destroyButton != null)
        {
            destroyButton.clicked -= OnDestroyButtonClicked;
        }
    }

    
}

using UnityEngine;
using UnityEngine.UIElements;

public class TowerMenu : MonoBehaviour
{
    private Button archer;
    private Button sword;
    private Button wizard;
    private Button Upgrade;
    private Button destroy;

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

        archer = root.Q<Button>("archer");
        sword = root.Q<Button>("sword");
        wizard = root.Q<Button>("wizard");
        Upgrade = root.Q<Button>("Upgrade");
        destroy = root.Q<Button>("destroy");

        if (archer != null)
        {
            archer.clicked += OnArcherButtonClicked;
        }

        if (sword != null)
        {
            sword.clicked += OnSwordButtonClicked;
        }

        if (wizard != null)
        {
            wizard.clicked += OnWizardButtonClicked;
        }

        if (Upgrade != null)
        {
            Upgrade.clicked += OnUpdateButtonClicked;
        }

        if (destroy != null)
        {
            destroy.clicked += OnDestroyButtonClicked;
        }

        root.visible = false;
    }
    private void CheckHideMenu(Vector3 clickPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("buildingPlaceGrass"))
        {
            // Toon het menu als er op buildingPlaceGrass is geklikt
            root.visible = true;
        }
        else
        {
            // Verberg het menu als er op een andere plaats is geklikt
            root.visible = false;
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Bepaal de positie van de muisklik in de wereldruimte
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Controleer of het menu moet worden verborgen op basis van de klikpositie
            CheckHideMenu(clickPosition);
        }
    }
    public void SetSite(ConstructionSite site)
    {
        selectedSite = site;
        if (selectedSite != null)
        {
            EvaluateMenu();
            root.visible = true; // Toon het TowerMenu als een site is geselecteerd
        }
        else
        {
            root.visible = false; // Verberg het TowerMenu als er geen site is geselecteerd
        }
    }




    public void EvaluateMenu()
    {
        if (selectedSite == null)
        {
            root.visible = false;
            return;
        }

        // Haal de beschikbare credits op van de GameManager
        int availableCredits = GameManger.Instance.GetCredits();

        // Gebruik de beschikbare credits om de menuknoppen in of uit te schakelen
        if (selectedSite.Level == SiteLevel.Level0)
        {
            // Voor Level0: Toon alleen de knoppen voor het bouwen van torens
            archer.SetEnabled(availableCredits >= GameManger.Instance.GetCost(TowerType.Archer, selectedSite.Level));
            sword.SetEnabled(availableCredits >= GameManger.Instance.GetCost(TowerType.Sword, selectedSite.Level));
            wizard.SetEnabled(availableCredits >= GameManger.Instance.GetCost(TowerType.Wizard, selectedSite.Level));
            Upgrade.SetEnabled(false); // De upgrade-knop is niet beschikbaar op niveau 0
            destroy.SetEnabled(false); // De vernietigingsknop is niet beschikbaar op niveau 0
        }
        else if (selectedSite.Level < SiteLevel.Level3)
        {
            // Voor Level1 en Level2: Toon de upgrade-knop en alle torenbouwknoppen
            archer.SetEnabled(availableCredits >= GameManger.Instance.GetCost(TowerType.Archer, selectedSite.Level));
            sword.SetEnabled(availableCredits >= GameManger.Instance.GetCost(TowerType.Sword, selectedSite.Level));
            wizard.SetEnabled(availableCredits >= GameManger.Instance.GetCost(TowerType.Wizard, selectedSite.Level));
            Upgrade.SetEnabled(availableCredits >= GameManger.Instance.GetCost(selectedSite.TowerType, selectedSite.Level + 1));
            destroy.SetEnabled(true); // De vernietigingsknop is altijd beschikbaar voor upgradebaar niveau
        }
        else if (selectedSite.Level == SiteLevel.Level3)
        {
            // Voor Level3: Toon alleen de vernietigingsknop, upgrade-knop is niet beschikbaar
            archer.SetEnabled(false); // De torenbouwknoppen zijn niet beschikbaar op niveau 3
            sword.SetEnabled(false);
            wizard.SetEnabled(false);
            Upgrade.SetEnabled(false); // De upgrade-knop is niet beschikbaar op niveau 3
            destroy.SetEnabled(true); // De vernietigingsknop is altijd beschikbaar voor niveau 3
        }

        // Toon of verberg het menu op basis van de geselecteerde site
        root.visible = selectedSite != null;
    }
    private void OnArcherButtonClicked()
    {
        GameManger.Instance.Build(TowerType.Archer, SiteLevel.Level0);
    }

    private void OnSwordButtonClicked()
    {
        GameManger.Instance.Build(TowerType.Sword, SiteLevel.Level0);
    }

    private void OnWizardButtonClicked()
    {
        GameManger.Instance.Build(TowerType.Wizard, SiteLevel.Level0);
    }

    private void OnUpdateButtonClicked()
    {
        // Controleer of de geselecteerde site niet null is
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

                // Update de UI direct zonder de EvaluateMenu-methode te gebruiken
                UpdateUI();
            }
        }
    }

    private void UpdateUI()
    {
        // Update de UI-elementen rechtstreeks zonder rekening te houden met de beschikbare credits
        // Dit kan bijvoorbeeld de status van de upgrade-knop direct aanpassen
        Upgrade.SetEnabled(selectedSite != null && selectedSite.GetLevel() < SiteLevel.Level3);
    }


    private void OnDestroyButtonClicked()
    {
        // Controleer of de geselecteerde site niet null is
        if (selectedSite != null)
        {
            // Haal de toren op die is gebouwd op de geselecteerde site
            GameObject tower = selectedSite.GetTower();

            // Controleer of de toren niet null is voordat je deze vernietigt
            if (tower != null)
            {
                // Verwijder de toren uit de scene
                Destroy(tower);
            }

            // Stel het niveau van de geselecteerde site in op Level0
            selectedSite.SetLevel(SiteLevel.Level0);

            // Evalueer het menu opnieuw
            EvaluateMenu();

            // Verberg het TowerMenu
            SetSite(null);
        }
    }


    private void OnDestroy()
    {
        // Controleer of de knopobjecten niet null zijn om NullReferenceException te voorkomen
        if (archer != null)
        {
            archer.clicked -= OnArcherButtonClicked;
        }

        if (sword != null)
        {
            sword.clicked -= OnSwordButtonClicked;
        }

        if (wizard != null)
        {
            wizard.clicked -= OnWizardButtonClicked;
        }

        if (Upgrade != null)
        {
            Upgrade.clicked -= OnUpdateButtonClicked;
        }

        if (destroy != null)
        {
            destroy.clicked -= OnDestroyButtonClicked;
        }
    }

    
}

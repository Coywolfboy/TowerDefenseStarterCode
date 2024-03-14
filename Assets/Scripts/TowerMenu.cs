using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        archerButton = root.Q<Button>("archer-button");
        swordButton = root.Q<Button>("sword-button");
        wizardButton = root.Q<Button>("wizard-button");
        updateButton = root.Q<Button>("button-upgrade");
        destroyButton = root.Q<Button>("button-destroy");

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

        // Implementeer de logica om de menuknoppen in- en uit te schakelen op basis van de geselecteerde site.
        // Je kunt toegang krijgen tot de geselecteerde site via de variabele 'selectedSite'.

        // Voorbeeld:
        // if (selectedSite.SiteLevel == SiteLevel.Zero)
        // {
        //     archerButton.SetEnabled(true);
        //     wizardButton.SetEnabled(true);
        //     swordButton.SetEnabled(true);
        //     updateButton.SetEnabled(false);
        //     destroyButton.SetEnabled(false);
        // }

        // Voer deze logica uit voor elk niveau van de constructieplaats en de bijbehorende knoppen.
    }

    private void OnArcherButtonClicked()
    {
        Debug.Log("Archer Tower button clicked");
    }

    private void OnSwordButtonClicked()
    {
        Debug.Log("Sword Tower button clicked");
    }

    private void OnWizardButtonClicked()
    {
        Debug.Log("Wizard Tower button clicked");
    }

    private void OnUpdateButtonClicked()
    {
        Debug.Log("Upgrade button clicked");
    }

    private void OnDestroyButtonClicked()
    {
        Debug.Log("Destroy button clicked");
    }

    private void OnDestroy()
    {
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
            destroyButton.clicked -= OnArcherButtonClicked;
        }
    }
}

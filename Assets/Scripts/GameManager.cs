using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject TowerMenu;
    private TowerMenu towerMenu;

    public List<GameObject> Archers = new List<GameObject>();
    public List<GameObject> Swords = new List<GameObject>();
    public List<GameObject> Wizards = new List<GameObject>();

    private ConstructionSite selectedSite;

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
        towerMenu = TowerMenu.GetComponent<TowerMenu>();
    }

    public void SelectSite(ConstructionSite site)
    {
        selectedSite = site;
        towerMenu.SetSite(selectedSite);
    }
}

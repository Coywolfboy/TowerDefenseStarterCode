using UnityEngine;

public class ConstructionSite
{
    public Vector3Int TilePosition { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public SiteLevel Level { get; private set; }
    public TowerType TowerType { get; private set; }
    private GameObject tower;

    public ConstructionSite(Vector3Int tilePosition, Vector3 worldPosition)
    {
        TilePosition = tilePosition;
        WorldPosition = worldPosition + new Vector3(0, 0.5f, 0); // Pas de hoogte aan
        Level = SiteLevel.Level0;
        tower = null;
    }

    public void SetTower(GameObject newTower, SiteLevel newLevel, TowerType newType)
    {
        // Controleer of er al een toren op deze bouwplaats staat
        if (tower != null)
        {
            // Vernietig de bestaande toren voordat je een nieuwe bouwt
            Object.Destroy(tower);
        }

        // Wijs de nieuwe toren toe
        tower = newTower;
        Level = newLevel;
        TowerType = newType;
    }

    public Vector3 GetBuildPosition()
    {
        return WorldPosition; // Gebruik de wereldpositie van de bouwplaats
    }

    // Methode om de toren op te halen
    public GameObject GetTower()
    {
        return tower;
    }

    // Vermoedelijk heb je een methode nodig om het niveau van de bouwplaats in te stellen
    public void SetLevel(SiteLevel newLevel)
    {
        Level = newLevel;
    }

    // Vermoedelijk heb je ook een methode nodig om het niveau van de bouwplaats op te halen
    public SiteLevel GetLevel()
    {
        return Level;
    }
}
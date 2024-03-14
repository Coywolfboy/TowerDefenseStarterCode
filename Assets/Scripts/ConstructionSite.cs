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
        Level = SiteLevel.Unbuilt;
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
}
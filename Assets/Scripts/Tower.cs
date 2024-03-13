using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.PlasticSCM.Editor.WebApi.CredentialsResponse;


public class Tower : MonoBehaviour
{
    public float attackRange = 1f; // Range within which the tower can detect and attack enemies 
    public float attackRate = 1f; // How often the tower attacks (attacks per second) 
    public int attackDamage = 1; // How much damage each attack does 
    public float attackSize = 1f; // How big the bullet looks 
    public float projectileSpeed = 10f; // Speed of the projectile
    public GameObject bulletPrefab; // The bullet prefab the tower will shoot 
    public TowerType type; // the type of this tower 

    private float nextAttackTime = 0f; // Timer to track the next attack time

    // Draw the attack range in the editor for easier debugging 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if it's time to attack
        if (Time.time >= nextAttackTime)
        {
            // Scan for enemies and shoot
            ScanForEnemiesAndShoot();
            // Update the next attack time
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    // Method to scan for enemies within range and shoot at one of them
    void ScanForEnemiesAndShoot()
    {
        // Find all colliders within the attack range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        // Iterate through each collider found
        foreach (Collider2D col in hitColliders)
        {
            // Check if the collider belongs to an enemy
            if (col.CompareTag("Enemy"))
            {
                // Shoot at the enemy
                ShootAtEnemy(col.gameObject);
                // Exit the loop after shooting at one enemy
                break;
            }
        }
    }

    // Method to shoot at a specific enemy
    void ShootAtEnemy(GameObject enemy)
    {
        // Instantiate the bullet prefab at the tower's position
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Set the scale of the bullet to the specified attack size
        bullet.transform.localScale = new Vector3(attackSize, attackSize, 1f);

        // Get the Projectile component from the bullet prefab
        Projectile projectile = bullet.GetComponent<Projectile>();

        // Check if the Projectile component exists
        if (projectile != null)
        {
            // Set the damage of the projectile to the tower's attack damage
            projectile.damage = attackDamage;
            // Set the target of the projectile to the enemy
            projectile.target = enemy.transform;
            // Set the speed of the projectile
            projectile.speed = projectileSpeed;
        }
        else
        {
            // Log an error if the Projectile component is missing
            Debug.LogError("Projectile component not found on bulletPrefab.");
        }
    }
}
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
    public float projectileSpeed = 5f; // Speed of the projectile

    public GameObject bulletPrefab; // The bullet prefab the tower will shoot
    public TowerType type; // the type of this tower

    // Draw the attack range in the editor for easier debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Start()
    {
        InvokeRepeating("Attack", 0f, 1f / attackRate); // Start attacking at the specified rate
    }

    void Attack()
    {
        // Find all enemies within the attack range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                ShootAtEnemy(collider.transform);
                return; // Only shoot at one enemy per attack cycle
            }
        }
    }

    void ShootAtEnemy(Transform target)
    {
        // Instantiate the bullet prefab
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Set the properties of the projectile
        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.target = target;
            projectile.speed = projectileSpeed;
            projectile.damage = attackDamage;
        }

        // Set the size of the projectile
        bullet.transform.localScale = new Vector3(attackSize, attackSize, 1f);
    }
}
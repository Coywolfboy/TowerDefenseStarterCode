using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float speed;
    public int damage;

    void Update()
    {
        // Als het doelwit niet meer bestaat, vernietig het projectiel
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Beweeg het projectiel naar het doelwit
        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        // Controleer of het projectiel het doelwit heeft geraakt
        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            // Breng schade toe aan het doelwit en vernietig het projectiel
            DamageTarget();
            Destroy(gameObject);
        }
    }

    // Methode om schade toe te brengen aan het doelwit
    void DamageTarget()
    {
        if (target != null && target.CompareTag("Enemy"))
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
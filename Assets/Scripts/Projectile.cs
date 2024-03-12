using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float speed;
    public int damage;

    void Start()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public float health = 10f;
    public int points = 1;
    public Path path { get; set; }
    public GameObject target { get; set; }
    private int pathIndex = 1;

    // Functie om schade toe te brengen aan de vijand
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GameManger.Instance.AddCreditsOnEnemyDestroy(); // Voeg credits toe wanneer een vijand wordt vernietigd
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);

        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target = EnemySpawner.Instance.RequestTarget(path, pathIndex);
            pathIndex++;

            if (target == null)
            {
                Destroy(gameObject);

                // Check welk pad de vijand volgt en verminder de gezondheid dienovereenkomstig
                if (path == Path.Path1)
                {
                    GameManger.Instance.AttackGate(Path.Path1);
                }
                else if (path == Path.Path2)
                {
                    GameManger.Instance.AttackGate(Path.Path2);
                }
            }
        }
    }
}

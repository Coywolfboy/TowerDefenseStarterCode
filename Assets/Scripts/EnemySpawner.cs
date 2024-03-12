using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    public List<GameObject> Path1;
    public List<GameObject> Path2;
    public List<GameObject> Enemies;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnTester", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnEnemy(int type, Path path)
    {
        var newEnemy = Instantiate(Enemies[type], Path1[0].transform.position, Path1[0].transform.rotation);
        var script = newEnemy.GetComponent<Enemy>();
        script.path = path;
        script.target = Path1[1];

    }

    private void SpawnTester()
    {
        SpawnEnemy(0, Path.Path1);
    }

    public GameObject RequestTarget(Path path, int index)
    {
        List<GameObject> selectedPath = path == Path.Path1 ? Path1 : Path2;

        if (index < selectedPath.Count)
            return selectedPath[index];
        else
            return null;
    }
}

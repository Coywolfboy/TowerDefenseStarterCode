using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

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

    public void SpawnWave(WaveInfo wave)
    {
        StartCoroutine(SpawnEnemies(wave));
    }

    private IEnumerator SpawnEnemies(WaveInfo wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            // Spawn vijanden van niveau 0 (of het gewenste niveau) in de eerste wave
            int enemyType = 0; // Hiermee spawnen we alleen vijanden van niveau 0 in de eerste wave

            // Willekeurig kiezen tussen Path1 en Path2
            Path path = Random.Range(0, 2) == 0 ? Path.Path1 : Path.Path2;

            SpawnEnemy(enemyType, path);
            yield return new WaitForSeconds(1f); // Wacht 1 seconde voordat een nieuwe vijand wordt gespawnd
        }
    }

    private void SpawnEnemy(int type, Path path)
    {
        List<GameObject> selectedPath = path == Path.Path1 ? Path1 : Path2;
        if (selectedPath.Count > 0)
        {
            var spawnPosition = selectedPath[0].transform.position;
            var newEnemy = Instantiate(Enemies[type], spawnPosition, Quaternion.identity);
            var script = newEnemy.GetComponent<Enemy>();
            script.path = path;
            script.target = selectedPath[1];
        }
        else
        {
            Debug.LogWarning("No path available for enemy spawn!");
        }
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

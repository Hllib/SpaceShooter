using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _enemyContainer;
    private readonly int _spawnRate = 3;
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (!_stopSpawning)
        {
            var newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-10f, 9f), Random.Range(4.5f, 6f), 0) ,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    IEnumerator PowerupSpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (!_stopSpawning)
        {
            var randomPowerupIndex = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[randomPowerupIndex], new Vector3(Random.Range(-10f, 9f), Random.Range(4.5f, 6f), 0), Quaternion.identity);
            var randomDelay = Random.Range(4.0f, 9.0f);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

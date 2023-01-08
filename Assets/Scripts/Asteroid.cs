using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 10.5f;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private SpawnManager _spawnManager;

    private Player _player;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); //TODO: make serialized

        if (_spawnManager == null)
        {
            Debug.Log("Spawn manager is NULL in Asteroid script");
        }

        _player = GameObject.Find("Player").GetComponent<Player>(); //TODO: make serialized

        if (_player == null)
        {
            Debug.Log("Player is NULL in Asteroid script");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }
        if (collision.CompareTag("Player"))
        {
            _player.DamageOnPlayer();
        }
    }
}

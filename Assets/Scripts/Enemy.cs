using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate = 3.0f;
    private float _canFireCooldown = 0.0f;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.Log("Player is NULL in Enemy");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFireCooldown)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFireCooldown = _fireRate + Time.time;

            var vector = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);

            GameObject enemyLaser = Instantiate(_laserPrefab, vector, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignLaserToEnemy();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        float randomX = Random.Range(-8f, 8f);
        transform.position = transform.position.y < -5.0f ? new Vector3(randomX, 7, 0)
            : transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.DamageOnPlayer();
            }

            ExplodeEnemy();
        }
    }

    public void ExplodeEnemy()
    {
        Destroy(GetComponent<Collider2D>());
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 0.25f);
    }
}

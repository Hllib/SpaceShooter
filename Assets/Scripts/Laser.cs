using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private Player _player;
    private Enemy _enemy;
    private bool _isEnemyLaser;

    [SerializeField]
    private AudioClip _laserFireSound;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource != null)
        {
            _audioSource.clip = _laserFireSound;
            _audioSource.Play();
        }
        else
        {
            Debug.Log("Audio Source is NULL in Laser");
        }
    }

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            Move(Vector3.up);
        }
        else
        {
            Move(Vector3.down);
        }
    }

    void Move(Vector3 direction)
    {
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y > 8f || transform.position.y < -9f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignLaserToEnemy()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _isEnemyLaser)
        {
            _player = collision.GetComponent<Player>();

            if (_player != null)
            {
                _player.DamageOnPlayer();
            }

            Destroy(this.gameObject);
        }
        if (collision.CompareTag("Enemy") && !_isEnemyLaser)
        {
            _enemy = collision.GetComponent<Enemy>();
            _player = GameObject.Find("Player").GetComponent<Player>();

            if (_enemy != null && _player != null)
            {
                _player.AddToScore(10);
            }
            if (_enemy != null)
            {
                _enemy.ExplodeEnemy();
            }

            Destroy(this.gameObject);
        }
    }
}

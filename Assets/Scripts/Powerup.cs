using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    //TODO: add additional life powerup
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int _id = 0;

    [SerializeField]
    private AudioClip _powerupSound;

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(_powerupSound, transform.position);
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                switch (_id)
                {
                    case 0: player.ActivateTripleShot(); break;
                    case 1: player.ActivateSpeedBoost(); break;
                    case 2: player.ActivateShield(); break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}

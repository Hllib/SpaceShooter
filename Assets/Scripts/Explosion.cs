using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;

    void Start()
    {
        Destroy(this.gameObject, 2.0f);
        FindAudioSource();
        _audioSource.Play();
    }

    private void FindAudioSource()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.Log("Audio Source in Explosion is NULL");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    private float _fireRate = 0.35f;
    public float PowerUpCoolDownRate = 5.0f;
    private float _canFire = 0.0f;

    private int _lives = 3;
    public int Score = 0;
    public static int BestScore;

    private bool IsTripleShotAllowed = false;
    private bool _isShieldActivated = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject[] _engines;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        FindSpawnManager();
        FindUIManager();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        transform.position = transform.position.x > 11 ? new Vector3(-11, transform.position.y, 0)
            : transform.position.x <= -11 ? new Vector3(11, transform.position.y, 0)
            : transform.position;
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (IsTripleShotAllowed)
        {
            Instantiate(_tripleShotPrefab, new Vector3(transform.position.x - 0.47f, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        }
    }

    public void DamageOnPlayer()
    {
        if (_isShieldActivated)
        {
            DeactivateShield();
            return;
        }

        _lives -= 1;
        _uiManager.UpdateLives(_lives);
        CheckEngineState();

        if (_lives < 1)
        {
            _uiManager.CheckForBestScore();
            _spawnManager.OnPlayerDeath();
            ExplodePlayer();
        }
    }

    private void ExplodePlayer()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 0.25f);
    }

    private void CheckEngineState()
    {
        switch (_lives)
        {
            case 1: _engines[0].gameObject.SetActive(true); break;
            case 2: _engines[1].gameObject.SetActive(true); break;
            default:
                _engines[1].gameObject.SetActive(false);
                _engines[0].gameObject.SetActive(false);
                break;
        }
    }

    private void FindSpawnManager()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); // find the object, get the component

        if (_spawnManager == null)
        {
            Debug.Log("SPAWN MANAGER IS NULL");
        }
    }

    private void FindUIManager()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.Log("UI MANAGER IS NULL");
        }
    }

    public void ActivateTripleShot()
    {
        IsTripleShotAllowed = true;
        StartCoroutine(TripleShotRoutine());
        _uiManager.SetTripleShotSlider();
    }

    IEnumerator TripleShotRoutine()
    {
        yield return new WaitForSeconds(PowerUpCoolDownRate);
        IsTripleShotAllowed = false;
        _uiManager.StopTripleShotSlider();
    }

    public void ActivateSpeedBoost()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostRoutine());
        _uiManager.SetBoostSlider();
    }

    IEnumerator SpeedBoostRoutine()
    {
        yield return new WaitForSeconds(PowerUpCoolDownRate);
        _speed /= _speedMultiplier;
        _uiManager.StopBoostSlider();
    }

    public void ActivateShield()
    {
        _isShieldActivated = true;
        _shieldVisualizer.SetActive(true);
    }

    public void DeactivateShield()
    {
        _isShieldActivated = false;
        _shieldVisualizer.SetActive(false);
    }

    public void AddToScore(int points)
    {
        Score += points;
        _uiManager.UpdateScore(Score);
    }
}


using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public int highScore = 0;

    public float heldShields = 0;
    public float maxShields;
    public bool shieldActive = false;
    public bool infiniteMode = false;

    public static float rotateSpeed = 0.85f;
    public float shieldAnimSpeed = 0.5f;

    public ParticleSystem playerShield;

    public GameObject[] sceneSpawners;
    public GameObject[] pickupSpawners;
    public List<string> tips;
    public bool isGameOver = false;
    public bool gamePaused = false;
    public bool playedTutorial = false;
    private bool previousGamePaused = false;
    public bool audioAlerts = false;

    public int minScore = 450;
    public int maxScore = 500;

    public static Action OnEnemyDestroyed;
    public static Action OnGameOver;
    public static Action OnPauseGame;
    public static Action OnResumeGame;
    public static Action OnShieldUsed;
    public static Action OnShieldHit;
    public static Action OnStartTutorial;

    public SaveData gameData;
    public KeyBinding keyBindings;

    public int currentStep = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        sceneSpawners = GameObject.FindGameObjectsWithTag("Spawner");
        pickupSpawners = GameObject.FindGameObjectsWithTag("PickupSpawner");
        //Debug.Log(Application.persistentDataPath);
        var loaded = SaveSystem.LoadGame();
        if (loaded == null)
        {
            var data = new SaveData();
            SaveSystem.SaveGame(data);
            gameData = data;
        }
        else
        {
            gameData = SaveSystem.LoadGame();
        }

        ApplyGameSettings();

    }

    void OnEnable()
    {
        PlayerScript.OnPlayerHit += GameOver;
        OnEnemyDestroyed += IncreaseScore;
        OnShieldHit += shieldHit;
    }

    void OnDisable()
    {
        PlayerScript.OnPlayerHit -= GameOver;
        OnEnemyDestroyed -= IncreaseScore;
        OnShieldHit -= shieldHit;
        score = 0;
        gamePaused = true;
        isGameOver = false;
        rotateSpeed = 0f;
    }

    void Update()
    {

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);

        if (!isGameOver && playedTutorial)
        {
            if (Input.GetKeyDown(keyBindings.pause))
            {
                gamePaused = !gamePaused;
            }

            if (Input.GetKeyDown(keyBindings.activateShield) && heldShields > 0 && !shieldActive)
            {
                AudioManager.Instance.PlaySoundEffect("Shield");
                playerShield.transform.DOScale(1, shieldAnimSpeed);
                shieldActive = true;
                heldShields = heldShields - 1;
                heldShields = Mathf.Clamp(heldShields, 0, maxShields);
            }

            if (gamePaused != previousGamePaused)
            {
                if (gamePaused)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }

                previousGamePaused = gamePaused;
            }
        }
    }


    public void StartTutorial()
    {
        Time.timeScale = 0.0001f;
        currentStep = 0;
        OnStartTutorial?.Invoke();
    }

    public void IncreaseStep()
    {
        currentStep++;
    }

    public void Pause()
    {
        OnPauseGame?.Invoke();
        Time.timeScale = 0.00001f;
    }

    public void Resume()
    {
        OnResumeGame?.Invoke();
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        Time.timeScale = 0.00001f;
        isGameOver = true;
        OnGameOver?.Invoke();

        if (score > gameData.highScore)
        {
            gameData.highScore = score;
            SaveSystem.SaveGame(gameData);
        }
        Debug.Log("Game Over");
    }

    public void IncreaseScore()
    {
        int gameScore = UnityEngine.Random.Range(minScore, maxScore + 1);
        score += gameScore;
    }

    public void shieldHit()
    {
        playerShield.transform.DOScale(0, shieldAnimSpeed);
        shieldActive = false;

    }

    public void IncreaseScoreWithValues(int lower, int upper)
    {
        int gameScore = UnityEngine.Random.Range(lower, upper + 1);
        score += gameScore;
    }


    public void ApplyGameSettings()
    {
        highScore = gameData.highScore;
        maxShields = gameData.maxShields;
        playedTutorial = gameData.playedTutorial;
        infiniteMode = gameData.infinteMode;
        audioAlerts = gameData.audioAlerts;
        keyBindings = gameData.keyBindings;

        foreach (GameObject obj in sceneSpawners)
        {
            if (obj.CompareTag("Spawner"))
            {
                Spawner script = obj.GetComponent<Spawner>();
                if (script != null)
                {
                    script.minimumSpawnInterval = gameData.minimumSpawnInterval;
                    script.spawnIntervalDecreaseRate = gameData.spawnIntervalDecreaseRate;
                    script.initialSpawnInterval = gameData.initialSpawnInterval;
                }
            }

        }
        foreach (GameObject obj in pickupSpawners)
        {

            if (obj.CompareTag("PickupSpawner"))
            {
                Spawner script = obj.GetComponent<Spawner>();
                if (script != null)
                {
                    script.minimumSpawnInterval = gameData.pickupminimumSpawnInterval;
                    script.spawnIntervalDecreaseRate = gameData.pickupspawnIntervalDecreaseRate;
                    script.initialSpawnInterval = gameData.pickupinitialSpawnInterval;
                }
            }

        }
    }
}

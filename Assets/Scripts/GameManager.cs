using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public int highScore = 0;

    public int heldPickups = 0;
    public int maxShields;

    public static float rotateSpeed = 0.85f;


    public GameObject[] sceneSpawners;

    public bool isGameOver = false;
    public bool gamePaused = false;
    private bool previousGamePaused = false;
    public bool autoShieldsEnabled = false;

    public int minScore = 450;
    public int maxScore = 500;

    public static Action OnEnemyDestroyed;
    public static Action OnGameOver;
    public static Action OnPauseGame;
    public static Action OnResumeGame;

    public SaveData gameData;

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance to this object
        Instance = this;

        // Ensure GameManager persists across scenes
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        sceneSpawners = GameObject.FindGameObjectsWithTag("Spawner");
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
    }

    void OnDisable()
    {
        PlayerScript.OnPlayerHit -= GameOver;
        OnEnemyDestroyed -= IncreaseScore;
        score = 0;
        gamePaused = true;
        isGameOver = false;
    }

    void Update()
    {

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);

        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                gamePaused = !gamePaused;
            }
            // Check if the pause state has changed
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

                // Update the previous state to the current state
                previousGamePaused = gamePaused;
            }
        }
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
        Debug.Log("Game Over");
    }

    public void IncreaseScore()
    {
        int gameScore = UnityEngine.Random.Range(minScore, maxScore + 1);
        score += gameScore;
    }

    public void IncreaseScoreWithValues(int lower, int upper)
    {
        int gameScore = UnityEngine.Random.Range(lower, upper + 1);
        score += gameScore;
    }


    void ApplyGameSettings()
    {
        highScore = gameData.highScore;
        maxShields = gameData.maxShields;
        foreach (GameObject obj in sceneSpawners)
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
}

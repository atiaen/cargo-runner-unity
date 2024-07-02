using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject confirmExitPanel;
    public GameObject gameOverPanel;

    public TMP_Text scoreText;
    public Image shieldImage;

    public float menuAnimSpeed;


    // Start is called before the first frame update
    void Start()
    {
        AdjustShieldImage();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score: {GameManager.Instance.score}";
        AdjustShieldImage();
        //shieldImage.DOFillAmount();
    }

    void OnEnable()
    {
        GameManager.OnPauseGame += OnPauseGame;
        GameManager.OnResumeGame += OnResumeGame;
        GameManager.OnGameOver += OnGameOver;
    }

    void OnDisable()
    {
        GameManager.OnPauseGame -= OnPauseGame;
        GameManager.OnResumeGame -= OnResumeGame;
        GameManager.OnGameOver -= OnGameOver;
    }

    public void OpenOptionsPanel()
    {
        pausePanel.transform.DOScale(0, menuAnimSpeed);
        gameOverPanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        optionsPanel.transform.DOScale(1.3f, menuAnimSpeed);
    }

    public void OnPauseGame()
    {
        pausePanel.transform.DOScale(1, menuAnimSpeed);
    }

    public void OnResumeGame()
    {
        GameManager.Instance.gamePaused = false;
        pausePanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        optionsPanel.transform.DOScale(0, menuAnimSpeed);
    }

    public void OnBackClick()
    {
        optionsPanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        pausePanel.transform.DOScale(1, menuAnimSpeed);

    }

    public void OnExitClick()
    {
        pausePanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(1.3f, menuAnimSpeed);
    }

    public void OnGameOver()
    {
        //GameManager.Instance.gamePaused = false;
        pausePanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        optionsPanel.transform.DOScale(0, menuAnimSpeed);
        gameOverPanel.transform.DOScale(1, menuAnimSpeed);
    }

    public void OnNoClick()
    {
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        pausePanel.transform.DOScale(1, menuAnimSpeed);

    }

    public void AdjustShieldImage()
    {
        float fillAmount = GameManager.Instance.heldShields / GameManager.Instance.maxShields;
        shieldImage.fillAmount = fillAmount;
    }

    public void GoToScene(string scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

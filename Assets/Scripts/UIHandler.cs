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
    public TMP_Text shieldsText;
    public TMP_Text highscoreText;
    public Image shieldImage;
    public TMP_Text tipText;


    public float menuAnimSpeed;

    public GameObject tutorialPanel;
    public TMP_Text tutorialText;
    public Button nextButton;
    public Image tutorialImage;

    public Sprite mouseSprite;
    public Sprite escapeKeySprite;
    public Sprite pKeySprite;

    public Sprite barrelSprite;
    public Sprite gemSprite;

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
        GameManager.OnStartTutorial += StartTutorial;
    }

    void OnDisable()
    {
        GameManager.OnPauseGame -= OnPauseGame;
        GameManager.OnResumeGame -= OnResumeGame;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnStartTutorial -= StartTutorial;

    }

    public void OpenOptionsPanel()
    {
        pausePanel.transform.DOScale(0, menuAnimSpeed);
        gameOverPanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        optionsPanel.SetActive(true);
        optionsPanel.transform.DOScale(1f, menuAnimSpeed);
    }

    public void OnPauseGame()
    {
        pausePanel.SetActive(true);
        pausePanel.transform.DOScale(1, menuAnimSpeed);
    }

    public void StartTutorial()
    {
        tutorialPanel.SetActive(true);
        tutorialPanel.transform.DOScale(1, menuAnimSpeed);
        StartPulsating();
        ShowNextStep();
    }

    public void EndTutorial()
    {
        AudioManager.Instance.PlaySoundEffect("Click");
        Time.timeScale = 1f;
        tutorialPanel.transform.DOScale(0, menuAnimSpeed);
        tutorialPanel.SetActive(false);
        GameManager.Instance.gameData.playedTutorial = true;
        SaveSystem.SaveGame(GameManager.Instance.gameData);
    }

    private void ShowNextStep()
    {
        switch (GameManager.Instance.currentStep)
        {
            case 0:
                tutorialText.text = "Welcome to Cargo Runners!! The goal of the game is simple. Last through the asteroid belt for as long as possible";
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 0.0001f;
                    GameManager.Instance.IncreaseStep();
                    AudioManager.Instance.PlaySoundEffect("Click");
                    ShowNextStep();
                });
                break;
            case 1:
                tutorialText.text = "The controls are simple. Use your mouse to control your ship. Click the left mouse button to activate your shields";
                tutorialImage.sprite = mouseSprite;
                tutorialImage.transform.DOScale(1, menuAnimSpeed);
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(() =>
                {
                    GameManager.Instance.IncreaseStep();
                    AudioManager.Instance.PlaySoundEffect("Click");
                    ShowNextStep();
                });
                break;
            case 2:
                StartCoroutine(KeysImageCoroutine());
                tutorialText.text = "You can always change your controls at anytime by pressing the 'Escape' key or 'P' key for the menu and clicking on 'Options'";
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(() =>
                {
                    GameManager.Instance.IncreaseStep();
                    AudioManager.Instance.PlaySoundEffect("Click");
                    ShowNextStep();

                });
                break;
            case 3:
                tutorialText.text = "In the options menu you can customize most of the gameplay such as the frequency of the asteroids or rebind the game controls. Feel free to explore!!!";
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(() =>
                {
                    GameManager.Instance.IncreaseStep();
                    AudioManager.Instance.PlaySoundEffect("Click");
                    ShowNextStep();

                });
                break;
            case 4:
                StartCoroutine(BarrelGemsCoroutine());
                tutorialText.text = "Lastly remember to pick up any 'barrels' or 'gems' if you see them floating around (they help alot!!)";
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(() =>
                {
                    GameManager.Instance.IncreaseStep();
                    AudioManager.Instance.PlaySoundEffect("Click");
                    ShowNextStep();

                });
                break;
            case 5:
                tutorialText.text = "Enjoy Cargo Runners!! and don't forget to fill in the survery in the main menu. ";
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(() => EndTutorial());
                break;
            default:
                EndTutorial();
                break;
        }
    }

    public void OnResumeGame()
    {
        GameManager.Instance.gamePaused = false;
        pausePanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        optionsPanel.transform.DOScale(0, menuAnimSpeed);
        pausePanel.SetActive(false);
        confirmExitPanel.SetActive(false);
        optionsPanel.SetActive(false);

    }

    public void OnBackClick()
    {
        optionsPanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        pausePanel.transform.DOScale(1, menuAnimSpeed);
        confirmExitPanel.SetActive(false);
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);

    }

    public void OnExitClick()
    {
        pausePanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(1.3f, menuAnimSpeed);

        pausePanel.SetActive(false);
        confirmExitPanel.SetActive(true);

    }

    public void StartPulsating()
    {
        DOTween.Sequence()
            .Append(nextButton.transform.DOScale(new Vector3(1, 1, 1) * 1.2f, 1f).SetEase(Ease.InOutQuad))
            .Append(nextButton.transform.DOScale(new Vector3(1, 1, 1), 1.2f).SetEase(Ease.InOutQuad))
            .SetLoops(-1);
    }


    public void OnGameOver()
    {
        //GameManager.Instance.gamePaused = false;
        ShowRandomTip();
        highscoreText.text = $"Current Score: {GameManager.Instance.score} \n Highscore: {GameManager.Instance.highScore}";
        scoreText.transform.DOScale(0, menuAnimSpeed);
        shieldImage.transform.DOScale(0, menuAnimSpeed);
        shieldsText.transform.DOScale(0, menuAnimSpeed);
        pausePanel.transform.DOScale(0, menuAnimSpeed);
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        optionsPanel.transform.DOScale(0, menuAnimSpeed);

        gameOverPanel.SetActive(true);
        gameOverPanel.transform.DOScale(1, menuAnimSpeed);

        confirmExitPanel.SetActive(false);
        pausePanel.SetActive(false);

    }

    public void ShowRandomTip()
    {
        var tips = GameManager.Instance.tips;
       int randomIndex = Random.Range(0, tips.Count);
        var tip = tips[randomIndex];

        tipText.text = $"Tip: {tip}";
    }

    IEnumerator KeysImageCoroutine()
    {
        tutorialImage.transform.DOScale(0, menuAnimSpeed);
        tutorialImage.sprite = escapeKeySprite;
        tutorialImage.transform.DOScale(1, menuAnimSpeed).WaitForCompletion();
        yield return new WaitForSecondsRealtime(3f);
        tutorialImage.transform.DOScale(0, menuAnimSpeed).WaitForCompletion();
        tutorialImage.sprite = pKeySprite;
        tutorialImage.transform.DOScale(1, menuAnimSpeed).WaitForCompletion();
        yield return new WaitForSecondsRealtime(3f);
        tutorialImage.transform.DOScale(0, menuAnimSpeed);

    }

    IEnumerator BarrelGemsCoroutine()
    {
        tutorialImage.sprite = barrelSprite;
        tutorialImage.transform.DOScale(1, menuAnimSpeed);
        yield return new WaitForSecondsRealtime(3f);
        tutorialImage.transform.DOScale(0, menuAnimSpeed);
        tutorialImage.sprite = gemSprite;
        tutorialImage.transform.DOScale(1, menuAnimSpeed);
        yield return new WaitForSecondsRealtime(3f);
        tutorialImage.transform.DOScale(0, menuAnimSpeed);

    }

    public void OnNoClick()
    {
        confirmExitPanel.transform.DOScale(0, menuAnimSpeed);
        pausePanel.transform.DOScale(1, menuAnimSpeed);

        confirmExitPanel.SetActive(false);
        pausePanel.SetActive(true);

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

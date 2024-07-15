using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuPanel;

    public GameObject optionsPanel;

    public GameObject confirmExit;

    public float mainMenuAnimSpeed;

    public float optionsAnimSpeed;

    public SaveData gameData;


    // Start is called before the first frame update
    void Start()
    {
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


    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * GameManager.rotateSpeed);

    }

    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);
        mainMenuPanel.transform.DOScale(0, mainMenuAnimSpeed);
        mainMenuPanel.SetActive(false);
        confirmExit.transform.DOScale(0, mainMenuAnimSpeed);
        optionsPanel.transform.DOScale(1, optionsAnimSpeed);
    }

    public void OnBackClick()
    {
        optionsPanel.transform.DOScale(0, optionsAnimSpeed);
        confirmExit.transform.DOScale(0, mainMenuAnimSpeed);
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        mainMenuPanel.transform.DOScale(1, mainMenuAnimSpeed);
        

    }

    public void OnExitClick()
    {
        confirmExit.SetActive(true);
        confirmExit.transform.DOScale(1, mainMenuAnimSpeed);
    }

    public void OnNoClick()
    {
        confirmExit.transform.DOScale(0, mainMenuAnimSpeed);
        confirmExit.SetActive(false);

    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

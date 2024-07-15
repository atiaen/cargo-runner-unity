using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class OptionsHandler : MonoBehaviour
{
    [Header("Music Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Asteroid Sliders")]
    public Slider miniumSlider;
    public Slider decreaseRateSlider;
    public Slider initalSpawnSlider;

    [Header("Pickup Sliders")]
    public Slider pickupMinimumSlider;
    public Slider pickupDecreaseRateSlider;
    public Slider pickupInitalSpawnSlider;

    [Header("Save Settings")]
    public Button saveButton;
    public Button resetButton;

    [Header("Input Settings")]
    public Slider moveSpeedSlider;

    public Button rebindLeftButton;
    public Button rebindRightButton;
    public Button rebindUpButton;
    public Button rebindDownButton;
    public Button rebindShieldButton;
    public Button rebindPauseButton;

    public Toggle infiniteToggle;
    public Toggle incomingAlert;
    public TMP_Dropdown controlSchemes;


    public GameObject confirmSettingsPanel;
    public GameObject confirmResetPanel;

    public SaveData saveData;

    private string currentBindingAction;
    // Start is called before the first frame update
    void Start()
    {


        saveData = SaveSystem.LoadGame();

        musicSlider.value = saveData.musicVolume;
        sfxSlider.value = saveData.soundEffectVolume;

        miniumSlider.value = saveData.minimumSpawnInterval;
        decreaseRateSlider.value = saveData.spawnIntervalDecreaseRate;
        initalSpawnSlider.value = saveData.initialSpawnInterval;

        pickupMinimumSlider.value = saveData.pickupminimumSpawnInterval;
        pickupDecreaseRateSlider.value = saveData.pickupspawnIntervalDecreaseRate;
        pickupInitalSpawnSlider.value = saveData.pickupinitialSpawnInterval;
        moveSpeedSlider.value = saveData.moveSpeed;

        infiniteToggle.isOn = saveData.infinteMode;
        incomingAlert.isOn = saveData.audioAlerts;

        controlSchemes.value = (int)saveData.controlType;

        musicSlider.onValueChanged.AddListener((val) =>
        {
            saveData.musicVolume = val;
        });

        sfxSlider.onValueChanged.AddListener((val) =>
        {
            saveData.soundEffectVolume = val;
        });

        miniumSlider.onValueChanged.AddListener((val) =>
        {
            saveData.minimumSpawnInterval = val;
        });

        decreaseRateSlider.onValueChanged.AddListener((val) =>
        {
            saveData.spawnIntervalDecreaseRate = val;
        });

        initalSpawnSlider.onValueChanged.AddListener((val) =>
        {
            saveData.initialSpawnInterval = val;
        });

        pickupInitalSpawnSlider.onValueChanged.AddListener((val) =>
        {
            saveData.pickupinitialSpawnInterval = val;
        });

        pickupMinimumSlider.onValueChanged.AddListener((val) =>
        {
            saveData.pickupminimumSpawnInterval = val;
        });

        pickupDecreaseRateSlider.onValueChanged.AddListener((val) =>
        {
            saveData.pickupspawnIntervalDecreaseRate = val;
        });

        moveSpeedSlider.onValueChanged.AddListener((val) =>
        {
            saveData.moveSpeed = val;
        });

        infiniteToggle.onValueChanged.AddListener((val) => {
            saveData.infinteMode = val;
        });

        incomingAlert.onValueChanged.AddListener((val) => {
            saveData.audioAlerts = val;
        });

        resetButton.onClick.AddListener(() =>
        {
            saveData.playedTutorial = false;
        });


        controlSchemes.onValueChanged.AddListener((val) =>
        {
            saveData.controlType = (ControlScheme)val;
        });


        rebindLeftButton.GetComponentInChildren<TMP_Text>().text = saveData.keyBindings.moveLeft.ToString();
        rebindRightButton.GetComponentInChildren<TMP_Text>().text = saveData.keyBindings.moveRight.ToString();
        rebindUpButton.GetComponentInChildren<TMP_Text>().text = saveData.keyBindings.moveUp.ToString();
        rebindDownButton.GetComponentInChildren<TMP_Text>().text = saveData.keyBindings.moveDown.ToString();
        rebindShieldButton.GetComponentInChildren<TMP_Text>().text = saveData.keyBindings.activateShield.ToString();
        rebindPauseButton.GetComponentInChildren<TMP_Text>().text = saveData.keyBindings.pause.ToString();

        rebindLeftButton.onClick.AddListener(() => StartBinding("MoveLeft"));
        rebindRightButton.onClick.AddListener(() => StartBinding("MoveRight"));
        rebindUpButton.onClick.AddListener(() => StartBinding("MoveUp"));
        rebindDownButton.onClick.AddListener(() => StartBinding("MoveDown"));
        rebindPauseButton.onClick.AddListener(() => StartBinding("Pause"));
        rebindShieldButton.onClick.AddListener(() => StartBinding("Shield"));

        //ApplySettings();

    }


    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener((val) =>
        {
            saveData.musicVolume = val;
        });

        sfxSlider.onValueChanged.RemoveListener((val) =>
        {
            saveData.soundEffectVolume = val;
        });

        miniumSlider.onValueChanged.RemoveListener((val) =>
        {
            saveData.minimumSpawnInterval = val;
        });

        decreaseRateSlider.onValueChanged.RemoveListener((val) =>
        {
            saveData.spawnIntervalDecreaseRate = val;
        });

        initalSpawnSlider.onValueChanged.RemoveListener((val) =>
        {
            saveData.initialSpawnInterval = val;
        });


        pickupInitalSpawnSlider.onValueChanged.RemoveListener((val) =>
        {
            saveData.pickupinitialSpawnInterval = val;
        });

        pickupMinimumSlider.onValueChanged.RemoveListener((val) =>
        {
            saveData.pickupminimumSpawnInterval = val;
        });

        pickupDecreaseRateSlider.onValueChanged.RemoveListener((val) =>
        {
            saveData.pickupspawnIntervalDecreaseRate = val;
        });

        moveSpeedSlider.onValueChanged.RemoveListener((val) =>
        {
            saveData.moveSpeed = val;
        });

        infiniteToggle.onValueChanged.RemoveListener((val) => {
            saveData.infinteMode = val;
        });

        incomingAlert.onValueChanged.RemoveListener((val) => {
            saveData.audioAlerts = val;
        });


        resetButton.onClick.RemoveListener(() =>
        {
            saveData.playedTutorial = false;
        });


        controlSchemes.onValueChanged.RemoveListener((val) =>
        {
            saveData.controlType = (ControlScheme)val;
        });
    }
    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
       
    }

    void StartBinding(string action)
    {
        currentBindingAction = action;
    }

    public void SaveSettings()
    {
        Debug.Log("Saved");
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            SaveSystem.SaveGame(saveData);
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            AudioManager.Instance.ReloadSettings();
            GameManager.Instance.ApplyGameSettings();

        }
        else
        {
            Debug.Log(saveData);
            SaveSystem.SaveGame(saveData);
            AudioManager.Instance.ReloadSettings();
        }
      
    }

    public void ResetTutorial()
    {
        saveData.playedTutorial = false;
        SaveSystem.SaveGame(saveData);
    }

    // Update is called once per frame
    void Update()
    {
        miniumSlider.value = saveData.minimumSpawnInterval;
        decreaseRateSlider.value = saveData.spawnIntervalDecreaseRate;
        initalSpawnSlider.value = saveData.initialSpawnInterval;
        infiniteToggle.isOn = saveData.infinteMode;
        controlSchemes.value = (int)saveData.controlType;


        if (!string.IsNullOrEmpty(currentBindingAction) && Input.anyKeyDown)
        {
          
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (currentBindingAction.Equals("MoveLeft"))
                    {
                        var text = rebindLeftButton.GetComponentInChildren<TMP_Text>();
                        text.text = key.ToString();
                    }
                    if (currentBindingAction.Equals("MoveRight"))
                    {
                        var text = rebindRightButton.GetComponentInChildren<TMP_Text>();
                        text.text = key.ToString();
                    }

                    if (currentBindingAction.Equals("MoveUp"))
                    {
                        var text = rebindUpButton.GetComponentInChildren<TMP_Text>();
                        text.text = key.ToString();
                    }


                    if (currentBindingAction.Equals("MoveDown"))
                    {
                        var text = rebindDownButton.GetComponentInChildren<TMP_Text>();
                        text.text = key.ToString();
                    }

                    if (currentBindingAction.Equals("Shield"))
                    {
                        var text = rebindShieldButton.GetComponentInChildren<TMP_Text>();
                        text.text = key.ToString();
                    }


                    if (currentBindingAction.Equals("Pause"))
                    {
                        var text = rebindPauseButton.GetComponentInChildren<TMP_Text>();
                        text.text = key.ToString();
                    }


                    saveData.keyBindings.SetKey(currentBindingAction, key);
                    currentBindingAction = null;
                    break;
                }
            }
        }

    }
}

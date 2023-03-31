using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    //Menu Toggles
    bool cmenuMain = false;
    bool cmenuPause = false;
    bool cmenuSettings = false;
    
    //GameObjects Manu Prefabs
    GameObject menuCanvas;
    GameObject menuMain;
    GameObject menuPause;
    GameObject menuSettings;
    //GameObjects Layout Groups
    GameObject groupGraphics;
    GameObject groupAudio;
    GameObject groupGameplay;
    //Buttons MainMenu
    Button buttonPlay;
    Button buttonMSettings;
    Button buttonCredits;
    Button buttonCloseApp;
    Button buttonClose;
    //Buttons PauseMenu
    Button buttonContinue;
    Button buttonPSettings;
    Button buttonReturnMain;
    //Buttons SettingsMenu
    Button buttonGraphics;
    Button buttonAudio;
    Button buttonGameplay;

    string currentScene; // Current scene name

    void Awake()
    {
        bindUIVariables();
        bindEventListeners();
        
        //buttonExit = GameObject.Find("Canvas/Menu_Main/TMPButton_Exit");
        Debug.Log(menuMain);

        //listeners


        List<GameObject> uiObjects = new List<GameObject>{menuCanvas, menuMain, menuPause, menuSettings};
        deActivateObjects(uiObjects);

        menuCanvas.SetActive(true);
        menuMain.SetActive(currentScene == "Lv0_MainMenu");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnPauseInput();
        }

    }
    
    //UI Events
    private void bindUIVariables()
    {
        currentScene = SceneManager.GetActiveScene().name;
        menuCanvas = GameObject.Find("Canvas");

        //Menu Prefabs
        menuMain = GameObject.Find("Menu_Main");
        menuPause = GameObject.Find("Canvas/Menu_Pause");
        menuSettings = GameObject.Find("Canvas/Menu_Settings");

        //Layout Groups
        groupGraphics = GameObject.Find("Canvas/Menu_Settings/GraphicsGroup");
        groupAudio = GameObject.Find("Canvas/Menu_Settings/AudioGroup");
        groupGameplay = GameObject.Find("Canvas/Menu_Settings/GameplayGroup");
        
        //Buttons MainMenu
        buttonPlay = GameObject.Find("Canvas/Menu_Main/TMPButton_Play").GetComponent<Button>();
        buttonMSettings = GameObject.Find("Canvas/Menu_Main/TMPButton_Settings").GetComponent<Button>();
        buttonCredits = GameObject.Find("Canvas/Menu_Main/TMPButton_Credits").GetComponent<Button>();
        buttonCloseApp = GameObject.Find("Canvas/Menu_Main/TMPButton_Exit").GetComponent<Button>();
        
        //Buttons PauseMenu
        buttonContinue = GameObject.Find("Canvas/Menu_Pause/PauseButtonsLayout/TMPButton_Continue").GetComponent<Button>();
        buttonPSettings = GameObject.Find("Canvas/Menu_Pause/PauseButtonsLayout/TMPButton_Settings").GetComponent<Button>();
        buttonReturnMain = GameObject.Find("Canvas/Menu_Pause/PauseButtonsLayout/TMPButton_Quit").GetComponent<Button>();

        //Buttons SettingsMenu
        buttonClose = GameObject.Find("Canvas/Menu_Settings/TMPButton_Close").GetComponent<Button>();
        buttonGraphics = GameObject.Find("Canvas/Menu_Settings/SettingsButtonsLayout/TMPButton_Graphics").GetComponent<Button>();
        buttonAudio = GameObject.Find("Canvas/Menu_Settings/SettingsButtonsLayout/TMPButton_Audio").GetComponent<Button>();
        buttonGameplay = GameObject.Find("Canvas/Menu_Settings/SettingsButtonsLayout/TMPButton_Gameplay").GetComponent<Button>();

    }

    private void bindEventListeners()
    {
        //Mainmenu Buttons
        buttonPlay.onClick.AddListener(OnPlayClick);
        buttonMSettings.onClick.AddListener(OnSettingsClick);
        buttonCredits.onClick.AddListener(OnButtonClick);
        buttonCloseApp.onClick.AddListener(OnExitClick);

        //Pausemenu Buttons
        buttonContinue.onClick.AddListener(OnPauseInput);
        buttonPSettings.onClick.AddListener(OnSettingsClick);
        buttonReturnMain.onClick.AddListener(OnExitClick);

        //Settingsmenu Buttons
        buttonClose.onClick.AddListener(OnSettingsClick);
        buttonGraphics.onClick.AddListener(OnGraphicsClick);
        buttonAudio.onClick.AddListener(OnAudioClick);
        buttonGameplay.onClick.AddListener(OnGameplayClick);

    }

    void OnButtonClick() 
    {
        Debug.Log("Button Clicked");
    }

    void OnPlayClick()
    {
        Debug.Log("Play Clicked");
        SceneManager.LoadScene("MainTesting");
    }

    void OnPauseInput() 
    {
        Debug.Log("Pause Input");
        if (menuPause.activeSelf)
        {
            menuPause.SetActive(false);
            if (currentScene == "Lv0_MainMenu")
            {
                menuMain.SetActive(true);
            }
        } else 
        {
            menuPause.SetActive(true);
            if (currentScene == "Lv0_MainMenu") 
            {
                menuMain.SetActive(false);
            }
            if (menuSettings.activeSelf)
            {
                menuSettings.SetActive(false);
            }
        }
    }

    void OnExitClick() 
    {
        Debug.Log("Exit or Quit Clicked");
        if (menuPause.activeSelf)
        {
            SceneManager.LoadScene("Lv0_MainMenu");
        } else
        {
            Application.Quit();
        }
    }

    void OnSettingsClickk() 
    {
        Debug.Log("Settings Clicked");
        if (menuSettings.activeSelf)
        {
            menuSettings.SetActive(false);
            groupGraphics.SetActive(false);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
        } else 
        {
            if (currentScene == "Lv0_MainMenu" && menuPause.activeSelf == true)
            {
                menuSettings.SetActive(true);
                groupGraphics.SetActive(true);
                groupAudio.SetActive(false);
                groupGameplay.SetActive(false);
                menuPause.SetActive(false);
            } 
            {

            }
            menuSettings.SetActive(true);
            groupGraphics.SetActive(true);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
        }
    }

    void OnSettingsClick() 
    {
        if (menuSettings.activeSelf == false && currentScene == "Lv0_MainMenu")
        {
            menuSettings.SetActive(true);
            groupGraphics.SetActive(true);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
        } else if (menuSettings.activeSelf == true && currentScene == "Lv0_MainMenu")
        {
            menuSettings.SetActive(false);
            groupGraphics.SetActive(false);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
        } else if (menuSettings.activeSelf == false && menuPause.activeSelf == true)
        {
            menuSettings.SetActive(true);
            groupGraphics.SetActive(true);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
            menuPause.SetActive(false);
        } else if (menuSettings.activeSelf == true && menuPause.activeSelf == false)
        {
            menuSettings.SetActive(false);
            groupGraphics.SetActive(false);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
            menuPause.SetActive(true);
        }
    }
    void OnGraphicsClick() 
    {
        Debug.Log("Graphics Clicked");
        groupGraphics.SetActive(true);
        groupAudio.SetActive(false);
        groupGameplay.SetActive(false);
    }

    void OnAudioClick() 
    {
        Debug.Log("Audio Clicked");
        groupGraphics.SetActive(false);
        groupAudio.SetActive(true);
        groupGameplay.SetActive(false);
    }

    void OnGameplayClick() 
    {
        Debug.Log("Gameplay Clicked");
        groupGraphics.SetActive(false);
        groupAudio.SetActive(false);
        groupGameplay.SetActive(true);
    }

    //Helpers
    private void deActivateObjects(List<GameObject> inputObjects) 
    {
        foreach (GameObject obj in inputObjects)
        {
            obj.SetActive(false);
        }

    }
}

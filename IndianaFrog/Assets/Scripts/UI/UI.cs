using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class UI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    //Menu Toggles
    bool cmenuMain = false;
    bool cmenuPause = false;
    bool cmenuSettings = false;
    
    //GameObjects Manu Prefabs
    GameObject menuCanvas;
    GameObject menuMain;
    GameObject menuPause;
    GameObject menuSettings;
    GameObject menuLevel;
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
    string joystick; //Should return a string for connected controller, "" if none connected

    void Awake()
    {
        bindUIVariables();
        bindEventListeners();
        joystick = Input.GetJoystickNames()[0];

        //buttonExit = GameObject.Find("Canvas/Menu_Main/TMPButton_Exit");
        Debug.Log(menuMain);

        //listeners

        List<GameObject> uiObjects = new List<GameObject>{menuCanvas, menuMain, menuPause, menuSettings, menuLevel};
        deActivateObjects(uiObjects);

        menuCanvas.SetActive(true);
        menuMain.SetActive(currentScene == "Lv0_MainMenu");
        if (!(joystick = Input.GetJoystickNames()[0]).Equals(""))
        {
            buttonPlay.Select();
        }
        else { EnableCursor(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pause"))
        {
            OnPauseInput();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (menuPause.activeSelf)
            {
                OnPauseInput();
            }
            else if (menuSettings.activeSelf)
            {
                OnSettingsClick();
            }
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
        menuLevel = GameObject.Find("Canvas/Menu_Level");

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
        buttonContinue = GameObject.Find("Canvas/Menu_Pause/PauseBG/PauseButtonsLayout/TMPButton_Continue").GetComponent<Button>();
        buttonPSettings = GameObject.Find("Canvas/Menu_Pause/PauseBG/PauseButtonsLayout/TMPButton_Settings").GetComponent<Button>();
        buttonReturnMain = GameObject.Find("Canvas/Menu_Pause/PauseBG/PauseButtonsLayout/TMPButton_Quit").GetComponent<Button>();

        //Buttons SettingsMenu
        buttonClose = GameObject.Find("Canvas/Menu_Settings/TMPButton_Close").GetComponent<Button>();
        buttonGraphics = GameObject.Find("Canvas/Menu_Settings/SettingsButtonsLayout/TMPButton_Graphics").GetComponent<Button>();
        buttonAudio = GameObject.Find("Canvas/Menu_Settings/SettingsButtonsLayout/TMPButton_Audio").GetComponent<Button>();
        buttonGameplay = GameObject.Find("Canvas/Menu_Settings/SettingsButtonsLayout/TMPButton_Gameplay").GetComponent<Button>();

    }

    private void bindEventListeners()
    {
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

    public void OnPlayClick()
    {
        Debug.Log("Play Clicked");
        SceneManager.LoadScene("MainTesting");
    }

    public void OnPauseInput() 
    {
        Debug.Log("Pause Input");
        if (menuPause.activeSelf)
        {
            DisableCursor();
            menuPause.SetActive(false);
            if (gameManager.IsPaused()) { gameManager.UnPause(); }
            if (currentScene == "Lv0_MainMenu") { menuMain.SetActive(true); }
        } else 
        {
            if ( !(joystick = Input.GetJoystickNames()[0]).Equals("") )
            {
                buttonContinue.Select();
            } else { EnableCursor(); }

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

    public void OnExitClick() 
    {
        Debug.Log("Exit or Quit Clicked");
        if (menuPause.activeSelf)
        {
            gameManager.UnPause();
            SceneManager.LoadScene("Lv0_MainMenu");
            if (!(joystick = Input.GetJoystickNames()[0]).Equals(""))
            {
                buttonContinue.Select();
            }
            else { EnableCursor(); }
        } else
        {
            Application.Quit();
        }
    }

    public void OnSettingsClick() 
    {
        if (!(joystick = Input.GetJoystickNames()[0]).Equals(""))
            {
                buttonGraphics.Select();
            }
            else { EnableCursor(); }

        if (menuSettings.activeSelf == false && currentScene == "Lv0_MainMenu")
        {
            menuSettings.SetActive(true);
            groupGraphics.SetActive(true);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
        } else if (menuSettings.activeSelf == true && currentScene == "Lv0_MainMenu")
        {
            if (!(joystick = Input.GetJoystickNames()[0]).Equals(""))
            {
                buttonPlay.Select();
            }
            else { EnableCursor(); }
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
            if (!(joystick = Input.GetJoystickNames()[0]).Equals(""))
            {
                buttonContinue.Select();
            }
            else { EnableCursor(); }
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

    //move to gameManager if we want to control cursor state globally
    private void EnableCursor() 
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class UI : MonoBehaviour
{
    string joystick; //Should be a string for connected controller, "" if none connected
    string currentScene;

    //Save state
    int highestLevelCompleted = 0;
    
    [Header("GameObjects Menu Prefabs")]
    //Menu Prefabs
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuLevel;
    [SerializeField] GameObject menuCredits;
    
    //Layout Group Prefabs
    [SerializeField] GameObject groupGraphics;
    [SerializeField] GameObject groupAudio;
    [SerializeField] GameObject groupGameplay;
    
    //MainMenu Buttons
    [SerializeField] Button buttonPlay;
    [SerializeField] Button buttonMainSettings;
    [SerializeField] Button buttonCredits;
    [SerializeField] Button buttonCloseApp;
    [SerializeField] Button buttonClose;
    
    //PauseMenu Buttons
    [SerializeField] Button buttonContinue;
    [SerializeField] Button buttonResartLevel;
    [SerializeField] Button buttonPauseSettings;
    [SerializeField] Button buttonReturnMain;
    
    //SettingsMenu Buttons
    [SerializeField] Button buttonGraphics;
    [SerializeField] Button buttonAudio;
    [SerializeField] Button buttonGameplay;
    
    //LevelMenu Buttons
    [SerializeField] Button[] buttonLevel;

    void Awake()
    {
        //intialize knowledge about the scene
        currentScene = SceneManager.GetActiveScene().name;
        joystick = TryCatchController();
        highestLevelCompleted = SaveSystem.GetHighestLevelCompleted();

        //Collect UI elements together and disable/hide them from view
        List<GameObject> uiObjects = new List<GameObject>{menuCanvas, menuMain, menuPause, menuSettings, menuLevel, menuCredits};
        DeActivateObjects(uiObjects);

        menuCanvas.SetActive(true);
        menuMain.SetActive(currentScene == "Main Menu");

        //Disable Level buttons based on Save data 
        for (int i= 0; i< buttonLevel.Length; i++)
        {
            if (i > highestLevelCompleted)
            {
                buttonLevel[i].interactable = false;
            }
        }

        //Set active button for controller if connected
        SelectButtonOrEnableCursor(buttonPlay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pause"))
        {
            if (SceneManager.GetActiveScene().name.Equals("Main Menu"))
            {
                return;
            } 
            else if (menuSettings.activeSelf) 
            {
                //Hackyway to handle the case of pressing pause when in settings menu
                GameManager.instance.Pause();
                OnPauseInput();
            }
            else
            {
                OnPauseInput();
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if ( menuSettings.activeSelf)
            {
                OnSettingsBackClick();
            }
            if ( menuPause.activeSelf)
            {
                OnPauseInput();
            }
            if ( menuLevel.activeSelf)
            {
                menuLevel.SetActive(false);
                SelectButtonOrEnableCursor(buttonPlay);
            }
            if ( menuCredits.activeSelf)
            {
                menuCredits.SetActive(false);
                SelectButtonOrEnableCursor(buttonPlay);
            }
        }

    }
    
    public void OnPlayClick()
    {
        //Check for save data: load level selector or first level
        highestLevelCompleted = SaveSystem.GetHighestLevelCompleted();
        if (highestLevelCompleted != 0)
        {
            if (!(menuLevel.activeSelf))
            {
                menuLevel.SetActive(true);
                SelectButtonOrEnableCursor(buttonLevel[0]);
                if (menuCredits.activeSelf) { menuCredits.SetActive(false); }
                if (menuSettings.activeSelf) { menuSettings.SetActive(false); }
            }
            else
            { menuLevel.SetActive(false); }
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    public void OnPauseInput() 
    {
        //Unpause
        if (menuPause.activeSelf)
        {
            DisableCursor();
            menuPause.SetActive(false);
            if (GameManager.instance.IsPaused()) { GameManager.instance.UnPause(); }
        }
        //Pause
        else
        {
            menuPause.SetActive(true);
            SelectButtonOrEnableCursor(buttonContinue);

            if (menuSettings.activeSelf)
            {
                menuSettings.SetActive(false);
            }
        }
    }

    public void OnRestartLevelClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExitClick() 
    {
        if (menuPause.activeSelf)
        {
            GameManager.instance.UnPause();
            SceneManager.LoadScene("Main Menu");
            SelectButtonOrEnableCursor(buttonContinue);
        } else
        {
            Application.Quit();
        }
    }

    public void OnSettingsPauseClick() 
    {
        if (menuCredits.activeSelf) { menuCredits.SetActive(false); }
        else if (menuLevel.activeSelf) { menuLevel.SetActive(false); }

        if (menuSettings.activeSelf == false && menuPause.activeSelf == true)
        {
            SelectButtonOrEnableCursor(buttonGraphics);
            menuSettings.SetActive(true);
            groupGraphics.SetActive(true);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
            menuPause.SetActive(false);
        }
        else if (menuSettings.activeSelf == true && menuPause.activeSelf == false)
        {
            SelectButtonOrEnableCursor(buttonContinue);
            menuSettings.SetActive(false);
            groupGraphics.SetActive(false);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
            menuPause.SetActive(true);
        }
    }

    public void OnSettingsMainClick() 
    {
        if ( menuCredits.activeSelf ) { menuCredits.SetActive(false);}
        else if (menuLevel.activeSelf) { menuLevel.SetActive(false);}
        SelectButtonOrEnableCursor(buttonGraphics);

        if ( menuSettings.activeSelf == false )
        {
            menuSettings.SetActive(true);
            groupGraphics.SetActive(true);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
        } else if (menuSettings.activeSelf == true )
        {
            SelectButtonOrEnableCursor(buttonPlay);
            menuSettings.SetActive(false);
            groupGraphics.SetActive(false);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
        }
    }

    public void OnSettingsBackClick()
    {
        if (currentScene.Equals("Main Menu"))
        {
            if (menuSettings.activeSelf)
            {
                SelectButtonOrEnableCursor(buttonPlay);
                menuSettings.SetActive(false);
                groupGraphics.SetActive(false);
                groupAudio.SetActive(false);
                groupGameplay.SetActive(false);
            }
        }
        else
        {
            if (menuSettings.activeSelf)
            {
                SelectButtonOrEnableCursor(buttonContinue);
                menuSettings.SetActive(false);
                groupGraphics.SetActive(false);
                groupAudio.SetActive(false);
                groupGameplay.SetActive(false);
                menuPause.SetActive(true);
            }
        }
    }

    public void OnGraphicsClick() 
    {
        groupGraphics.SetActive(true);
        groupAudio.SetActive(false);
        groupGameplay.SetActive(false);
    }

    public void OnAudioClick() 
    {
        groupGraphics.SetActive(false);
        groupAudio.SetActive(true);
        groupGameplay.SetActive(false);
    }

    public void OnGameplayClick() 
    {
        groupGraphics.SetActive(false);
        groupAudio.SetActive(false);
        groupGameplay.SetActive(true);
    }

    public void OnLevelClick(string levelName) 
    {
        SceneManager.LoadScene(levelName);
    }

    public void OnCreditsClick()
    {
        if ( !(menuCredits.activeSelf) ) { menuCredits.SetActive(true); } else { menuCredits.SetActive(false); }
        if ( menuSettings.activeSelf ) { menuSettings.SetActive(false);}
        if (menuLevel.activeSelf) { menuLevel.SetActive(false); }
    }

    //Helper Functions
    //move Cursor ones to gameManager if we want to control cursor state globally
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

    private void DeActivateObjects(List<GameObject> inputObjects) 
    {
        foreach (GameObject obj in inputObjects)
        {
            obj.SetActive(false);
        }

    }

    private string TryCatchController()
    {
        try
        {
            joystick = Input.GetJoystickNames()[0];
            return joystick;
        }
        catch (System.IndexOutOfRangeException)
        {
            return "";
        }
    }

    private void SelectButtonOrEnableCursor(Button button)
    {
        if (TryCatchController() != "")
        {
            button.Select();
        }
        else { EnableCursor(); }
    }
}

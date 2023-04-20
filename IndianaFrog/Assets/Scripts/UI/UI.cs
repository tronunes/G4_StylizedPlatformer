using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class UI : MonoBehaviour
{
    string joystick; //Should return a string for connected controller, "" if none connected
    string currentScene; // Current scene name

    //Save state
    int highestLevelCompleted = 0;
    
    //GameObjects Menu Prefabs
    GameObject menuCanvas;
    GameObject menuMain;
    GameObject menuPause;
    GameObject menuSettings;
    GameObject menuLevel;
    GameObject menuCredits;
    
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
    
    //Level buttons
    Button[] buttonLevel;

    void Awake()
    {
        bindUIVariables();
        joystick = trycatchController();
        highestLevelCompleted = SaveSystem.GetHighestLevelCompleted();

        List<GameObject> uiObjects = new List<GameObject>{menuCanvas, menuMain, menuPause, menuSettings, menuLevel, menuCredits};
        deActivateObjects(uiObjects);

        menuCanvas.SetActive(true);
        menuMain.SetActive(currentScene == "Main Menu");

        for (int i= 0; i< buttonLevel.Length; i++)
        {
            if (i > highestLevelCompleted)
            {
                buttonLevel[i].interactable = false;
            }
        }

        if (trycatchController() != "")
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
            if ( menuSettings.activeSelf)
            {
                OnSettingsBackClick();
            }
            else if (menuPause.activeSelf)
            {
                OnPauseInput();
            }
        }

    }
    
    //UI Events
    private void bindUIVariables()
    {
        currentScene = SceneManager.GetActiveScene().name;
        menuCanvas = GameObject.Find("Menu_Canvas");

        //Menu Prefabs
        menuMain = GameObject.Find("Menu_Canvas/Menu_Main");
        menuPause = GameObject.Find("Menu_Canvas/Menu_Pause");
        menuSettings = GameObject.Find("Menu_Canvas/Menu_Settings");
        menuLevel = GameObject.Find("Menu_Canvas/Menu_Level");
        menuCredits = GameObject.Find("Menu_Canvas/Menu_Credits");

        //Layout Groups
        groupGraphics = GameObject.Find("Menu_Canvas/Menu_Settings/GraphicsGroup");
        groupAudio = GameObject.Find("Menu_Canvas/Menu_Settings/AudioGroup");
        groupGameplay = GameObject.Find("Menu_Canvas/Menu_Settings/GameplayGroup");
        
        //Buttons MainMenu
        buttonPlay = GameObject.Find("Menu_Canvas/Menu_Main/TMPButton_Play").GetComponent<Button>();
        buttonMSettings = GameObject.Find("Menu_Canvas/Menu_Main/TMPButton_Settings").GetComponent<Button>();
        buttonCredits = GameObject.Find("Menu_Canvas/Menu_Main/TMPButton_Credits").GetComponent<Button>();
        buttonCloseApp = GameObject.Find("Menu_Canvas/Menu_Main/TMPButton_Exit").GetComponent<Button>();
        
        //Buttons PauseMenu
        buttonContinue = GameObject.Find("Menu_Canvas/Menu_Pause/PauseBG/PauseButtonsLayout/TMPButton_Continue").GetComponent<Button>();
        buttonPSettings = GameObject.Find("Menu_Canvas/Menu_Pause/PauseBG/PauseButtonsLayout/TMPButton_Settings").GetComponent<Button>();
        buttonReturnMain = GameObject.Find("Menu_Canvas/Menu_Pause/PauseBG/PauseButtonsLayout/TMPButton_Quit").GetComponent<Button>();

        //Buttons SettingsMenu
        buttonClose = GameObject.Find("Menu_Canvas/Menu_Settings/BottomBarGroup/TMPButton_Close").GetComponent<Button>();
        buttonGraphics = GameObject.Find("Menu_Canvas/Menu_Settings/TopBarGroup/SettingsButtonsLayout/TMPButton_Graphics").GetComponent<Button>();
        buttonAudio = GameObject.Find("Menu_Canvas/Menu_Settings/TopBarGroup/SettingsButtonsLayout/TMPButton_Audio").GetComponent<Button>();
        buttonGameplay = GameObject.Find("Menu_Canvas/Menu_Settings/TopBarGroup/SettingsButtonsLayout/TMPButton_Gameplay").GetComponent<Button>();

        //Get Level buttons
        buttonLevel = GameObject.Find("Menu_Canvas/Menu_Level/LevelLayoutGroup").GetComponentsInChildren<Button>();
    }

    public void OnPlayClick()
    {
        highestLevelCompleted = SaveSystem.GetHighestLevelCompleted();
        if (highestLevelCompleted != 0)
        {
            if ( !(menuLevel.activeSelf) ) { menuLevel.SetActive(true); } else { menuLevel.SetActive(false); }
        } 
        else 
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    public void OnPauseInput() 
    {
        Debug.Log("Pause Input");
        if (menuPause.activeSelf)
        {
            DisableCursor();
            menuPause.SetActive(false);
            if (GameManager.instance.IsPaused()) { GameManager.instance.UnPause(); }
            if (currentScene == "Lv0_MainMenu") { menuMain.SetActive(true); }
        } else 
        {
            if ( trycatchController() != "" )
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
            GameManager.instance.UnPause();
            SceneManager.LoadScene("Main Menu");
            if ( trycatchController() != "" )
            {
                buttonContinue.Select();
            }
            else { EnableCursor(); }
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
            if (trycatchController() != "")
            {
                buttonGraphics.Select();
            }
            else { EnableCursor(); }
            menuSettings.SetActive(true);
            groupGraphics.SetActive(true);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
            menuPause.SetActive(false);
        }
        else if (menuSettings.activeSelf == true && menuPause.activeSelf == false)
        {
            if (trycatchController() != "")
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

    public void OnSettingsMainClick() 
    {
        if ( trycatchController() != "" )
            {
                buttonGraphics.Select();
            }
            else { EnableCursor(); }

        if ( menuCredits.activeSelf ) { menuCredits.SetActive(false);}
        else if (menuLevel.activeSelf) { menuLevel.SetActive(false);}

        if ( menuSettings.activeSelf == false )
        {
            menuSettings.SetActive(true);
            groupGraphics.SetActive(true);
            groupAudio.SetActive(false);
            groupGameplay.SetActive(false);
        } else if (menuSettings.activeSelf == true )
        {
            if ( trycatchController() != "" )
            {
                buttonPlay.Select();
            }
            else { EnableCursor(); }
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
                if (trycatchController() != "")
                {
                    buttonPlay.Select();
                }
                else { EnableCursor(); }
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
                if (trycatchController() != "")
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
    }

    public void OnGraphicsClick() 
    {
        Debug.Log("Graphics Clicked");
        groupGraphics.SetActive(true);
        groupAudio.SetActive(false);
        groupGameplay.SetActive(false);
    }

    public void OnAudioClick() 
    {
        Debug.Log("Audio Clicked");
        groupGraphics.SetActive(false);
        groupAudio.SetActive(true);
        groupGameplay.SetActive(false);
    }

    public void OnGameplayClick() 
    {
        Debug.Log("Gameplay Clicked");
        groupGraphics.SetActive(false);
        groupAudio.SetActive(false);
        groupGameplay.SetActive(true);
    }

    public void OnLevelClick(string levelName) 
    {
        Debug.Log($"Level Clicked: {levelName}");
        SceneManager.LoadScene(levelName);
    }

    public void OnCreditsClick()
    {
        if ( menuSettings.activeSelf ) { menuSettings.SetActive(false);}
        else if (menuSettings.activeSelf) { menuSettings.SetActive(false);}
        if ( !(menuCredits.activeSelf) ) { menuCredits.SetActive(true); } else { menuCredits.SetActive(false); }
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

    public string trycatchController()
    {
        try
        {
            joystick = Input.GetJoystickNames()[0];
            return joystick;
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log("No Controller");
            return "";
        }
    }
}

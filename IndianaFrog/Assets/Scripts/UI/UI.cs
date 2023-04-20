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
    
    //GameObjects Menu Prefabs
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuLevel;
    [SerializeField] GameObject menuCredits;
    
    //GameObjects Layout Groups
    [SerializeField] GameObject groupGraphics;
    [SerializeField] GameObject groupAudio;
    [SerializeField] GameObject groupGameplay;
    
    //Buttons MainMenu
    [SerializeField] Button buttonPlay;
    [SerializeField] Button buttonMainSettings;
    [SerializeField] Button buttonCredits;
    [SerializeField] Button buttonCloseApp;
    [SerializeField] Button buttonClose;
    
    //Buttons PauseMenu
    [SerializeField] Button buttonContinue;
    [SerializeField] Button buttonPauseSettings;
    [SerializeField] Button buttonReturnMain;
    
    //Buttons SettingsMenu
    [SerializeField] Button buttonGraphics;
    [SerializeField] Button buttonAudio;
    [SerializeField] Button buttonGameplay;
    
    //Level buttons
    [SerializeField] Button[] buttonLevel;

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene().name;
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
    
    public void OnPlayClick()
    {
        highestLevelCompleted = SaveSystem.GetHighestLevelCompleted();
        if (highestLevelCompleted != 0)
        {
            if ( !(menuLevel.activeSelf) ) { menuLevel.SetActive(true); } else { menuLevel.SetActive(false); }
            if ( trycatchController() != "" )
            {
                buttonLevel[0].Select();
            } else { EnableCursor(); }
        } 
        else 
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    public void OnPauseInput() 
    {
        if (menuPause.activeSelf)
        {
            DisableCursor();
            menuPause.SetActive(false);
            if (GameManager.instance.IsPaused()) { GameManager.instance.UnPause(); }
            if (currentScene == "Main Menu") { menuMain.SetActive(true); }
        } else 
        {
            if (SceneManager.GetActiveScene().name.Equals("Main Menu"))
            {
                return;
            }
            else
            {
                menuPause.SetActive(true);
                if (trycatchController() != "")
                {
                    buttonContinue.Select();
                }
                else { EnableCursor(); }

                if (menuSettings.activeSelf)
                {
                    menuSettings.SetActive(false);
                }
            }
        }
    }

    public void OnExitClick() 
    {
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
            return "";
        }
    }
}

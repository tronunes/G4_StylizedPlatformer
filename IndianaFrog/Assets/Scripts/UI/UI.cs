using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    //Menu Toggles
    bool cmenuMain = false;
    bool cmenuPause = false;
    bool cmenuSettings= false;
    //GameObjects
    GameObject menuCanvas;
    GameObject menuMain;
    GameObject menuPause;
    GameObject menuSettings;
    string currentScene; // Current scene name

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene().name;
        menuCanvas = GameObject.Find("Canvas");
        menuMain = GameObject.Find("Menu_Main");
        menuPause = GameObject.Find("Canvas/Menu_Pause");
        menuSettings = GameObject.Find("Canvas/Menu_Settings");
        Debug.Log(menuMain);

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
        
    }

    private void deActivateObjects(List<GameObject> inputObjects) 
    {
        foreach (GameObject obj in inputObjects)
        {
            obj.SetActive(false);
        }

    }
}

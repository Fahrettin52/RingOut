using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Code_MenuManager : MonoBehaviour {

    public List<GameObject> playerSelectMenuButtons = new List<GameObject>(); // The list that contains all the buttons in the player select menu
    public List<GameObject> mainMenuButtons = new List<GameObject>(); // The list that contains all the buttons in the main menu
    public List<GameObject> settingsButtons = new List<GameObject>(); // The list that contains all the buttons in the settings menu
    public List<GameObject> controlButtons = new List<GameObject>(); // The list that contains all the buttons in the controls menu
    public List<GameObject> confirmQuitButtons = new List<GameObject>(); // The list that contains all the buttons in the confirm quit menu
    public List<GameObject> playAgainButtons = new List<GameObject>(); // The list that contains all the buttons in the play again menu

    public GameObject playerSelectMenu; // The player select gameobject
    public GameObject mainMenuBorder; // The main menu gameobject
    public GameObject settingsMenu; // The settingsmenu gameobject
    public GameObject controlsMenu; // The controls menu gameobject
    public GameObject confirmQuit; // The confirm quit menu gameobject
    public GameObject playAgain; // The play again menu gameobject

    public GameObject soundManager; // Store the soundmanager 

    public EventSystem eventSystem; // Store the eventsystem 

    /// Use this for initialization
    public void Start() {
        // Call the InitializeMenu methode
        InitializeMenu();
    }

    /// <summary>
    /// Initializes the game
    /// </summary>
    public virtual void InitializeMenu() {
        // Add all the buttons in the mainmenu to the mainmenu list
        foreach (Transform item in mainMenuBorder.transform) {
            // Only add the buttons to the list
            if (item.GetComponent<Button>() != null) {
                mainMenuButtons.Add(item.gameObject);
            }
        }
        // Add all the buttons in the settingsmenu to the settingsmenu list
        foreach (Transform item in settingsMenu.transform) {
            // Only add the buttons and toggles to the list
            if (item.GetComponent<Button>() != null || item.GetComponent<Toggle>() != null) {
                settingsButtons.Add(item.gameObject);
            }
        }
        // Add all the buttons in the controlsmenu to the settingsmenu list
        foreach (Transform item in controlsMenu.transform) {
            if (item.GetComponent<Button>() != null) {
                controlButtons.Add(item.gameObject);
            }
        }
        // Add all the buttons in the confirmquitmenu to the confirmquit list
        foreach (Transform item in confirmQuit.transform) {
            // Only add the buttons to the list
            if (item.GetComponent<Button>() != null) {
                confirmQuitButtons.Add(item.gameObject);
            }
        }
        // Check if the mainmenu gameobject is active
        if (mainMenuBorder.activeSelf != false) {
            // Pick the first button in the mainmenu 
            PickFirstButton(mainMenuButtons, mainMenuBorder.activeSelf, 0);
        }
    }

    /// <summary>
    /// Toggles the mainmenu border and picks the first button for selection
    /// </summary>
    /// <param name="number"></param>
    public void ToggleMenus(int number) {
        // Toggle the mainmenu border
        mainMenuBorder.SetActive(!mainMenuBorder.activeSelf);
 
        switch (number) {
            case 0:
                // If the mainmenu is active pick the first button
                if (mainMenuBorder.activeInHierarchy) {
                    eventSystem.SetSelectedGameObject(mainMenuButtons[0]);
                }
                // Clear the SetSelectedGameObject
                else {
                    eventSystem.SetSelectedGameObject(null);
                }
                break;
            case 1:
                // Toggle the settingsmenu
                settingsMenu.SetActive(!settingsMenu.activeSelf);
                // Select the button that activates the settingsmenu
                PickFirstButton(settingsButtons, !settingsMenu.activeSelf, number);
                break;
            case 2:
                // Toggle the controlsmenu
                controlsMenu.SetActive(!controlsMenu.activeSelf);
                // Select the button that activates the controlsmenu
                PickFirstButton(controlButtons, !controlsMenu.activeSelf, number);
                break;
            case 3:
                // Toggle the confirmquitmenu
                confirmQuit.SetActive(!confirmQuit.activeSelf);
                // Select the button that activates the confirmquitmenu
                PickFirstButton(confirmQuitButtons, !confirmQuit.activeSelf, number);
                break;
            case 4:
                // Toggle the playerselectmenu
                playerSelectMenu.SetActive(!playerSelectMenu.activeSelf);
                // Select the button that activates the playerselectmenu
                PickFirstButton(playerSelectMenuButtons, !playerSelectMenu.activeSelf, number);
                break;
            case 5:
                // Toggle the confirmquitmenu
                confirmQuit.SetActive(!confirmQuit.activeSelf);
                // Select the button that activates the confirmquitmenu
                PickFirstButton(confirmQuitButtons, !confirmQuit.activeSelf, number);
                break;
            case 6:
                // Toggle the playagain menu
                mainMenuBorder.SetActive(!mainMenuBorder.activeSelf);
                playAgain.SetActive(!playAgain.activeSelf);
                // Select the button that activates the playagain menu
                PickFirstButton(playAgainButtons, !playAgain.activeSelf, 6);
                break;
            // If non of the above methodes are called break the code
            default:
                break;
        }
    }

    /// <summary>
    /// Load the scene with the scene index
    /// </summary>
    /// <param name="scene"></param>
    public virtual void LoadGameScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitGame() {
        Application.Quit();
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public virtual void Pause() {
        // Pause the game
        if (Time.timeScale == 0) {
            Time.timeScale = 1;
        }
        // Unpause the game
        else { 
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Picks the first button in the list
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="boolean"></param>
    /// <param name="number"></param>
    public void PickFirstButton(List<GameObject> gameObject, bool boolean, int number) {
        if (boolean) {
            // Select the button in the list with the index 
            eventSystem.SetSelectedGameObject(mainMenuButtons[number]);
        }
        else {
            // Select the first button in the list
            eventSystem.SetSelectedGameObject(gameObject[0]);
        }
    }
}

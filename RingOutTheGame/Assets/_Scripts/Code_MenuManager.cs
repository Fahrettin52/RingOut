using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class Code_MenuManager : MonoBehaviour {

    public List<GameObject> playerSelectMenuButtons = new List<GameObject>();
    public List<GameObject> mainMenuButtons = new List<GameObject>();
    public List<GameObject> settingsButtons = new List<GameObject>();
    public List<GameObject> controlButtons = new List<GameObject>();
    public List<GameObject> confirmQuitButtons = new List<GameObject>();

    public GameObject playerSelectMenu;
    public GameObject mainMenuBorder;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    public GameObject confirmQuit;
    public GameObject soundManager;

    public EventSystem eventSystem;

    /// Use this for initialization
    public void Start() {
        
        InitializeMenu();
    }

    /// <summary>
    /// Initializes the game
    /// </summary>
    public virtual void InitializeMenu() {

        foreach (Transform item in mainMenuBorder.transform) {
            if (item.GetComponent<Button>() != null) {
                mainMenuButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in settingsMenu.transform) {
            if (item.GetComponent<Button>() != null || item.GetComponent<Toggle>() != null) {
                settingsButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in controlsMenu.transform) {
            if (item.GetComponent<Button>() != null) {
                controlButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in confirmQuit.transform) {
            if (item.GetComponent<Button>() != null) {
                confirmQuitButtons.Add(item.gameObject);
            }
        }

        if (mainMenuBorder.activeSelf != false) {
            PickFirstButton(mainMenuButtons, mainMenuBorder.activeSelf, 0);
        }
    }

    /// <summary>
    /// Toggles the MainMenu elements and picks the first element for selection
    /// </summary>
    /// <param name="number"></param>
    public void ToggleMenus(int number) {

        mainMenuBorder.SetActive(!mainMenuBorder.activeSelf);

        switch (number) {
            case 0:
                if (mainMenuBorder.activeInHierarchy) {
                    eventSystem.SetSelectedGameObject(mainMenuButtons[0]);
                }
                else {
                    eventSystem.SetSelectedGameObject(null);
                }
                break;
            case 1:
                settingsMenu.SetActive(!settingsMenu.activeSelf);
                PickFirstButton(settingsButtons, !settingsMenu.activeSelf, number);
                break;
            case 2:
                controlsMenu.SetActive(!controlsMenu.activeSelf);
                PickFirstButton(controlButtons, !controlsMenu.activeSelf, number);
                break;
            case 3:
                confirmQuit.SetActive(!confirmQuit.activeSelf);
                PickFirstButton(confirmQuitButtons, !confirmQuit.activeSelf, number);
                break;
            case 4:
                playerSelectMenu.SetActive(!playerSelectMenu.activeSelf);
                PickFirstButton(playerSelectMenuButtons, !playerSelectMenu.activeSelf, number);
                break;

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
    public void Pause() {
        if (Time.timeScale == 0) {
            Time.timeScale = 1;
        }
        else { 
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Picks the first elemnt in the list
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="boolean"></param>
    /// <param name="number"></param>
    public void PickFirstButton(List<GameObject> gameObject, bool boolean, int number) {
        if (boolean) {
            eventSystem.SetSelectedGameObject(mainMenuButtons[number]);
        }
        else {
            eventSystem.SetSelectedGameObject(gameObject[0]);
        }
    }
}

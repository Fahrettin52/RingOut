using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class Code_MenuManager : MonoBehaviour {

    public List<GameObject> mainMenuButtons = new List<GameObject>();
    public List<GameObject> settingsButtons = new List<GameObject>();
    public List<GameObject> controlButtons = new List<GameObject>();
    public List<GameObject> confirmQuitButtons = new List<GameObject>();
    public GameObject mainMenuBorder;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    public GameObject confirmQuit;

    public EventSystem eventSystem;

    public bool pauseToggle;

    // Use this for initialization
    public void Start() {
        InitializeMenu();
    }

    // Initializes the game
    public virtual void InitializeMenu() {
        foreach (Transform item in mainMenuBorder.transform) {
            if (item.GetComponent<Button>() != null)
            {
                mainMenuButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in settingsMenu.transform) {
            if (item.GetComponent<Button>() != null || item.GetComponent<Toggle>() != null)
            {
                settingsButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in controlsMenu.transform) {
            if (item.GetComponent<Button>() != null)
            {
                controlButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in confirmQuit.transform) {
            if (item.GetComponent<Button>() != null)
            {
                confirmQuitButtons.Add(item.gameObject);
            }
        }

        if (mainMenuBorder.activeSelf != false)
        {
            PickFirstButton(mainMenuButtons, mainMenuBorder.activeSelf, 0);
        }

        pauseToggle = false;
    }

    public void ToggleMenus(int number) {

        mainMenuBorder.SetActive(!mainMenuBorder.activeSelf);

        switch (number) {
            case 0:
                PickFirstButton(mainMenuButtons, !mainMenuBorder.activeSelf, number);
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

            default:
                break;
        }
    }

    // Load the scene with the scene index
    public virtual void LoadGameScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    // Quits the application
    public void QuitGame() {
        Application.Quit();
    }

    // Pause the game
    public void Pause() {
        if (pauseToggle) {
            Time.timeScale = 1;
        }
        else { 
            Time.timeScale = 0;
        }
        pauseToggle = !pauseToggle;
    }

    public void PickFirstButton(List<GameObject> gameObject, bool boolean, int number) {
        if (boolean) {
            eventSystem.SetSelectedGameObject(mainMenuButtons[number]);
        }
        else {
            eventSystem.SetSelectedGameObject(gameObject[0]);
        }
    }
}

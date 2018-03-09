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

    // Use this for initialization
    public void Start() {
        InitializeMenu();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("StartButton"))
        {
            ToggleMainMenu();
        }
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
    }

    // Toggles the mainMenu
    public void ToggleMainMenu() {
        mainMenuBorder.SetActive(!mainMenuBorder.activeSelf);
        PickFirstButton(mainMenuButtons, !mainMenuBorder.activeSelf, 0);
    }

    // Toggles the settingsMenu
    public void ToggleSettingMenu() {
        mainMenuBorder.SetActive(!mainMenuBorder.activeSelf);
        settingsMenu.SetActive(!settingsMenu.activeSelf);
        PickFirstButton(settingsButtons, !settingsMenu.activeSelf, 1);
    }

    // Toggles the controlsMenu
    public void ToggleControlsMenu() {
        mainMenuBorder.SetActive(!mainMenuBorder.activeSelf);
        controlsMenu.SetActive(!controlsMenu.activeSelf);
        PickFirstButton(controlButtons, !controlsMenu.activeSelf, 2);
    }

    // Toggles the confirmQuit
    public void ToggleConformQuit() {
        mainMenuBorder.SetActive(!mainMenuBorder.activeSelf);
        confirmQuit.SetActive(!confirmQuit.activeSelf);
        PickFirstButton(confirmQuitButtons, !confirmQuit.activeSelf, 3);
    }

    // Load the scene with the scene index
    public void LoadGameScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    // Quits the application
    public void QuitGame() {
        Application.Quit();
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

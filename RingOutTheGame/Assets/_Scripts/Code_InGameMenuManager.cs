using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Code_InGameMenuManager : Code_MenuManager {

    public Code_GameManager gameMng; // The game manager in the scene
    public Code_SoundManager soundMng; // The Code_SoundManager

    public bool allowStart; // Boolean to check if you may pause he game

    // Update is called once per frame
    void Update() {
        // When pressing the start button on the XBox controller
        if (Input.GetButtonDown("StartButton") && !playerSelectMenu.activeSelf && allowStart) {
            // Check if other menu's are open
            if (!CheckMenuElements()) {
                // Pause the game
                Pause();
                // Open the mainmenu and select the first button
                ToggleMenus(0);
            }
        }
    }

    /// <summary>
    /// Checks if any menu is open except for the mainmenu
    /// </summary>
    public bool CheckMenuElements() {
        if (settingsMenu.activeInHierarchy || controlsMenu.activeInHierarchy || confirmQuit.activeInHierarchy) {
            return true;
        }
        else {
            return false;
        }
    }

    /// <summary>
    /// Initializes the game
    /// </summary>
    public override void InitializeMenu() {
        // Add all the buttons in the playerselectmenu to the playerselect list
        foreach (Transform item in playerSelectMenu.transform) {
            // Only add the buttons to the list
            if (item.GetComponent<Button>() != null) {
                playerSelectMenuButtons.Add(item.gameObject);
            }
        }
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
        // Add all the buttons in the replayagaintmenu to the playagain list
        foreach (Transform item in playAgain.transform) {
            // Only add the buttons to the list
            if (item.GetComponent<Button>() != null) {
                playAgainButtons.Add(item.gameObject);
            }
        }
        // Pick the first button in the mainmenu 
        PickFirstButton(playerSelectMenuButtons, !playerSelectMenu.activeSelf, 0);
    }

    /// <summary>
    /// Load the scene with the scene index
    /// </summary>
    /// <param name="scene"></param>
    public override void LoadGameScene(int scene) {
        SceneManager.LoadScene(scene);
        // Pause the game
        Pause();
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public override void Pause() {
        // Unpause the game
        if (Time.timeScale == 0) {
            //Time.timeScale = 1;
            // Play the ambient music
            if (soundMng.volumeChecks[0].isOn && soundMng.volumeChecks[1].isOn) {
                soundMng.musicAudioSource[0].mute = true;
                soundMng.musicAudioSource[1].mute = false;
                soundMng.PlayGameMusic();
            }
        }
        // Pause the game
        else {            
            Time.timeScale = 0;
            allowStart = false;

            // Play the mainmenu music
            if (soundMng.volumeChecks[0].isOn && soundMng.volumeChecks[1].isOn) {
                soundMng.musicAudioSource[0].mute = false;
                soundMng.musicAudioSource[1].mute = true;
                soundMng.PlayMainMenuMusic();                        
            }
        }
    }
}

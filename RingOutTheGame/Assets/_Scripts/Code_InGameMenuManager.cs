using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Code_InGameMenuManager : Code_MenuManager {

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("StartButton")) {
            if (!CheckMenuElements()) {
                Pause();
                ToggleMenus(0);
            }
        }
    }

    /// <summary>
    /// Load the scene with the scene index
    /// </summary>
    /// <param name="scene"></param>
    public override void LoadGameScene(int scene) {
        SceneManager.LoadScene(scene);
        Pause();
    }

    /// <summary>
    /// Checks if the elements in the menu are active
    /// </summary>
    public bool CheckMenuElements() {
        if (settingsMenu.activeInHierarchy || controlsMenu.activeInHierarchy || confirmQuit.activeInHierarchy) {
            return true;
        }
        else {
            return false;
        }
    }
}

using UnityEngine.SceneManagement;
using UnityEngine;

public class Code_InGameMenuManager : Code_MenuManager {

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("StartButton")) {
            if (!CheckMenuElements())
            {
                Pause();

                ToggleMenus(0);
            }
        }
    }

    // Load the scene with the scene index
    public override void LoadGameScene(int scene)
    {
        SceneManager.LoadScene(scene);

        Pause();
    }

    /// <summary>
    /// Checks if the elemnts in the menu are active
    /// </summary>
    /// <returns></returns>
    public bool CheckMenuElements()
    {
        if (settingsMenu.activeInHierarchy || controlsMenu.activeInHierarchy || confirmQuit.activeInHierarchy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

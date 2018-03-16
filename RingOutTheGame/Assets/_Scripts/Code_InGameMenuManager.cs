using UnityEngine.SceneManagement;
using UnityEngine;

public class Code_InGameMenuManager : Code_MenuManager {

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("StartButton")) {
            Pause();

            ToggleMenus(0);
        }
    }

    // Load the scene with the scene index
    public override void LoadGameScene(int scene)
    {
        SceneManager.LoadScene(scene);

        Pause();
    }
}

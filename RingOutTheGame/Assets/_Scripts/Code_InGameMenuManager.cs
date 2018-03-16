using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_InGameMenuManager : Code_MenuManager {

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("StartButton")) {
            Pause();

            ToggleMenus(0);
        }
    }
}

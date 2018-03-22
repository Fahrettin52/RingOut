using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Code_InGameMenuManager : Code_MenuManager
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("StartButton") && !playerSelectMenu.activeSelf)
        {
            if (!CheckMenuElements())
            {
                Pause();
                ToggleMenus(0);
            }
        }
    }

    /// <summary>
    /// Checks if the elements in the menu are active
    /// </summary>
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

    public override void InitializeMenu()
    {
        foreach (Transform item in playerSelectMenu.transform)
        {
            if (item.GetComponent<Button>() != null)
            {
                playerSelectMenuButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in mainMenuBorder.transform)
        {
            if (item.GetComponent<Button>() != null)
            {
                mainMenuButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in settingsMenu.transform)
        {
            if (item.GetComponent<Button>() != null || item.GetComponent<Toggle>() != null)
            {
                settingsButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in controlsMenu.transform)
        {
            if (item.GetComponent<Button>() != null)
            {
                controlButtons.Add(item.gameObject);
            }
        }

        foreach (Transform item in confirmQuit.transform)
        {
            if (item.GetComponent<Button>() != null)
            {
                confirmQuitButtons.Add(item.gameObject);
            }
        }

        PickFirstButton(playerSelectMenuButtons, !playerSelectMenu.activeSelf, 0);
    }

    public override void LoadGameScene(int scene) {        
        SceneManager.LoadScene(scene);
        Pause();
    }
}

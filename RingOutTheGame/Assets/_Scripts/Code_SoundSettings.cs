using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Code_SoundSettings : MonoBehaviour {

    public List<Toggle> soundToggles = new List<Toggle>();
    public List<bool> soundBools = new List<bool>();
    public Transform SettingsMenu;
    public Scene curScene;
    public static Code_SoundSettings instance;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        curScene = SceneManager.GetActiveScene();
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            // TODO get the values in the existing instance
            GetValuesFromInstance();

            // TODO DEstroy the instance once all values were transfered
            Destroy(instance.gameObject);

            // Set this gameobject as the new instance value
            instance = this;
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (curScene == scene) {
            FillSettingMenu();            
            SetValues();
            GetValues();
        }
    }

    // Sets the values of the souboundbools list
    public void SetValues() {
        for (int i = 0; i < soundToggles.Count; i++) {
            soundBools[i] = soundToggles[i].isOn;
        }
    }

    public void GetValuesFromInstance() {
        soundBools = instance.soundBools;
        GetValues();   
    }

    // Set the soundToggles list to the soundbools list
    public void GetValues() {
        int i = 0;
        foreach (Transform item in SettingsMenu.transform) {
            if (item.GetComponent<Toggle>() != null) {
                item.GetComponent<Toggle>().isOn = soundBools[i];
                i++;
            }
        }
    }

    // Fill the soundToggles list with the toggles of the settingsmenu
    public void FillSettingMenu() {
        foreach (Transform item in SettingsMenu.transform) {
            if (item.GetComponent<Toggle>() != null) {
                soundToggles.Add(item.GetComponent<Toggle>());
            }
        }

        soundBools = new List<bool>(new bool[soundToggles.Count]);
    }
}

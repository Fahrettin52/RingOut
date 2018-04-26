using UnityEngine;
using UnityEngine.UI;

public class Code_SoundManager : MonoBehaviour {

    public AudioSource[] musicAudioSource; // Audiosource that controls the music sounds
    public AudioSource[] sFXAudioSource; // Audiosource that controls the sfx sounds
    public AudioSource[] characterAudioSource; // Audiosource that controls the character sounds

    public AudioClip[] musicAudioClip; // The music sounds
    public AudioClip[] sFXAudioClip; // The SFX sounds
    public AudioClip[] characterAudioClip; // The character sounds

    public Toggle[] volumeChecks; // An array that contains all the music toggles in the settings menu

    // Use this for initialization
    void Start() {
        // Store each music audioclip in the music audiosource
        for (int i = 0; i < musicAudioSource.Length; i++) {
            musicAudioSource[i].clip = musicAudioClip[i];
        }
        // Store each sfx audioclip in the sfx audiosource
        for (int i = 0; i < sFXAudioSource.Length; i++) {
            sFXAudioSource[i].clip = sFXAudioClip[i];
        }
        // Store each character audioclip in the character audiosource
        for (int i = 0; i < characterAudioSource.Length; i++) {
            characterAudioSource[i].clip = characterAudioClip[i];
        }
        // Call the methode that plays the background music
        PlayMainMenuMusic();
    }

    /// <summary>
    /// Play the mainmenu music
    /// </summary>
    public void PlayMainMenuMusic() {
        musicAudioSource[0].Play();
    }

    /// <summary>
    /// Play the in game ambient music
    /// </summary>
    public void PlayGameMusic() {
        if (volumeChecks[1].isOn) {
            musicAudioSource[1].Play();
        }
    }

    /// <summary>
    /// Play the hover over button sound
    /// </summary>
    public void PlayButtonHover() {
        sFXAudioSource[0].Play();
    }

    /// <summary>
    /// Play the confirmation sound
    /// </summary>
    public void PlayUIConfirmation() {
        sFXAudioSource[1].Play();
    }

    /// <summary>
    /// Play the checkmark sound
    /// </summary>
    public void PlayToggleSetting() {
        sFXAudioSource[2].Play();
    }

    /// <summary>
    /// Mute or unmute the the chosen audiosource
    /// </summary>
    /// <param name="index"></param>
    public void ToggleSelectedSound(int index) {
        // Check if the mastervolumes boolean is true 
        if (volumeChecks[0].isOn) {
            switch (index) {
                case 0:
                    // Toggle the music sound
                    foreach (AudioSource item in musicAudioSource) {
                        if (item != musicAudioSource[1]) {
                            item.mute = !volumeChecks[1].isOn;
                        }
                    }
                    break;
                case 1:
                    // Toggle the sfx sound
                    foreach (AudioSource item in sFXAudioSource) {
                        item.mute = !volumeChecks[2].isOn;
                    }
                    break;
                case 2:
                    // Toggle the character sound
                    foreach (AudioSource item in characterAudioSource) {
                        item.mute = !volumeChecks[3].isOn;
                    }
                    break;
                // If non of the above methodes are called break the code
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Turn off all the sounds in game
    /// </summary>
    public void TurnOffAllSounds() {
        // Check if the mastervolumes boolean is true 
        if (!volumeChecks[0].isOn) {
            // Mute all the music sounds
            for (int i = 0; i < musicAudioSource.Length; i++) {
                musicAudioSource[i].mute = true;
            }
            // Mute all the sfx sounds
            for (int i = 0; i < sFXAudioSource.Length; i++) {
                sFXAudioSource[i].mute = true;
            }
            // Mute all the character sounds
            for (int i = 0; i < characterAudioSource.Length; i++) {
                characterAudioSource[i].mute = true;
            }
        }
        // If the mastervolumes boolean is not rue
        else { 
            // Mute or unmute the music audiosource depending on the current state of the corresponding volumecheck item
            for (int i = 0; i < musicAudioSource.Length; i++) {
                if (musicAudioSource[i] != musicAudioSource[1]) {
                    musicAudioSource[i].mute = !volumeChecks[1].isOn;
                }
            }
            // Mute or unmute the sfx audiosource depending on the current state of the corresponding volumecheck item
            for (int i = 0; i < sFXAudioSource.Length; i++) {
                sFXAudioSource[i].mute = !volumeChecks[2].isOn;
            }
            // Mute or unmute the character audiosource depending on the current state of the corresponding volumecheck item
            for (int i = 0; i < characterAudioSource.Length; i++) {
                characterAudioSource[i].mute = !volumeChecks[3].isOn;
            }
        }
    }
}

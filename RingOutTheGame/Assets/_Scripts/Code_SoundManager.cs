using UnityEngine;

public class Code_SoundManager : MonoBehaviour
{

    public AudioSource[] musicAudioSource;
    public AudioSource[] sFXAudioSource;
    public AudioSource[] ambientAudioSource;
    public AudioSource[] characterAudioSource;

    public AudioClip[] musicAudioClip;
    public AudioClip[] sFXAudioClip;
    public AudioClip[] ambientAudioClip;
    public AudioClip[] characterAudioClip;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < musicAudioSource.Length; i++)
        {
            musicAudioSource[i].clip = musicAudioClip[i];
        }

        for (int i = 0; i < sFXAudioSource.Length; i++)
        {
            sFXAudioSource[i].clip = sFXAudioClip[i];
        }

        for (int i = 0; i < ambientAudioSource.Length; i++)
        {
            ambientAudioSource[i].clip = ambientAudioClip[i];
        }

        for (int i = 0; i < characterAudioSource.Length; i++)
        {
            characterAudioSource[i].clip = characterAudioClip[i];
        }

        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        musicAudioSource[0].Play();
    }

    public void PlayButtonHover()
    {
        sFXAudioSource[0].Play();
    }

    public void PlayUIConfirmation()
    {
        sFXAudioSource[1].Play();
    }

    public void PlayToggleSetting()
    {
        sFXAudioSource[2].Play();
    }

    public void PlayAmbientMusic()
    {
        ambientAudioSource[0].Play();
    }

    public void TurnOffSelectedSound(int index)
    {
        switch (index)
        {
            case 0:
                foreach (AudioSource item in musicAudioSource)
                {
                    item.mute = !item.mute;
                }
                break;

            case 1:
                foreach (AudioSource item in sFXAudioSource)
                {
                    item.mute = !item.mute;
                }
                break;

            case 2:
                foreach (AudioSource item in ambientAudioSource)
                {
                    item.mute = !item.mute;
                }
                break;

            case 3:
                foreach (AudioSource item in characterAudioSource)
                {
                    item.mute = !item.mute;
                }
                break;

            default:
                break;
        }
    }

    public void TurnOffAllSounds()
    {
        for (int i = 0; i < musicAudioSource.Length; i++)
        {
            musicAudioSource[i].mute = !musicAudioSource[i].mute;
        }

        for (int i = 0; i < sFXAudioSource.Length; i++)
        {
            sFXAudioSource[i].mute = !sFXAudioSource[i].mute;
        }

        for (int i = 0; i < ambientAudioSource.Length; i++)
        {
            ambientAudioSource[i].mute = !ambientAudioSource[i].mute;
        }

        for (int i = 0; i < characterAudioSource.Length; i++)
        {
            characterAudioSource[i].mute = !characterAudioSource[i].mute;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource soundEffectSource;
    public AudioSource musicSource;

    //Effect sounds
    public AudioClip extraLifeSound;
    public AudioClip getCoinSound;
    public AudioClip breakBlockSound;
    public AudioClip bumpSound;
    public AudioClip smallJumpSound;
    public AudioClip bigJumpSound;
    public AudioClip enterPipeSound;
    public AudioClip powerupAppearsSound;
    public AudioClip powerupSound;
    public AudioClip crushEnemySound;
    public AudioClip kickEnemySound;
    public AudioClip fireballSound;
    public AudioClip marioDiesSound;
    public AudioClip gameOverSound;
    public AudioClip flagpoleSound;
    public AudioClip stageClearSound;

    //Music
    public AudioClip groundTheme;
    public AudioClip groundAceleratedTheme;
    public AudioClip undergroundTheme;
    public AudioClip undergroundAceleratedTheme;
    public AudioClip superPowerTheme;

    public bool isLevelTheme = false;

    public AudioClip currentTheme;
    public void PlaySoundEffect(AudioClip soundClip)
    {
        soundEffectSource.Stop();

        soundEffectSource.clip = soundClip;
        soundEffectSource.Play();
    }

    public void StopSoundEffect()
    {
        soundEffectSource.Stop();
    }

    public void PlayMusic (AudioClip music)
    {
        musicSource.Stop();

        musicSource.clip = music;
        currentTheme = music;
        musicSource.Play();
        if (isLevelTheme)
        {
            musicSource.loop = true;
            isLevelTheme = false;
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}

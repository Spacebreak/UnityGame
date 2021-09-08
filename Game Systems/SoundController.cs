using System.Collections;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource[] audioSounds;   //The audio clips associated with the object this script attached to.
    private bool[] audioCooldowns;      //Tracks if the default clip is currently on cooldown or not.

    public AudioSource[] hurtSounds;
    private bool[] hurtSoundCooldowns;  //Tracks if the hurt clip is currently on cooldown or not.



    //AudioClips and AudioCooldowns have a 1:1 relationship, cooldown is tracked.
    public void PlayAudioClip(int audioClipIndex)
    {
        if(audioCooldowns[audioClipIndex] == false)
        {
            audioSounds[audioClipIndex].Play();
        }
    }

    public void PlayAudioClip(AudioCollection audioType, int audioClipIndex, float timeToDelay)
    {
        if(audioType == AudioCollection.Jump)
        {
            if (audioCooldowns[audioClipIndex] == false)
            {
                audioSounds[audioClipIndex].Play();
                StartCoroutine(DelayClipFromRepeating(audioType, audioClipIndex, timeToDelay));
            }
        } 
        else if (audioType == AudioCollection.Hurt)
        {
            if (hurtSoundCooldowns[audioClipIndex] == false)
            {
                hurtSounds[audioClipIndex].Play();
                StartCoroutine(DelayClipFromRepeating(audioType, audioClipIndex, timeToDelay));
            }
        }
        else if (audioType == AudioCollection.Collectable)
        {
            if (hurtSoundCooldowns[audioClipIndex] == false)
            {
                hurtSounds[audioClipIndex].Play();
                StartCoroutine(DelayClipFromRepeating(audioType, audioClipIndex, timeToDelay));
            }
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        audioCooldowns = new bool[audioSounds.Length];
        hurtSoundCooldowns = new bool[hurtSounds.Length];
    }

    //Places the clip on cooldown. Waits a small duration, and then takes the clip off cooldown.
    private IEnumerator DelayClipFromRepeating(AudioCollection audioCollection, int audioClipIndex, float timeToDelay)
    {
        if(audioCollection == AudioCollection.Jump)
        {
            audioCooldowns[audioClipIndex] = true;
        }
        else if(audioCollection == AudioCollection.Hurt)
        {
            hurtSoundCooldowns[audioClipIndex] = true;
        }

        yield return new WaitForSeconds(timeToDelay);

        if (audioCollection == AudioCollection.Jump)
        {
            audioCooldowns[audioClipIndex] = false;
        }
        else if (audioCollection == AudioCollection.Hurt)
        {
            hurtSoundCooldowns[audioClipIndex] = false;
        }
    }

    public enum AudioCollection 
    { 
        Default, 
        Jump, 
        Hurt,
        Collectable
    }
}

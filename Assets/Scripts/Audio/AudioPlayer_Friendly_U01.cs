using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer_Friendly_U01 : AudioPlayerBase
{
    public override void Init()
    {
        instance = this;
        audioPlayers = new AudioSource[audioChannels];
        AudioManager.AudioVolumes volumes = AudioManager.instance.Volumes;

        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            audioPlayers[i] = this.gameObject.AddComponent<AudioSource>();
            audioPlayers[i].playOnAwake = false;
            audioPlayers[i].volume = volumes.Effect;
        }

    }

    public void PlayAudio(EAudioType_Friendly_U01 _audioType)
    {
        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            int loopIndex = (i + channelIndex) % audioPlayers.Length;

            // if (audioPlayers[loopIndex].isPlaying) continue;

            channelIndex = loopIndex;
            audioPlayers[loopIndex].clip = audioClips[(int)_audioType];
            Debug.Log("Friendly AudioPlayers.Play Start");
            audioPlayers[loopIndex].Play();
            Debug.Log("Friendly AudioPlayers.Play End");
            break;
        }
    }
    
    
    /*
    public void PlayAttackAudio(EFriendlyAudioType _audioType)
    {
        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            int loopIndex = (i + channelIndex) % audioPlayers.Length;

            if (audioPlayers[loopIndex].isPlaying) continue;

            channelIndex = loopIndex;
            audioPlayers[loopIndex].clip = audioClips[(int)_audioType];
            audioPlayers[loopIndex].Play();
            break;
        }
    }
    */


    
    [Header("#FriendlyAudio")]
    [SerializeField] private AudioClip[] audioClips;
    // [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
    public static AudioPlayer_Friendly_U01 instance;
    
    public enum EAudioType_Friendly_U01 { NONE = -1, PRODUCE, SELECT_01, SELECT_02, SELECT_03, ORDER_01, ORDER_02, ORDER_03, ORDER_04, ATTACK, LENGTH } 
}

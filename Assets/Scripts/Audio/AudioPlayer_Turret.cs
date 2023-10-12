using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer_Turret : MonoBehaviour
{
    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Init()
    {
        audioPlayers = new AudioSource[audioChannels];

        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            audioPlayers[i] = this.gameObject.AddComponent<AudioSource>();
            audioPlayers[i].playOnAwake = false;
            audioPlayers[i].volume = audioVolume;
        }

    }

    public void PlayAudio(EAudioType_Turret _audioType)
    {
        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            int loopIndex = (i + channelIndex) % audioPlayers.Length;

            channelIndex = loopIndex;
            audioPlayers[loopIndex].clip = audioClips[(int)_audioType];
            audioPlayers[loopIndex].Play();
            break;
        }
    }
    
    
    [Header("#UIAudio")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
    public static AudioPlayer_Turret instance;
    public enum EAudioType_Turret { NONE = -1, ATTACK, LENGTH } 
}
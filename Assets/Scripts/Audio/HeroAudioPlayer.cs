using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAudioPlayer : MonoBehaviour
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

    public void PlayAttackAudio(EHeroAudioType _audioType)
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

    public static HeroAudioPlayer instance;
    public enum EHeroAudioType { NONE = -1, ATTACK, MOVE, SELECT, LENGTH } 
    
    [Header("#HeroAudio")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
}

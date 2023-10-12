using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer_Hero : MonoBehaviour
{
    
    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Init()
    {
        audioPlayers = new AudioSource[audioChannels];
        AudioManager.AudioVolumes volumes = AudioManager.instance.Volumes;

        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            audioPlayers[i] = this.gameObject.AddComponent<AudioSource>();
            audioPlayers[i].playOnAwake = false;
            audioPlayers[i].volume = volumes.Effect;
        }

    }

    public void PlayAudio(EAudioType_Hero _audioType)
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
    
    [Header("#HeroAudio")]
    [SerializeField] private AudioClip[] audioClips;
    // [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
    public static AudioPlayer_Hero instance;
    public enum EAudioType_Hero { NONE = -1, PRODUCE, SELECT_01, SELECT_02, SELECT_03, ORDER_01, ORDER_02, ORDER_03, ORDER_04, ATTACK, LENGTH } 
}

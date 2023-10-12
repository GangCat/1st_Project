using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer_Struct : MonoBehaviour
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

    public void PlayAudio(EAudioType_Struct _audioType)
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
    
    
    [Header("#StructAudio")]
    [SerializeField] private AudioClip[] audioClips;
    // [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
    public static AudioPlayer_Struct instance;
    public enum EAudioType_Struct { NONE = -1, SELECT, BUILD, ATTACK, LOAD, UNLOAD, PRODUCE, LENGTH } 
}

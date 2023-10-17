using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer_Misc : AudioPlayerBase
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

    public void PlayAudio(EAudioType_Misc _audioType)
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
    
    
    [Header("#MiscAudio")]
    [SerializeField] private AudioClip[] audioClips;
    // [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
    public static AudioPlayer_Misc instance;
    public enum EAudioType_Misc { NONE = -1, NUCLEAR_EXPLOSION,  LENGTH } 
}

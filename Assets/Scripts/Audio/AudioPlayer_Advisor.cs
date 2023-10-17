using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer_Advisor : AudioPlayerBase
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

    public void PlayAudio(EAudioType_Advisor _audioType)
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
    
    [Header("#AdvisorAudio")]
    [SerializeField] private AudioClip[] audioClips;
    
    
    // [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
    public static AudioPlayer_Advisor instance;
    public enum EAudioType_Advisor { NONE = -1, ENERGY, CORE, RESEARCH, UPGRADE, CONST_COMPLETE,CONST_CANCEL, PAUSE, RESUME, NUCLEAR_READY, NUCLEAR_LAUNCH, UNDERATTACK_01, UNDERATTACK_02, UNDERATTACK_03, LENGTH } 
}

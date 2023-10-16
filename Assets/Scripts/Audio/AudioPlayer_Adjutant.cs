using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer_Adjutant : MonoBehaviour
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

    public void PlayAudio(EAudioType_Adjutant _audioType)
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
    
    public static AudioPlayer_Adjutant instance;
    public enum EAudioType_Adjutant { NONE = -1, ENERGY, CORE, RESEARCH, UPGRADE, CONST_COMPLETE,CONST_CANCEL, PAUSE, RESUME, NUCLEAR_READY, NUCLEAR_LAUNCH, UNDERATTACK_01, UNDERATTACK_02, UNDERATTACK_03, LENGTH } 
}

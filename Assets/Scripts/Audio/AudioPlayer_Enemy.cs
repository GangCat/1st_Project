using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class AudioPlayer_Enemy : AudioPlayerBase
{
    public override void Init()
    {
        instance = this;
        // 효과음 플레이어 초기화
        // GameObject sfxObject = new GameObject("EnemySfxPlayer");
        // sfxObject.transform.parent = transform;             // AudioManager 자식으로 등록
        audioPlayers = new AudioSource[audioChannels];
        AudioManager.AudioVolumes volumes = AudioManager.instance.Volumes;

        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            audioPlayers[i] = this.gameObject.AddComponent<AudioSource>();
            audioPlayers[i].playOnAwake = false;
            audioPlayers[i].volume = volumes.Effect;
        }

    }

    public void PlayAudio(EAudioType_Enemy _audioType)
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

    public static AudioPlayer_Enemy instance;

    
    [Header("#EnemyAudio")]
    [SerializeField] private AudioClip[] audioClips;
    // [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
    public enum EAudioType_Enemy { NONE = -1, SELECT_01, SELECT_02, SELECT_03, ATTACK, DEAD, LENGTH } 
}

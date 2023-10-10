using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAudioPlayer : MonoBehaviour
{
    
    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Init()
    {
        /*
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;             // AudioManager 자식으로 등록
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // bgmPlayer 초기화
        bgmPlayer.playOnAwake = false;                      // 배경음악 자동재생 false
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        */


        // 효과음 플레이어 초기화
        // GameObject sfxObject = new GameObject("FriendlySfxPlayer");
        // sfxObject.transform.parent = transform;             // AudioManager 자식으로 등록
        audioPlayers = new AudioSource[audioChannels];

        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            audioPlayers[i] = this.gameObject.AddComponent<AudioSource>();
            audioPlayers[i].playOnAwake = false;
            audioPlayers[i].volume = audioVolume;
        }

    }

    public void PlayAttackAudio(EFriendlyAudioType _audioType)
    {
        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            int loopIndex = (i + channelIndex) % audioPlayers.Length;

            // if (audioPlayers[loopIndex].isPlaying) continue;

            channelIndex = loopIndex;
            audioPlayers[loopIndex].clip = audioClips[(int)_audioType];
            Debug.Log("HERO AudioPlayers.Play Start");
            audioPlayers[loopIndex].Play();
            Debug.Log("HERO AudioPlayers.Play End");
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


    public static FriendlyAudioPlayer instance;
    public enum EFriendlyAudioType { NONE = -1, SHOT, MOVE, LENGTH } 
    
    [Header("#FriendlyAudio")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
}

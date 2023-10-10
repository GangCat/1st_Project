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
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;             // AudioManager �ڽ����� ���
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // bgmPlayer �ʱ�ȭ
        bgmPlayer.playOnAwake = false;                      // ������� �ڵ���� false
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        */


        // ȿ���� �÷��̾� �ʱ�ȭ
        // GameObject sfxObject = new GameObject("FriendlySfxPlayer");
        // sfxObject.transform.parent = transform;             // AudioManager �ڽ����� ���
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
    public enum EFriendlyAudioType { NONE = -1, ATTACK, MOVE, LENGTH } 
    
    [Header("#FriendlyAudio")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
    
}

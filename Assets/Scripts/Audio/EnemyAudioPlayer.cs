using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class EnemyAudioPlayer : MonoBehaviour
{
    
    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Init()
    {

        // ȿ���� �÷��̾� �ʱ�ȭ
        // GameObject sfxObject = new GameObject("EnemySfxPlayer");
        // sfxObject.transform.parent = transform;             // AudioManager �ڽ����� ���
        audioPlayers = new AudioSource[audioChannels];

        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            audioPlayers[i] = this.gameObject.AddComponent<AudioSource>();
            audioPlayers[i].playOnAwake = false;
            audioPlayers[i].volume = audioVolume;
        }

    }

    public void PlayAttackAudio(EEnemyAudioType _audioType)
    {
        Debug.Log("PlayAttackAudio called with type: " + _audioType);
        Debug.Log("audioPlayers.Length: " + audioPlayers.Length); // �迭 ���� Ȯ��

        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            Debug.Log("PlayAttackAudio called 1");
            int loopIndex = (i + channelIndex) % audioPlayers.Length;

            Debug.Log("PlayAttackAudio called 2");
            if (audioPlayers[loopIndex].isPlaying) continue;

            Debug.Log("Trying to play audio at index: " + loopIndex);
            channelIndex = loopIndex;
            audioPlayers[loopIndex].clip = audioClips[(int)_audioType];
            Debug.Log("Enemy AudioPlayers.Play Start");
            audioPlayers[loopIndex].Play();
            Debug.Log("EnemyAudioPlayers.Play End");
            break;
        }
    }

    public static EnemyAudioPlayer instance;
    public enum EEnemyAudioType { NONE = -1, SHOT, MOVE, LENGTH } 
    
    [Header("#FriendlyAudio")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float audioVolume;
    [SerializeField] private int audioChannels; 
    private AudioSource[] audioPlayers;

    private int channelIndex;
}

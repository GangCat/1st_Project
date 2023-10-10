using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.HighDefinition;

public class AudioManager : MonoBehaviour
{
   
    private void Awake()
    {
        instance = this;
    }

    public void PlayAttackAudio(EObjectType _objectType)
    {
        switch (_objectType)
        {
            case EObjectType.UNIT:
                Debug.Log("Unit Attack Audio!");
                FriendlyAudioPlayer.instance.PlayAttackAudio(FriendlyAudioPlayer.EFriendlyAudioType.SHOT);
                break;
            case EObjectType.UNIT_HERO:
                Debug.Log("Hero Attack Audio!");
                FriendlyAudioPlayer.instance.PlayAttackAudio(FriendlyAudioPlayer.EFriendlyAudioType.SHOT);
                break;
            case EObjectType.ENEMY_UNIT:
                Debug.Log("Enemy Attack Audio!");
                EnemyAudioPlayer.instance.PlayAttackAudio(EnemyAudioPlayer.EEnemyAudioType.SHOT);
                break;
            default:
                break;
        }
            
    }
    

    public static AudioManager instance;
    
    /*
    [Header("#BGM")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private float bgmVolume;
    private AudioSource bgmPlayer;
    */
    
}

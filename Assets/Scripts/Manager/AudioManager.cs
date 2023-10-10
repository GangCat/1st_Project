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
                FriendlyAudioPlayer.instance.PlayAttackAudio(FriendlyAudioPlayer.EFriendlyAudioType.ATTACK);
                break;
            case EObjectType.UNIT_HERO:
                HeroAudioPlayer.instance.PlayAttackAudio(HeroAudioPlayer.EHeroAudioType.ATTACK);
                break;
            case EObjectType.ENEMY_UNIT:
                EnemyAudioPlayer.instance.PlayAttackAudio(EnemyAudioPlayer.EEnemyAudioType.ATTACK);
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

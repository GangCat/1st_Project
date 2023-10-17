using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public void Init()
    {
        // ΩÃ±€≈Ê ∆–≈œ √ ±‚»≠
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("AudioManager initialized.");
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        foreach (AudioPlayerBase APB in GetComponentsInChildren<AudioPlayerBase>())
            APB.Init();
    }


    public void PlayAudio_Attack(EObjectType _objectType)
    {
        switch (_objectType)
        {
            case EObjectType.UNIT_01:
                Debug.Log("UNIT_01 Audio!");
                AudioPlayer_Friendly_U01.instance.PlayAudio(AudioPlayer_Friendly_U01.EAudioType_Friendly_U01.ATTACK);
                break;
            case EObjectType.UNIT_02:
                Debug.Log("UNIT_01 Audio!");
                AudioPlayer_Friendly_U02.instance.PlayAudio(AudioPlayer_Friendly_U02.EAudioType_Friendly_U02.ATTACK);
                break;
            case EObjectType.UNIT_HERO:
                Debug.Log("UNIT_HERO Audio!");
                AudioPlayer_Hero.instance.PlayAudio(AudioPlayer_Hero.EAudioType_Hero.ATTACK);
                break;
            case EObjectType.ENEMY_UNIT:
                Debug.Log("ENEMY_UNIT Audio!");
                AudioPlayer_Enemy.instance.PlayAudio(AudioPlayer_Enemy.EAudioType_Enemy.ATTACK);
                break;
            case EObjectType.TURRET:
                Debug.Log("TURRET Audio!");
                AudioPlayer_Turret.instance.PlayAudio(AudioPlayer_Turret.EAudioType_Turret.ATTACK);
                break;
            default:
                break;
        }
    }
    
    public void PlayAudio_Order(EObjectType _objectType)
    {
        int Idx = UnityEngine.Random.Range(0, 4); // Generates 0, 1, 2, or 3

        switch (_objectType)
        {
            case EObjectType.UNIT_HERO:
                AudioPlayer_Hero.EAudioType_Hero audioTypeHero = 
                    (AudioPlayer_Hero.EAudioType_Hero)((int)AudioPlayer_Hero.EAudioType_Hero.ORDER_01 + Idx);
                AudioPlayer_Hero.instance.PlayAudio(audioTypeHero);
                break;
            case EObjectType.UNIT_01:
                AudioPlayer_Friendly_U01.EAudioType_Friendly_U01 audioTypeU01 = 
                    (AudioPlayer_Friendly_U01.EAudioType_Friendly_U01)((int)AudioPlayer_Friendly_U01.EAudioType_Friendly_U01.ORDER_01 + Idx);
                AudioPlayer_Friendly_U01.instance.PlayAudio(audioTypeU01);
                break;
            case EObjectType.UNIT_02:
                AudioPlayer_Friendly_U02.EAudioType_Friendly_U02 audioTypeU02 = 
                    (AudioPlayer_Friendly_U02.EAudioType_Friendly_U02)((int)AudioPlayer_Friendly_U02.EAudioType_Friendly_U02.ORDER_01 + Idx);
                AudioPlayer_Friendly_U02.instance.PlayAudio(audioTypeU02);
                break;
            default:
                break;
        }
    }

    public void PlayAudio_Select(EObjectType _objectType)
    {
        int Idx = UnityEngine.Random.Range(0, 3); // Generates 0, 1, 2

        switch (_objectType)
        {
            case EObjectType.UNIT_HERO:
                AudioPlayer_Hero.EAudioType_Hero audioTypeHero = 
                    (AudioPlayer_Hero.EAudioType_Hero)((int)AudioPlayer_Hero.EAudioType_Hero.SELECT_01 + Idx);
                AudioPlayer_Hero.instance.PlayAudio(audioTypeHero);
                break;
            case EObjectType.UNIT_01:
                AudioPlayer_Friendly_U01.EAudioType_Friendly_U01 audioTypeU01 = 
                    (AudioPlayer_Friendly_U01.EAudioType_Friendly_U01)((int)AudioPlayer_Friendly_U01.EAudioType_Friendly_U01.SELECT_01 + Idx);
                AudioPlayer_Friendly_U01.instance.PlayAudio(audioTypeU01);
                break;
            case EObjectType.UNIT_02:
                AudioPlayer_Friendly_U02.EAudioType_Friendly_U02 audioTypeU02 = 
                    (AudioPlayer_Friendly_U02.EAudioType_Friendly_U02)((int)AudioPlayer_Friendly_U02.EAudioType_Friendly_U02.SELECT_01 + Idx);
                AudioPlayer_Friendly_U02.instance.PlayAudio(audioTypeU02);
                break;
            case EObjectType.MAIN_BASE:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.MAIN_BASE);
                break;
            case EObjectType.NUCLEAR:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.NUCLEAR);
                break;
            case EObjectType.TURRET:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.TURRET);
                break;
            case EObjectType.BARRACK:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.BARRACK);
                break;
            case EObjectType.BUNKER:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.BUNKER);
                break;
            case EObjectType.WALL:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.WALL);
                break;
            default:
                break;
        }
    }
    
    
    public void PlayAudio_Advisor(EAudioType_Advisor _audioType)
    {
        int Idx = UnityEngine.Random.Range(0, 3); // Generates 0, 1, 2, or 3

        switch (_audioType)
        {
            case EAudioType_Advisor.UNDERATTACK:
                AudioPlayer_Advisor.EAudioType_Advisor audioTypeAdvisor = 
                    (AudioPlayer_Advisor.EAudioType_Advisor)((int)AudioPlayer_Advisor.EAudioType_Advisor.UNDERATTACK_01 + Idx);
                AudioPlayer_Advisor.instance.PlayAudio(audioTypeAdvisor);
                break;
            case EAudioType_Advisor.ENERGY:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.ENERGY);
                break;
            case EAudioType_Advisor.CORE:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.CORE);
                break;
            case EAudioType_Advisor.RESEARCH:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.RESEARCH);
                break;
            case EAudioType_Advisor.UPGRADE:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.UPGRADE);
                break;
            case EAudioType_Advisor.CONST_COMPLETE:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.CONST_COMPLETE);
                break;
            case EAudioType_Advisor.CONST_CANCEL:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.CONST_CANCEL);
                break;
            case EAudioType_Advisor.PAUSE:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.PAUSE);
                break;
            case EAudioType_Advisor.RESUME:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.RESUME);
                break;
            case EAudioType_Advisor.NUCLEAR_READY:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.NUCLEAR_READY);
                break;
            case EAudioType_Advisor.NUCLEAR_LAUNCH:
                AudioPlayer_Advisor.instance.PlayAudio(AudioPlayer_Advisor.EAudioType_Advisor.NUCLEAR_LAUNCH);
                break;
            default:
                break;
        }
    }
    
    
    public void PlayAudio_Build(EObjectType _objectType)
    {
        AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.BUILD);
    }
    
    public void PlayAudio_Destroy2(EObjectType _objectType)
    {
        AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.DESTROY);
    }
    
    public void PlayAudio_Destroy(EObjectType _objectType)
    {
        switch (_objectType)
        {
            case EObjectType.UNIT_01:
                AudioPlayer_Friendly_U01.instance.PlayAudio(AudioPlayer_Friendly_U01.EAudioType_Friendly_U01.DEAD);
                break;
            case EObjectType.UNIT_02:
                AudioPlayer_Friendly_U02.instance.PlayAudio(AudioPlayer_Friendly_U02.EAudioType_Friendly_U02.DEAD);
                break;
            case EObjectType.UNIT_HERO:
                AudioPlayer_Hero.instance.PlayAudio(AudioPlayer_Hero.EAudioType_Hero.DEAD);
                break;
            case EObjectType.ENEMY_UNIT:
                AudioPlayer_Enemy.instance.PlayAudio(AudioPlayer_Enemy.EAudioType_Enemy.DEAD);
                break;
            case EObjectType.MAIN_BASE:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.DESTROY);
                break;
            case EObjectType.NUCLEAR:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.DESTROY);
                break;
            case EObjectType.TURRET:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.DESTROY);
                break;
            case EObjectType.BARRACK:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.DESTROY);
                break;
            case EObjectType.BUNKER:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.DESTROY);
                break;
            case EObjectType.WALL:
                AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.DESTROY);
                break;
            default:
                break;
        }
    }
    
    public void PlayAudio_Misc(EAudioType_Misc _audioType)
    {
        int Idx = UnityEngine.Random.Range(0, 3); // Generates 0, 1, 2, or 3

        switch (_audioType)
        {
            case EAudioType_Misc.NUCLEAR_EXPLOSION:
                AudioPlayer_Misc.instance.PlayAudio(AudioPlayer_Misc.EAudioType_Misc.NUCLEAR_EXPLOSION);
                break;
            default:
                break;
        }
    }
    
        
    public void PlayAudio_UI(EObjectType _objectType)
    {
        AudioPlayer_UI.instance.PlayAudio(AudioPlayer_UI.EAudioType_UI.CLICK);
    }

    public struct AudioVolumes
    {
        public float Main;
        public float BGM;
        public float Effect;
    }

    public AudioVolumes Volumes
    {
        get
        {
            return new AudioVolumes
            {
                Main = audioVolume_Master,
                BGM = audioVolume_BGM,
                Effect = audioVolume_Effect
            };
        }
    }
    
    
    public static AudioManager instance;
    
    [SerializeField] private float audioVolume_Master;
    [SerializeField] private float audioVolume_BGM;
    [SerializeField] private float audioVolume_Effect;
    
    /*
    [Header("#BGM")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private float bgmVolume;
    private AudioSource bgmPlayer;
    */
    
}

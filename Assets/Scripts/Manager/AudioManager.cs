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
    }


    public void PlayAudio_Attack(EObjectType _objectType)
    {
        switch (_objectType)
        {
            case EObjectType.UNIT_01:
                Debug.Log("UNIT_01 Audio!");
                AudioPlayer_Friendly_U01.instance.PlayAudio(AudioPlayer_Friendly_U01.EAudioType_Friendly_U01.ATTACK);
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
            default:
                break;
        }
    }

    
    public void PlayAudio_Build(EObjectType _objectType)
    {
        AudioPlayer_Struct.instance.PlayAudio(AudioPlayer_Struct.EAudioType_Struct.BUILD);

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    public int bgmChannels;
    AudioSource[] bgmPlayers;
    int bgmChannelIndex;

    //[Header("#BGM")]
    //public AudioClip bgmClip;
    //public float bgmVolume;
    //public int bgmChannel;
    //AudioSource bgmPlayer;
    //int bgmChannelIndex;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels;
    AudioSource[] sfxPlayers;
    int sfxChannelIndex;
    public enum BGM
    {
        InGame, Lobby
    }
    public enum SFX 
    { 
        EnemyDeath, EnemyIdle, EnemyLockOn, EnemyShoot, 
        PlayerColorSwap, PlayerDeath,  PlayerGuard, PlayerGunOn, 
        PlayerJam, PlayerReload, PlayerRun, PlayerShoot, PlayerswordOn, PlayerWalk
    }

    void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[bgmChannels];

        for (int index = 0; index < bgmPlayers.Length; index++)
        {
            bgmPlayers[index] = bgmObject.AddComponent<AudioSource>();
            bgmPlayers[index].playOnAwake = false;
            bgmPlayers[index].loop = true;
            bgmPlayers[index].volume = bgmVolume;
            bgmPlayers[index].clip = bgmClips[index];
        }

        //GameObject bgmObject = new GameObject("BgmPlayer");
        //bgmObject.transform.parent = transform;
        //bgmPlayer = bgmObject.AddComponent<AudioSource>();
        //bgmPlayer.playOnAwake = false;
        //bgmPlayer.volume = bgmVolume;
        //bgmPlayer.clip = bgmClip;



        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }
    public void PlayBgm(BGM bgm/*bool isPlay*/)
    {
        
        for (int index = 0; index < bgmPlayers.Length; index++)
        {
            int loopIndex = (index + bgmChannelIndex) % bgmPlayers.Length;

            if (bgmPlayers[loopIndex].isPlaying)
            {
                continue;
            }
            int ranIndex = 0;
            //if (bgm == BGM.Lobby)
            //{
            //    ranIndex = Random.Range(0, 2);
            //}

            bgmChannelIndex = loopIndex;
            bgmPlayers[0].clip = bgmClips[(int)bgm + ranIndex];
            bgmPlayers[0].Play();
            Debug.Log("bgm");
            break;
        }
        //if (isPlay)
        //{
        //    bgmPlayer.Play();
        //}
        //else
        //{
        //    bgmPlayer.Stop();
        //}
    }


    public void PlaySfx(SFX sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + sfxChannelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            int ranIndex = 0;
            //if (sfx == SFX.PlayerColorSwap)
            //{
            //    ranIndex = Random.Range(0, 2);
            //}

            sfxChannelIndex = loopIndex;
            sfxPlayers[0].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[0].Play();
            break;
        }
    }
}

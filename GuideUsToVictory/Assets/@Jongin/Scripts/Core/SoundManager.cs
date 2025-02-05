using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public AudioSource bgmPlayer;
    public AudioSource effectPlayer;
    float volume;
    public AudioClip titleBgm;
    public AudioClip selectBgm;
    public AudioClip inGameBgm;
    public AudioClip[] winBgms;
    public AudioClip loseBgm;

    public AudioClip uiHoverSound;
    public AudioClip unitGenerateSound;
    public AudioClip makeBidSound;
    public AudioClip successBidSound;
    public AudioClip hitSound;
    public AudioClip putBlockSound;
    void Start()
    {
        volume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);

        bgmPlayer.volume = volume;


    }

    public void OnPlayBGM(AudioClip bgm)
    {
        bgmPlayer.clip = bgm;
        bgmPlayer.Play();
    }
    public void SetBGMVolume(float value)
    {
        bgmPlayer.volume = value;
    }
    public void SetEffectVolume(float value)
    {
        effectPlayer.volume = value;
    }

    int currentTrack = 0;
    public void PlayWinBgm()
    {
        if (currentTrack < winBgms.Length)
        {
            bgmPlayer.clip = winBgms[currentTrack];
            bgmPlayer.Play();
            Invoke(nameof(PlayWinBgm), winBgms[currentTrack].length);
            currentTrack++;
        }
    }
    public void PlayOneShot(AudioClip effectClip)
    {
        effectPlayer.PlayOneShot(effectClip);
    }

    public AudioClip[] explosionSounds; // 3���� ���� �Ҹ�

    public float minDelay = 0.2f; // �ּ� ��� ����
    public float maxDelay = 0.5f; // �ִ� ��� ����

    private int explosionCount = 20; // ������ ��� Ƚ��


    public void PlayMeteorSound()
    {
        StartCoroutine(PlayExplosions());
    }

    IEnumerator PlayExplosions()
    {
        for (int i = 0; i < explosionCount; i++)
        {
            PlayRandomExplosion(); // ������ ���
            float delay = Random.Range(minDelay, maxDelay); // ������ ��� �ð�
            yield return new WaitForSeconds(delay); // ���� ������� ���
        }
    }

    void PlayRandomExplosion()
    {
        if (explosionSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, explosionSounds.Length); // ������ ������ ����
            effectPlayer.PlayOneShot(explosionSounds[randomIndex]); // ������ ���
        }
    }
}

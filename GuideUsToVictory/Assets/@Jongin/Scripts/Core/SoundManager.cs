using UnityEngine;

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
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{

    public GameObject optionPanel;
    public Slider bgmSlider;
    public Slider effectSlider;
    private void Start()
    {
        SoundManager.Instance.OnPlayBGM(SoundManager.Instance.titleBgm);
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume",1f);
        effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1f);
        bgmSlider.onValueChanged.AddListener(BGMVolumeChange);
        effectSlider.onValueChanged.AddListener(EffectVolumeChange);
    }
    public void LoadUnitSelectScene()
    {
        SceneManager.LoadScene(1);
    }
    public void GameQuit()
    {
        Application.Quit();
    }

    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }

    public void BGMVolumeChange(float value)
    {
        PlayerPrefs.SetFloat("BGMVolume", value);
        SoundManager.Instance.SetBGMVolume(value);
    }
    public void EffectVolumeChange(float value)
    {
        PlayerPrefs.SetFloat("EffectVolume", value);
        SoundManager.Instance.SetEffectVolume(value);
    }

}

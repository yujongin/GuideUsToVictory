using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;
public class UnitSelectManager : MonoBehaviour
{
    public Image description;
    public Image race;

    public Sprite[] descriptions;
    public Sprite[] raceImages;

    ERace currentSelectRace;

    int num = 0;
    Vector3 descriptionStartPos;
    Sequence descriptionTween;
    private void Start()
    {
        description.sprite = descriptions[num];
        race.sprite = raceImages[num]; 
        ShowDescription(num);
        currentSelectRace = (ERace)num;

        descriptionStartPos = description.transform.position;

        //353 -> 473
        descriptionTween = DOTween.Sequence()
            .Append(description.GetComponent<RectTransform>().DOMoveX(473, 1f))
            .Join(description.DOFade(1, 1))
            .SetAutoKill(false);

        SoundManager.Instance.OnPlayBGM(SoundManager.Instance.selectBgm);
    }

    public void SelectChange()
    {
        num++;
        if (num > raceImages.Length - 1) { num = 0; }

        ShowDescription(num);
        race.sprite = raceImages[num];
        currentSelectRace = (ERace)num;
    }

    public void ShowDescription(int num)
    {
        description.sprite = descriptions[num];
        descriptionTween.Restart();
    }

    public void ReadyButtonPressed()
    {
        DOTween.KillAll();
        PlayerPrefs.SetString("MyRace", currentSelectRace.ToString());
        SceneManager.LoadScene(2);

    }
    public void BackButtonPressed()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(0);

    }
}

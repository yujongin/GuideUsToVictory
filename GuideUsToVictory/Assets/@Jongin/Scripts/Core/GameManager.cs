using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;
public class GameManager : MonoBehaviour
{
    public LayerMask enemyLayer;

    EGameState gameState;
    public EGameState GameState { get { return gameState; } }

    public TMP_Text timerText;
    public TMP_Text noticeText;
    public TMP_Text blockCountText;
    public TMP_Text unitCountText;
    public TMP_Text faithText;

    public TeamData[] teamDatas = new TeamData[2];
    public TeamData myTeamData;
    public TeamData enemyTeamData;

    public GameObject resultImage;
    public GameObject optionPanel;

    public Image teamImage;
    private float time;
    float readyTime = 30f;
    float unitSpawnTerm = 30f;

    UnitSelectAI unitSelectAI;
    Sequence noticeTextSequence;
    Sequence teamImageSequence;

    public ETeam loseTeam;
    private void Start()
    {
        GameInit();
        Managers.UI.Init();
        unitSelectAI = GameObject.Find("AIManager").GetComponent<UnitSelectAI>();
        unitSelectAI.Init();
        Managers.Auction.Init();
        SoundManager.Instance.OnPlayBGM(SoundManager.Instance.inGameBgm);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
        }

        time -= Time.deltaTime;

        if (time <= 0)
        {
            switch (GameState)
            {
                case EGameState.Ready:
                    SetState(EGameState.Battle);
                    StartCoroutine(AddFaithPerSeconds());
                    break;
                case EGameState.Battle:
                    SpawnUnitsInDict();
                    break;
                case EGameState.End:
                    break;

            }
        }
        if (GameState == EGameState.End) return;

        timerText.text = time.ToString("F0");
        blockCountText.text = myTeamData.CurBlockCount.ToString();
        unitCountText.text = myTeamData.Population.ToString();
        faithText.text = myTeamData.Faith.ToString();
    }
    IEnumerator AddFaithPerSeconds()
    {
        while (GameState == EGameState.Battle)
        {
            for (int i = 0; i < teamDatas.Length; i++)
            {
                teamDatas[i].Faith += teamDatas[i].AddFaith;
            }

            yield return new WaitForSeconds(5);
        }
    }
    void GameInit()
    {
        //ETeam randomTeam = (ETeam)UnityEngine.Random.Range(1, 2);
        //ETeam otherTeam = randomTeam == ETeam.Blue ? ETeam.Red : ETeam.Blue;

        ERace myRace = (ERace)Enum.Parse(typeof(ERace), PlayerPrefs.GetString("MyRace"));
        // check ai or human
        ERace enemyRace = (ERace)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ERace)).Length);
        //ERace enemyRace = ERace.Human;

        myTeamData = new TeamData(ETeam.Blue, myRace);
        enemyTeamData = new TeamData(ETeam.Red, enemyRace);
        teamDatas[0] = myTeamData;
        teamDatas[1] = enemyTeamData;
        enemyLayer = LayerMask.GetMask(enemyTeamData.Team.ToString());
        //set default value unitCount, blockCount
        for (int i = 0; i < 2; i++)
        {
            List<GameObject> units = Managers.Resource.Units[teamDatas[i].Team];
            for (int j = 0; j < units.Count; j++)
            {
                UnitBase unit = units[j].GetComponent<UnitBase>();
                if (unit.baseStat.Race == teamDatas[i].Race)
                {
                    teamDatas[i].UnitCountDict.Add(unit.baseStat.name, 0);
                }
            }
            teamDatas[i].MaxBlockCount = 4;
            teamDatas[i].CurBlockCount = teamDatas[i].MaxBlockCount;
            teamDatas[i].Population = 0;
            teamDatas[i].Faith = 300;
            teamDatas[i].AddFaith = 2;
        }

        time = readyTime;

        //ImageTween
        teamImageSequence = DOTween.Sequence();
        teamImageSequence.Append(teamImage.DOFade(1, 0.5f));
        teamImageSequence.AppendInterval(1);
        teamImageSequence.Append(teamImage.DOFade(0, 0.5f));

        //textTween
        noticeTextSequence = DOTween.Sequence();
        Tween FadeIn = DOTween.To(() => noticeText.alpha, x => noticeText.alpha = x, 1f, 0.5f);
        noticeTextSequence.Append(FadeIn);
        noticeTextSequence.AppendInterval(3);
        Tween FadeOut = DOTween.To(() => noticeText.alpha, x => noticeText.alpha = x, 0f, 0.5f);
        noticeTextSequence.Append(FadeOut)
            .SetAutoKill(false).Pause();

        CallNoticeTextFade("30초 후 유닛을 소환합니다. \n유닛을 추가하세요!", Color.white);
        SetState(EGameState.Ready);
    }

    public void AddFaith(ETeam team, float price)
    {
        GetTeamData(team).Faith += price;
    }
    public void UseFaith(ETeam team, float price)
    {
        GetTeamData(team).Faith -= price;
    }

    public void AddUnit(ETeam team, UnitData unitData)
    {
        TeamData data = GetTeamData(team);
        // over than max population or less than unit Capacity
        if (data.CurBlockCount == 0 || data.CurBlockCount < unitData.Capacity)
        {
            CallNoticeTextFade("소환 블록이 부족합니다.", Color.red);
            return;
        }
        // lack of money
        if (data.Faith < unitData.PriceFaith)
        {
            CallNoticeTextFade("신앙이 부족합니다.", Color.red);
            return;
        }
        data.CurBlockCount -= unitData.Capacity;
        data.Faith -= unitData.PriceFaith;
        data.Population++;
        data.UnitCountDict[unitData.name]++;
    }

    public void IncreaseMaxBlock(ETeam team, int count)
    {
        TeamData data = GetTeamData(team);

        data.MaxBlockCount += count;
        data.CurBlockCount += count;
    }

    public void UnlockUnit(ETeam team)
    {
        for (int i = 0; i < GetTeamData(team).UnitUnlock.Length; i++)
        {
            if (!GetTeamData(team).UnitUnlock[i])
            {
                GetTeamData(team).UnitUnlock[i] = true;
                if (team == myTeamData.Team)
                    Managers.UI.SetUnitUnlock();
                return;
            }
        }
    }
    public void RemoveUnit(ETeam team, UnitData unitData)
    {
        TeamData data = GetTeamData(team);
        if (data.CurBlockCount == data.MaxBlockCount || data.UnitCountDict[unitData.name] == 0) return;

        data.CurBlockCount += unitData.Capacity;
        data.Faith += unitData.PriceFaith;
        data.Population--;
        data.UnitCountDict[unitData.name]--;
    }

    TeamData GetTeamData(ETeam team)
    {
        for (int i = 0; i < 2; i++)
        {
            if (teamDatas[i].Team == team)
            {
                return teamDatas[i];
            }
        }
        return null;
    }

    public void SetState(EGameState state)
    {
        gameState = state;
    }

    public void GameEnd(ETeam loseTeam)
    {
        this.loseTeam = loseTeam;
        SetState(EGameState.End);

        if(loseTeam == myTeamData.Team)
        {
            SoundManager.Instance.OnPlayBGM(SoundManager.Instance.loseBgm);
        }
        else
        {
            SoundManager.Instance.PlayWinBgm();
        }
        string text = loseTeam == myTeamData.Team ? "Lose" : "Win";
        resultImage.SetActive(true);
        resultImage.transform.Find(myTeamData.Team.ToString() + text).gameObject.SetActive(true);
    }

    public void LoadSceneTitle()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(0);
    }
    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }
    void SpawnUnitsInDict()
    {
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.unitGenerateSound);
        //summon Timer
        time = unitSpawnTerm;

        //Set AIUnit
        unitSelectAI.SetAIUnitGreedy();

        //unit production
        for (int i = 0; i < 2; i++)
        {
            Managers.UnitSpawn.SpawnUnits(teamDatas[i]);
        }
    }
    public void CallNoticeTextFade(string text, Color color)
    {
        noticeText.color = color;
        noticeText.text = text;
        noticeText.alpha = 0;
        noticeTextSequence.Restart();
    }

}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public TeamData[] teamDatas = new TeamData[2];
    public TeamData myTeam;
    public TeamData enemyTeam;

    private float time;
    float readyTime = 10f;
    float unitSpawnTerm = 20f;
    private void Start()
    {
        GameInit();
        Managers.UI.Init();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    AddUnit(ETeam.Blue, "EliteArcher");
        //    AddUnit(ETeam.Blue, "JuniorKnight");
        //    AddUnit(ETeam.Red, "EliteArcher");
        //    AddUnit(ETeam.Red, "JuniorKnight");
        //}

        time -= Time.deltaTime;
        
        if (time <= 0)
        {
            switch (GameState)
            {
                case EGameState.Ready:
                    SetState(EGameState.Battle);
                    break;
                case EGameState.Battle:
                    SpawnUnits();
                    break;
            }
        }

        timerText.text = time.ToString("F0");
        blockCountText.text = myTeam.CurBlockCount.ToString();
        unitCountText.text = myTeam.Population.ToString();
    }
    void GameInit()
    {
        ETeam randomTeam = (ETeam)Random.Range(0, 2);
        ETeam otherTeam = randomTeam == ETeam.Blue ? ETeam.Red : ETeam.Blue;
        ERace tempRace = ERace.Human;
        myTeam = new TeamData(randomTeam, tempRace);
        enemyTeam = new TeamData(otherTeam, tempRace);
        teamDatas[0] = myTeam;
        teamDatas[1] = enemyTeam;

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
        }

        time = readyTime;
        SetState(EGameState.Ready);
    }

    public void AddUnit(ETeam team, UnitData unitData)
    {
        TeamData data = GetTeamData(team);
        if (data.CurBlockCount == 0 || data.CurBlockCount < unitData.Capacity) return;

        data.CurBlockCount-= unitData.Capacity;
        data.Population++;
        data.UnitCountDict[unitData.name]++;
    }

    public void IncreaseMaxBlock(ETeam team, int count)
    {
        TeamData data = GetTeamData(team);

        data.MaxBlockCount += count;
        data.CurBlockCount += count;
    }

    public void RemoveUnit(ETeam team, UnitData unitData)
    {
        TeamData data = GetTeamData(team);
        if (data.CurBlockCount == data.MaxBlockCount || data.UnitCountDict[unitData.name] == 0) return;

        data.CurBlockCount+= unitData.Capacity;
        data.Population--;
        data.UnitCountDict[unitData.name]--;
    }

    TeamData GetTeamData(ETeam team)
    {
        for(int i = 0; i < 2; i++)
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

    void SpawnUnits()
    {
        //summon Timer
        time = unitSpawnTerm;
        //unit production
        for(int i = 0; i < 2;i++)
        {
            Managers.UnitSpawn.SpawnUnits(teamDatas[i]);
        }
    }
    void CallNoticeTextFade(string text)
    {
        noticeText.text = text;
        noticeText.alpha = 0;
        Sequence noticeTextSequence = DOTween.Sequence();
        Tween FadeIn = DOTween.To(() => noticeText.alpha, x => noticeText.alpha = x, 1f, 0.5f);
        noticeTextSequence.Append(FadeIn);
        noticeTextSequence.AppendInterval(3);
        Tween FadeOut = DOTween.To(() => noticeText.alpha, x => noticeText.alpha = x, 0f, 0.5f);
        noticeTextSequence.Append(FadeOut);
    }

    void UpdateEnd()
    {
        //show Win Lose ment 
    }

}

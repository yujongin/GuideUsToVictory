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

    private void Start()
    {
        GameInit();
    }

    private void Update()
    {

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
        }

        SetState(EGameState.Ready);
    }

    public void AddUnit(ETeam team, string name)
    {
        TeamData data = GetTeamData(team);
        if (data.CurBlockCount == 0) return;

        data.CurBlockCount--;
        data.UnitCountDict[name]++;
    }

    public void RemoveUnit(ETeam team, string name)
    {
        TeamData data = GetTeamData(team);
        if (data.CurBlockCount == data.MaxBlockCount || data.UnitCountDict[name] == 0) return;

        data.CurBlockCount++;
        data.UnitCountDict[name]--;
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

        switch (gameState)
        {
            case EGameState.Ready:
                StartCoroutine(UpdateReady());
                break;
            case EGameState.Battle:
                break;
            case EGameState.End:
                break;
        }
    }

    IEnumerator UpdateReady()
    {
        //explain text 
        CallNoticeTextFade("30초 뒤에 유닛이 생성됩니다.");
        time = 30;
        while (gameState == EGameState.Ready)
        {
            if (time <= 0)
            {
                time = 0;
                SetState(EGameState.Battle);
                yield break;
            }

            time -= Time.deltaTime;
            timerText.text = time.ToString("F1");
            yield return null;
        }
        //change first 4 block to unit


        yield return null;
    }
    void UpdateBattle()
    {
        //summon Timer
        time = 20;
        //unit production
        while (gameState == EGameState.Battle)
        {

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

    }

}

using System;
using UnityEngine;
using static Define;
public class UnitSelectAI : MonoBehaviour
{
    TeamData AITeamData;
    UnitData[] AIUnitDatas;
    ETeam AITeam;
    ERace AIRace;
    public void Init()
    {
        AITeamData = Managers.Game.enemyTeamData;
        AITeam = AITeamData.Team;
        AIRace = AITeamData.Race;
        AIUnitDatas = Managers.Resource.GetUnitsByRace(AITeam, AIRace);

        SetAIUnitGreedy();
    }

    public void SetAIUnitGreedy()
    {
        //초기화
        for (int i = 0; i < AIUnitDatas.Length; i++)
        {
            AITeamData.CurBlockCount += AIUnitDatas[i].Capacity * AITeamData.UnitCountDict[AIUnitDatas[i].name];
            AITeamData.Faith += AIUnitDatas[i].PriceFaith * AITeamData.UnitCountDict[AIUnitDatas[i].name];
            AITeamData.UnitCountDict[AIUnitDatas[i].name] = 0;
        }
        AITeamData.Population = 0;

        int curBlock = AITeamData.CurBlockCount;
        float curFaith = AITeamData.Faith;

        Array.Sort(AIUnitDatas, (a, b) => (a.PriceFaith > b.PriceFaith) ? -1 : 1);
        for (int i = 0; i < AIUnitDatas.Length; i++)
        {
            int cost = AIUnitDatas[i].cost - 1;
            if (AITeamData.UnitUnlock[cost] == false)
            {
                continue;
            }
            //구입할 수 있는만큼 구입
            while (AIUnitDatas[i].Capacity <= curBlock && AIUnitDatas[i].PriceFaith <= curFaith)
            {
                int num = i;
                if (AIUnitDatas[i].Capacity == 1)
                {
                    num = UnityEngine.Random.Range(i, AIUnitDatas.Length);
                }
                Managers.Game.AddUnit(AITeam, AIUnitDatas[num]);
                curBlock -= AIUnitDatas[num].Capacity;
                curFaith -= AIUnitDatas[num].PriceFaith;
            }
        }
    }
}


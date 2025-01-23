using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TeamData
{
    public ETeam Team;
    public ERace Race;
    public Dictionary<string, int> UnitCountDict;
    public int MaxBlockCount;
    public int CurBlockCount;
    public int Population;
    public float Faith;
    public float AddFaith;
    public TeamData(ETeam team, ERace race)
    {
        this.Team = team;
        this.Race = race;
        UnitCountDict = new Dictionary<string, int>();
    }
}

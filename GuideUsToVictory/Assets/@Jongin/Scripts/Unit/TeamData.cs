using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TeamData
{
    public ETeam Team;
    public ERace Race;
    public Dictionary<string, int> UnitCountDict;
    public float MaxBlockCount;
    public float CurBlockCount;

    public TeamData(ETeam team, ERace race)
    {
        this.Team = team;
        this.Race = race;
        UnitCountDict = new Dictionary<string, int>();
    }
}

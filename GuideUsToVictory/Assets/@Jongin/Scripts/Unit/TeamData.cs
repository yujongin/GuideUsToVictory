using System.Collections.Generic;
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
    public bool[] UnitUnlock;
    public TeamData(ETeam team, ERace race)
    {
        this.Team = team;
        this.Race = race;
        UnitCountDict = new Dictionary<string, int>();
        UnitUnlock = new bool[4] { true, true, false, false };
    }
}

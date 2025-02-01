using System.Collections;
using UnityEngine;

public class AuctionAI : MonoBehaviour
{
    TeamData aiTeamData;

    public void Start()
    {
        aiTeamData = Managers.Game.enemyTeamData;
    }
    public void MakeBidAI()
    {
    }
}

using System.Collections.Generic;
using UnityEngine;
using static Define;
public class GameManager : MonoBehaviour
{
    EGameState gameState;
    public EGameState GameState { get { return gameState; } }


    private void Awake()
    {
        gameState = EGameState.Ready;
    }


    public void SetState(EGameState state)
    {
        gameState = state;

        switch (gameState)
        {
            case EGameState.Ready:
                break;
            case EGameState.Battle:
                break;
            case EGameState.End:
                break;
        }
    }

    void UpdateReady()
    {
        //change first 4 block to unit

    }

    void UpdateBattle()
    {
        //summon Timer
        
        //unit production


    }

    void UpdateEnd()
    {

    }

}

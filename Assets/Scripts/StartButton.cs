using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private ChessGameController cgc;
    // Start is called before the first frame update
    void Start()
    {
        cgc = GameObject.FindGameObjectsWithTag("GameMaster")[0].GetComponent(typeof(ChessGameController)) as ChessGameController;
    }

    public void startGame() {
        cgc.RestartGame();
    }
}

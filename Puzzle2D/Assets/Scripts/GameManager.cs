using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public PlayerController pc1, pc2;
    public int winner;
    public void GameOver(int playerId) {
        if (playerId == 1) {
            winner = 2;
        }
        if (playerId == 2) {
            winner = 1;
        }
        print("Game Over\n Winner is player " + winner );

        pc1.enabled = false;
        pc2.enabled = false;
        enabled = false;
    }
}

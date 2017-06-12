using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public PlayerController pc1, pc2;
    public PlayerGrid grid1, grid2;
    public void GameOver(int playerId) {
        print("Game Over\n Winner is player " + playerId);
        pc1.enabled = false;
        pc2.enabled = false;
        enabled = false;
    }

    // Update is called once per frame
    void update() {
 //   public void IsGameOver() { 
        if (grid1.grid[(int)pc1.defaultSpawnX1][(int)pc1.defaultSpawnY1] != PuyoType.None) {
            GameOver(2);
            print("loppuu");
        }
        if (grid2.grid[(int)pc2.defaultSpawnX2][(int)pc2.defaultSpawnY2] != PuyoType.None) {
            GameOver(1);
            print("Loppuu");
        }
        else {
            print("Peli jatkuu");
        }
    }
}

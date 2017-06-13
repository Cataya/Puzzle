using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public string nextScene;
    Text text;

    public PlayerController pc1, pc2;
    public int winner;
    public void GameOver(int playerId) {
        if (playerId == 1) {
            winner = 2;
        }
        if (playerId == 2) {
            winner = 1;
        }
        text = GameObject.FindObjectOfType<Text>();
        text.text = "Game Over!\n Player" + winner + "win\n press spacebar \n to restart game!";
        print("Game Over\n Winner is player " + winner );
        pc1.enabled = false;
        pc2.enabled = false;
        return;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (nextScene == "")//Jos seuraavaa kohtausta/kenttää ei ole määritelty
            {
                nextScene = SceneManager.GetActiveScene().name;//Annetaan muuttujan nimeksi nykyisen kentän nimi
            }
            SceneManager.LoadScene(nextScene); //Ladataan kenttä, jonka nimi on muuttujassa
        }
    }
}

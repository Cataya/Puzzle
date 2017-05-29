﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTuomas : MonoBehaviour {
    public GameObject debugSprite;
    Transform sprite;
    public float debugX;
    public float debugY;
    public PlayerGrid grid;
    public float velocity;
    
    //the falling test sprite
    GameObject g;

    void Start () {
        g = Instantiate(debugSprite);
        sprite = g.transform;
    }
	
	void Update () {
        // liikuttaminen

        // kääntäminen

        // tippuminen (2)
        if (IsTherePuyoBelow() && g != null) {
            GameObject d = Instantiate(g);
            Destroy(g);
            grid.AddPuyo((int)debugX, (int)debugY + 1, PuyoType.Puyo1, d);
        }
        if(!IsTherePuyoBelow()) {
            debugY = debugY - velocity * Time.deltaTime;                    //Ohajataan spriten liikettä y-akselilla
        }

        // piirtäminen (1)
        float worldX = -(grid.nX - 1) / 2f * grid.gridDistance + debugX * grid.gridDistance;
        float worldY = -(grid.nY - 1) / 2f * grid.gridDistance + debugY * grid.gridDistance;

        if (g != null) {
            sprite.transform.position = new Vector3(worldX, worldY) + grid.transform.position;          //Liikutetaan spritea, riippuvainen debugY:n arvosta
        }
    }

    bool IsTherePuyoBelow() {
        if (grid.grid[(int)debugX][(int)debugY] != PuyoType.None) {
            print("There is indeed a muthafucking Puyo below!");
            return true;
        }else {
            return false;
        }
    }
}

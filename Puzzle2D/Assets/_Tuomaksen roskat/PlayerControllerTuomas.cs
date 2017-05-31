using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTuomas : MonoBehaviour {
    public GameObject debugSprite;
    public float debugX;
    public float debugY;
    private float defaultDebugX;
    private float defaultDebugY;
    public PlayerGrid grid;
    public float velocity;
    
    //the falling test sprite
    GameObject g;
    Transform sprite;

    void Start () {
        defaultDebugX = debugX;
        defaultDebugY = debugY;

        g = Instantiate(debugSprite);
        sprite = g.transform;
    }
	
	void Update () {          
        //spawnaaminen
        if (g == null) {
            g = Instantiate(debugSprite);
            sprite = g.transform;
        }

        // liikuttaminen

        // kääntäminen

        // tippuminen (2)
        if (IsTherePuyoBelow() && g != null) {
            grid.AddPuyo((int)debugX, (int)debugY + 1, PuyoType.Puyo1, g);
            g = null;
            debugX = defaultDebugX;
            debugY = defaultDebugY;
        }
        if(!IsTherePuyoBelow()) {
            debugY = debugY - velocity * Time.deltaTime;                    //Ohjataan spriten liikettä y-akselilla
        }

        // piirtäminen (1)
        float worldX = -(grid.nX - 1) / 2f * grid.gridDistance + debugX * grid.gridDistance;
        float worldY = -(grid.nY - 1) / 2f * grid.gridDistance + debugY * grid.gridDistance;

        if (g != null) {
            sprite.transform.position = new Vector3(worldX, worldY) + grid.transform.position;          //Liikutetaan spritea, riippuvainen debugY:n arvosta
        }
    }

    bool IsTherePuyoBelow() {
        return grid.grid[(int)debugX][(int)debugY] != PuyoType.None;
    }
}

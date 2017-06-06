using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject debugSprite;
    public float spawnX;
    public float spawnY;
    private float defaultSpawnX;
    private float defaultSpawnY;
    public PlayerGrid grid;
    public float velocity;

    //the falling test sprite
    GameObject g1, g2;
    Transform sprite1, sprite2;

    void Start() {
        defaultSpawnX = spawnX;
        defaultSpawnY = spawnY;

        g1 = Instantiate(debugSprite);
        sprite1 = g1.transform;
    }

    void Update() {
        //spawnaaminen
        if (g1 == null) {
            g1 = Instantiate(debugSprite);
            sprite1 = g1.transform;
        }

        // liikuttaminen

        // Puyon siirto oikealle ja vasemmalle
        if (Input.GetKeyDown(KeyCode.LeftArrow) && spawnX > 0)
        {
            spawnX = spawnX - 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && spawnX < grid.nX -1)
        {
            spawnX = spawnX + 1;
        }
        // kääntäminen

        // tippuminen (2)

        //Tarkistus onko alhaalla jotain edessä?
        if (IsThereObstacleBelow() && g1 != null) {
            grid.AddPuyo(Mathf.FloorToInt(spawnX), Mathf.FloorToInt(spawnY + 1), PuyoType.Puyo2, g1);
            g1 = null;
            spawnX = defaultSpawnX;
            spawnY = defaultSpawnY;
            StartCoroutine(grid.DropMatchRemove());
        }
        if (!IsThereObstacleBelow()/* && IsThereObstacleLeft() && IsThereObstacleRigh()*/) {
            spawnY = spawnY - velocity * Time.deltaTime;                    //Ohjataan spriten liikettä y-akselilla
        }
        //Tarkistus onko vasemmalla jotain edessä?
        if (IsThereObstacleLeft() && g1 != null) {
            print("Palikka edessä vasemmalla");
            //******************************
        }
        //Tarkistus onko vasemmalla jotain edessä
        if (IsThereObstacleRight() && g1 != null) {
            print("Palikka edessä oikealla");
            //******************************
        }


        // piirtäminen (1)
        float worldX = -(grid.nX - 1) / 2f * grid.gridDistance + spawnX * grid.gridDistance;
        float worldY = -(grid.nY - 1) / 2f * grid.gridDistance + spawnY * grid.gridDistance;

        if (g1 != null) {
            sprite1.transform.position = new Vector3(worldX, worldY) + grid.transform.position;          //Liikutetaan spritea, riippuvainen debugY:n arvosta
        }
    }

    bool IsThereObstacleBelow() {
        return grid.grid[(int)spawnX][(int)spawnY] != PuyoType.None || spawnY < 0;
    }
    bool IsThereObstacleLeft() {
        bool rightPuyo = false;
        if (spawnX > 0) {
           rightPuyo = grid.grid[(int)spawnX - 1][(int)spawnY] != PuyoType.None;
        }
        return rightPuyo;
    }
    bool IsThereObstacleRight() {
        bool leftPuyo = false;
        if (spawnX < grid.nX ) {
            leftPuyo = grid.grid[(int)spawnX + 1][(int)spawnY] != PuyoType.None;
        }
        return leftPuyo;
    }
}

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
    GameObject g;
    Transform sprite;

    void Start() {
        defaultSpawnX = spawnX;
        defaultSpawnY = spawnY;

        g = Instantiate(debugSprite);
        sprite = g.transform;
    }

    void Update() {
        //spawnaaminen
        if (g == null) {
            g = Instantiate(debugSprite);
            sprite = g.transform;
        }

        // liikuttaminen

        // Puyon siirto vasemmalle
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            spawnX = spawnX - grid.gridDistance;
        }
        // Puyon siirto oikealla
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            spawnX = spawnX + grid.gridDistance;
        }
        // kääntäminen

        // tippuminen (2)
        if (IsThereObstacleBelow() && g != null) {
            grid.AddPuyo(Mathf.FloorToInt(spawnX), Mathf.FloorToInt(spawnY + 1), PuyoType.Puyo2, g);
            g = null;
            spawnX = defaultSpawnX;
            spawnY = defaultSpawnY;
        }
        if (!IsThereObstacleBelow()) {
            spawnY = spawnY - velocity * Time.deltaTime;                    //Ohjataan spriten liikettä y-akselilla
        }

        // piirtäminen (1)
        float worldX = -(grid.nX - 1) / 2f * grid.gridDistance + spawnX * grid.gridDistance;
        float worldY = -(grid.nY - 1) / 2f * grid.gridDistance + spawnY * grid.gridDistance;

        if (g != null) {
            sprite.transform.position = new Vector3(worldX, worldY) + grid.transform.position;          //Liikutetaan spritea, riippuvainen debugY:n arvosta
        }
    }

    bool IsThereObstacleBelow() {
        return grid.grid[(int)spawnX][(int)spawnY] != PuyoType.None || spawnY < 0;
    }
}

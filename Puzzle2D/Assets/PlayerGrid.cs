using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuyoType {None, Trash, Puyo1, Puyo2, Puyo3, Puyo4, Puyo5}

public class PlayerGrid : MonoBehaviour {

    public int nX; // Leveys
    public int nY; // Korkeus

    public float gridDistance;

    public Transform gridCenter;

    public GameObject[] debugSprites;

    List<List<PuyoType>> grid;
    List<List<GameObject>> sprites;

    void Awake() {
        grid = new List<List<PuyoType>>();
        sprites = new List<List<GameObject>>();
        for (int i = 0; i < nX; i++) {
            var column = new List<PuyoType>(nY);
            var columnSprite = new List<GameObject>(new GameObject[nY]);
            for (int j = 0; j < nY; j++) {
                column.Add(PuyoType.None);
            }
            grid.Add(column);
            sprites.Add(columnSprite);
        }
    }

    // Use this for initialization
    void Start() {
        //grid[1][0] = PuyoType.Puyo1;
        //grid[1][1] = PuyoType.Puyo2;
        //print(grid[1][0]);
        var b = Instantiate(debugSprites[0]);
        var r = Instantiate(debugSprites[1]);
        var y = Instantiate(debugSprites[2]);
        AddPuyo(0, 0, PuyoType.Puyo1, b);
        AddPuyo(4, 0, PuyoType.Puyo2, r);
        AddPuyo(0, 5, PuyoType.Puyo3, y);
    }

    public void AddPuyo(int x, int y, PuyoType puyo, GameObject Sprite) {
        grid[x][y] = puyo;
        sprites[x][y] = Sprite;
        PlacePuyo(x, y, Sprite);
    }

    void PlacePuyo(int gridX, int gridY, GameObject Sprite) {
        float worldX = -(nX - 1) / 2f * gridDistance + gridX * gridDistance;
        float worldY = -(nY - 1) / 2f * gridDistance + gridY * gridDistance;

        Sprite.transform.position = new Vector3(worldX, worldY) + gridCenter.position;
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {

        }
    }
}

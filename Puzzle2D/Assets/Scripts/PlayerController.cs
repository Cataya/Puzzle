using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject debugSprite;
    Transform sprite;
    public float debugX;
    public float debugY;
    public PlayerGrid grid;
    // Use this for initialization
	void Start () {
        var g = Instantiate(debugSprite);
        sprite = g.transform;
	}
	
	void Update () {
        // liikuttaminen

        // kääntäminen

        // tippuminen (2)

        // piirtäminen (1)
        float worldX = -(grid.nX - 1) / 2f * grid.gridDistance + debugX * grid.gridDistance;
        float worldY = -(grid.nY - 1) / 2f * grid.gridDistance + debugY * grid.gridDistance;

        sprite.transform.position = new Vector3(worldX, worldY) + grid.transform.position;
    }
}

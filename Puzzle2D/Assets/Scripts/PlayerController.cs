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
	
	// Update is called once per frame
	void Update () {
		
	}
}

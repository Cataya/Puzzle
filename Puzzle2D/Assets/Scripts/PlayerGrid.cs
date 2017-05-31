using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PuyoType {None, Trash, Puyo1, Puyo2, Puyo3, Puyo4} // Millaisia puyoja meillä on olemassa

public class PlayerGrid : MonoBehaviour {

    public int nX; // Leveys
    public int nY; // Korkeus

    public float gridDistance; //Ruutujen keskipisteiden etäisyys toisistaan

    public GameObject[] debugSprites;
    public GameObject[] randomizedPuyos;

    public PlayerController pc;

    [HideInInspector]
    public List<List<PuyoType>> grid; // Lista-taulukko, johon merkitään millainen puyo on kussakin ruududussa
    [HideInInspector]
    public List<List<GameObject>> sprites; // Lista-taulukko, jossa hallinnoidaan mikä palikka on missäkin ruudussa.


    public float y { get; private set; }
    public float x { get; private set; }

    void Awake() {  //(Teemu, Katja + Ykä)
        grid = new List<List<PuyoType>>(); // Muodostetaan lista-taulukko, johon tullaan tallentamaan tieto, onko ruudussa Puyo, jos on niin millainen puyo
        sprites = new List<List<GameObject>>(); // Muodostetaan lista-taulukko, johon tallennetaan millainen palikka piirretään mihinkin kohtaan
        for (int i = 0; i < nX; i++) { // luodaan niin monta taulukkoa kun on määritelty kentän leveydeksi
            var column = new List<PuyoType>(nY); // Luodaan jokaiseen taulukkoon niin monta solua, kuin on määritelty kentän korkeudeksi
            var columnSprite = new List<GameObject>(new GameObject[nY]); // Luodaan sama myös palikoille
            for (int j = 0; j < nY; j++) {
                column.Add(PuyoType.None); // Täytetään taulukon solut ensin tiedolla, että ne ovat tyhjiö 
            }
            grid.Add(column); //Täytetään taulukko
            sprites.Add(columnSprite); //Täytetään taulukko
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(nX * gridDistance, nY * gridDistance));        
    }
    // Use this for initialization
    void Start() { //(Teemu, Katja + Ykä)
        //grid[1][0] = PuyoType.Puyo1;
        //grid[1][1] = PuyoType.Puyo2;
        //print(grid[1][0]);
         TestGroups();
         FindPuyoGroups();


    }

	public void AddPuyo(int x, int y, PuyoType puyo, GameObject Sprite) { // Funktio, jossa lisätään taulukkoon tieto puyosta. (x-koordinaatti, y-koordinaatti, millainen puyo, millainen palikka)//(Teemu, Katja + Ykä)
        grid[x][y] = puyo; //Mihin koordinaatteihin lisätään tieto millainen puyo
        sprites[x][y] = Sprite; // Mihin koordinaatteihin lisätään millainen palikka
        PlacePuyo(x, y, Sprite); //Kutsutaan funktio, jolla piirretään palikka
    }
    public void DropMatchRemove() {
        pc.enabled = false;
        bool removedGroups = false;
            do {
            // DropPuyos();
                var groups = FindPuyoGroups();
                var groupsToRemove = MoreThanThreeInGroups(groups);
                removedGroups = groupsToRemove.Count > 0;
                RemoveGroups(groupsToRemove);

            } while (removedGroups);
        pc.enabled = true;
        }

    public void PlacePuyo(int gridX, int gridY, GameObject Sprite) { // Lasketaan paikka, johon piirretään palikka //(Teemu, Katja + Ykä)
        float worldX = -(nX - 1) / 2f * gridDistance + gridX * gridDistance; 
        float worldY = -(nY - 1) / 2f * gridDistance + gridY * gridDistance;

        Sprite.transform.position = new Vector3(worldX, worldY) + transform.position; //Piirretään palikka
    }
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)) {
            DropMatchRemove();
        }
#endif
    }
    // Katja, kesken
//    GameObject RandomPuyo() {
//        int index = Random.Range(0, randomizedPuyos.Length);
//        return gameObject;
//    }
	//Katja, kesken; hahmotelmaa miten list-toimii
    List<List<Vector2>> FindPuyoGroups() {
        List<List<Vector2>> groups = new List<List<Vector2>>(); //tehdään kaikista puyoista ryhmä, joihin lisätään viereiset puyot, mikäli ovat samanlaisia.

        // loopataan kaikki x, y läpi alhaalta ylös
        // jokaiselle x, y:
        for (int x=0; x < nX; x++) {
            for (int y = 0; y < nY ; y++) {
                var aPuyo = grid[x][y]; //Nykyinen puyo
                if (aPuyo == PuyoType.None) //Jos ruudussa ei ole puyoa niin ei tarvitse verrata viereisiin ruutuihin
                    continue;

                bool samePuyoLeft = x > 0 && aPuyo == grid[x - 1][y]; //Sama puyo vasemmalla
                bool samePuyoDown = y > 0 && aPuyo == grid[x][y - 1]; //Sama puyo oikealla
                bool samePuyoDownLeft = samePuyoLeft && samePuyoDown; //Sama puyo oikealla ja vasemmalla

                if (samePuyoDownLeft) {
                    //Jos alhaalla ja vasemmalla sama puyo yhdistetään taulukot
                    List<Vector2> gl= null, gd = null;
                    foreach (var g in groups) {
                        if (g.Contains(new Vector2(x - 1, y))){
                            gl = g;
                        }
                    }
                    foreach (var g in groups) {
                        if (g.Contains(new Vector2(x, y - 1))) {
                            gd = g;
                        }
                    }
                        gl.AddRange(gd);
                        gl.Add(new Vector2(x, y));
                        groups.Remove(gd);
                    }
                else if (samePuyoDown) {
                    //Jos alhaalla on sama puyo niin lisätään taulukkoon
                    foreach (var p in groups) {
                        bool found = p.Contains(new Vector2(x, y - 1));
                        if (found) {
                            p.Add(new Vector2(x, y));
                            break;
                        }
                    }   
                    //Jos sama puyo vasemmalla niin lisätään taulukkoon  
                } else if (samePuyoLeft) {
                    foreach (var p in groups) {
                        bool found = p.Contains(new Vector2(x -1, y) );
                        if (found) {
                            p.Add(new Vector2(x, y));
                            break;
                        }      
                    }
                    //Jos ei samoja puyoja alhaalla, eikä ylhäällä niin tehdään taulukko mihin lisätään
                } else {
                    groups.Add(new List<Vector2>());
                    groups[groups.Count - 1].Add(new Vector2(x,y));
                }
            }
        }
        return groups;
    }
    //Katjan, kesken
    List<List<Vector2>> MoreThanThreeInGroups(List<List<Vector2>> groups) {
        List<List<Vector2>> groupsToRemove = new List<List<Vector2>>();
        foreach (var g in groups) {
            if (g.Count > 3) {
                groupsToRemove.Add(g);
                }
            }
        return groupsToRemove;
        }

    void RemoveGroups(List<List<Vector2>> groupsToRemove) {
        for (int i = 0; i < groupsToRemove.Count; i++) {
            var group = groupsToRemove[i];
            while (group.Count > 0) {
                var vector = group[0];
                var GO = sprites[(int)vector.x][(int)vector.y];
                sprites[(int)vector.x][(int)vector.y] = null;
                Destroy(GO);
                group.RemoveAt(0);
                grid[(int)vector.x][(int)vector.y] = PuyoType.None;
            }
    }
}

    //Katja, saa käyttää testaukseen
    void TestGroups() {
        var b = Instantiate(debugSprites[0]);
        var r = Instantiate(debugSprites[1]);
        var b2 = Instantiate(debugSprites[0]);
        var r2 = Instantiate(debugSprites[1]);
        var y = Instantiate(debugSprites[2]);
        var y2 = Instantiate(debugSprites[2]);
        var r3 = Instantiate(debugSprites[1]);
        var r4 = Instantiate(debugSprites[1]);
        AddPuyo(0, 0, PuyoType.Puyo1, b);
        AddPuyo(1, 0, PuyoType.Puyo2, r);
        AddPuyo(1, 1, PuyoType.Puyo2, r2);
        AddPuyo(1, 2, PuyoType.Puyo1, b2);
		AddPuyo(4, 1, PuyoType.Puyo3, y);
        AddPuyo(4, 0, PuyoType.Puyo3, y2);
        AddPuyo(0, 1, PuyoType.Puyo2, r3);
        AddPuyo(0, 2, PuyoType.Puyo2, r4);
        
    }
    void DebugPrint(List<List<Vector2>> g) {
        foreach (var group in g) {
            string s = "";
            foreach (var cord in group) {
                s += cord;
            }
            print(s);
        }
    }
}

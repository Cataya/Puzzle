using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuyoType {None, Trash, Puyo1, Puyo2, Puyo3, Puyo4} // Millaisia puyoja meillä on olemassa

public class PlayerGrid : MonoBehaviour {

    public int nX; // Leveys
    public int nY; // Korkeus

    public float gridDistance; //Ruutujen keskipisteiden etäisyys toisistaan

    public GameObject[] debugSprites;
    public GameObject[] randomizedPuyos;

    List<List<PuyoType>> grid; // Lista-taulukko, johon merkitään millainen puyo on kussakin ruududussa
    List<List<GameObject>> sprites; // Lista-taulukko, jossa hallinnoidaan mikä palikka on missäkin ruudussa.

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

    void PlacePuyo(int gridX, int gridY, GameObject Sprite) { // Lasketaan paikka, johon piirretään palikka //(Teemu, Katja + Ykä)
        float worldX = -(nX - 1) / 2f * gridDistance + gridX * gridDistance; 
        float worldY = -(nY - 1) / 2f * gridDistance + gridY * gridDistance;

        Sprite.transform.position = new Vector3(worldX, worldY) + transform.position; //Piirretään palikka
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {

        }
    }
    // Katja, kesken
//    GameObject RandomPuyo() {
//        int index = Random.Range(0, randomizedPuyos.Length);
//        return gameObject;
//    }
	//Katja, kesken; hahmotelmaa miten list-toimii
    List<List<Vector2>> FindPuyoGroups() {
        List<List<Vector2>> groups = new List<List<Vector2>>();
        //int gridx = (int)xy.x;
        //int gridy = (int)xy.y;
        ////var aPuyo = grid[gridx][gridy];
        //var bPuyo = grid[gridx -][gridy];
        //var cPuyo = grid[gridx][gridy+1];
        //print("A" + aPuyo + "B" + bPuyo + "C" + cPuyo);

        //      print(groups);

        // loopataan kaikki x, y läpi alhaalta ylös
        // jokaiselle x, y:
        for (int i=0; i < nY-1; i++) {
            for (int j = 0; j < nX-1 ; j++) {
                var aPuyo = grid[i][j];
                print("\nA" + aPuyo);
                if (i > 0) {
                    // vas = vasemmalla puyo samaa väriä?
                    var bPuyo = grid[i-1][j];
                    print("\nB" + bPuyo);
                    if (aPuyo == bPuyo && aPuyo!= PuyoType.None && bPuyo!=PuyoType.None) {
                        // -> lisätään nyk. vasemman gruuppiin
                        print("\nVasemmalle" + aPuyo + bPuyo);
                    }
                }
                if (j > 0) {
                    // alh = alhaalla puyo samaa väriä?
                    var cPuyo = grid[i][j-1];
                    print("\nC" + cPuyo);
                    if (aPuyo == cPuyo && aPuyo!=PuyoType.None && cPuyo!=PuyoType.None) {
                        // -> lisätään nyk. alagruuppiin
                        print("\nAlas" + aPuyo + cPuyo);
                    }
                }

            }
        }

        // 
        // vas && alh ?
        // -> yhdistetään gruupit, lisätään nyk.
        // vas?
        // -> lisätään nyk. vasemman gruuppiin
        // alh?
        // -> lisätään nyk. alagruuppiin
        // ei kumpikaan?
        // -> uusi gruuppi, nyk. siihen

        return groups;
    }

    //Katja, saa käyttää testaukseen
    void TestGroups() {
        var b = Instantiate(debugSprites[0]);
        var r = Instantiate(debugSprites[1]);
        var b2 = Instantiate(debugSprites[0]);
        var r2 = Instantiate(debugSprites[1]);
        var y = Instantiate(debugSprites[2]);
        var y2 = Instantiate(debugSprites[2]);
        AddPuyo(0, 0, PuyoType.Puyo1, b);
        AddPuyo(1, 0, PuyoType.Puyo2, r);
        AddPuyo(1, 1, PuyoType.Puyo2, r2);
        AddPuyo(1, 2, PuyoType.Puyo1, b2);
		AddPuyo(3, 3, PuyoType.Puyo3, y);
        AddPuyo(2, 3, PuyoType.Puyo3, y2);
    }
}

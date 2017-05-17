using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuyoType {None, Trash, Puyo1, Puyo2, Puyo3, Puyo4} // Millaisia puyoja meillä on olemassa

public class PlayerGrid : MonoBehaviour {

    public int nX; // Leveys
    public int nY; // Korkeus

    public float gridDistance; //Ruutujen keskipisteiden etäisyys toisistaan

    public Transform gridCenter; // Ruudukon keskipiste

    public GameObject[] debugSprites;
    public GameObject[] randomizedPuyos;

    List<List<PuyoType>> grid; // Lista-taulukko, johon merkitään millainen puyo on kussakin ruududussa
    List<List<GameObject>> sprites; // Lista-taulukko, jossa hallinnoidaan mikä palikka on missäkin ruudussa.

    void Awake() {
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

	public void AddPuyo(int x, int y, PuyoType puyo, GameObject Sprite) { // Funktio, jossa lisätään taulukkoon tieto puyosta. (x-koordinaatti, y-koordinaatti, millainen puyo, millainen palikka)
        grid[x][y] = puyo; //Mihin koordinaatteihin lisätään tieto millainen puyo
        sprites[x][y] = Sprite; // Mihin koordinaatteihin lisätään millainen palikka
        PlacePuyo(x, y, Sprite); //Kutsutaan funktio, jolla piirretään palikka
    }

    void PlacePuyo(int gridX, int gridY, GameObject Sprite) { // Lasketaan paikka, johon piirretään palikka 
        float worldX = -(nX - 1) / 2f * gridDistance + gridX * gridDistance; 
        float worldY = -(nY - 1) / 2f * gridDistance + gridY * gridDistance;

        Sprite.transform.position = new Vector3(worldX, worldY) + gridCenter.position; //Piirretään palikka
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {

        }
    }
    void RandomPuyo() {
        int index = Random.Range(0, randomizedPuyos.Length);
    }
}

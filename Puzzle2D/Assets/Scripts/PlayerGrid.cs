﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGrid : MonoBehaviour {

    public int nX; // Leveys
    public int nY; // Korkeus
    public float destroyDelay;
    Text text;

    public float dropTime = 0.2f;
    public float gridDistance; //Ruutujen keskipisteiden etäisyys toisistaan

    int incomingTrash = 0; //Tulevaa roskaa
    int nextTrash = 0; //Seuraavien roskien aloituspaikka

    public PlayerController pc;
    public GameManager gm;
    public PlayerGrid otherPG;
    public Audio audioScript;

    public GameObject[] bridgePrefabs;

    [HideInInspector]
    public List<List<PuyoType>> grid; // Lista-taulukko, johon merkitään millainen puyo on kussakin ruududussa
    [HideInInspector]
    public List<List<GameObject>> sprites; // Lista-taulukko, jossa hallinnoidaan mikä palikka on missäkin ruudussa.
    List<Vector2> dropping = new List<Vector2>(); //Lista pudotettavista palikoista

    [HideInInspector]
    public List<List<GameObject>> horizontalBridges;
    [HideInInspector]
    public List<List<GameObject>> verticalBridges;

    public enum PuyoDropStatus { None, Drop, DropAndStop };

    //public float y { get; private set; }
    //public float x { get; private set; }





    void Awake() {  //(Teemu, Katja + Ykä)
        grid = new List<List<PuyoType>>(); // Muodostetaan lista-taulukko, johon tullaan tallentamaan tieto, onko ruudussa Puyo, jos on niin millainen puyo
        sprites = new List<List<GameObject>>(); // Muodostetaan lista-taulukko, johon tallennetaan millainen palikka piirretään mihinkin kohtaan
        for (int x = 0; x < nX; x++) { // luodaan niin monta taulukkoa kun on määritelty kentän leveydeksi
            var column = new List<PuyoType>(nY); // Luodaan jokaiseen taulukkoon niin monta solua, kuin on määritelty kentän korkeudeksi
            var columnSprite = new List<GameObject>(new GameObject[nY]); // Luodaan sama myös palikoille
            for (int y = 0; y < nY; y++) {
                column.Add(PuyoType.None); // Täytetään taulukon solut ensin tiedolla, että ne ovat tyhjiö 
            }
            grid.Add(column); //Täytetään taulukko
            sprites.Add(columnSprite); //Täytetään taulukko
        }

        horizontalBridges = new List<List<GameObject>>(nX - 1);
        verticalBridges = new List<List<GameObject>>(nY - 1);
        for(int x = 0; x < nX - 1; x++) {
            List<GameObject> column = new List<GameObject>(nY);
            horizontalBridges.Add(column);
        }
        for (int y = 0; y < nY - 1; y++) {
            List<GameObject> column = new List<GameObject>(nY);
            verticalBridges.Add(column);
        }
    }

    void Update() {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)) {
            DropMatchRemove();
        }
#endif
    }





    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(nX * gridDistance, nY * gridDistance));
    }

    public void AddPuyo(int x, int y, PuyoType puyo, GameObject Sprite) { // Funktio, jossa lisätään taulukkoon tieto puyosta. (x-koordinaatti, y-koordinaatti, millainen puyo, millainen palikka)//(Teemu, Katja + Ykä)
        grid[x][y] = puyo; //Mihin koordinaatteihin lisätään tieto millainen puyo
        sprites[x][y] = Sprite; // Mihin koordinaatteihin lisätään millainen palikka
        PlacePuyo(x, y, Sprite); //Kutsutaan funktio, jolla piirretään palikka
    }

    public void UpdateBridges() {
        //FindAdjacentPuyos()
    }

    public void PlacePuyo(int gridX, int gridY, GameObject Sprite) { // Lasketaan paikka, johon piirretään palikka //(Teemu, Katja + Ykä)
        float worldX = -(nX - 1) / 2f * gridDistance + gridX * gridDistance;
        float worldY = -(nY - 1) / 2f * gridDistance + gridY * gridDistance;

        Sprite.transform.position = new Vector3(worldX, worldY) + transform.position; //Piirretään palikka
    }

    public void AddBridge(int gridX, int gridY, GameObject bridge) {

    }

    public void AddTrash(int amount) {
        if (amount > 0) {
            text = GameObject.FindObjectOfType<Text>();
            text.text = "Pelaajalle " + pc.playerId + "\non tulossa \n" + amount + " roskaa";
            //        print("Pelaajalle " + pc.playerId + " roskaa tulossa " + amount);
        }
        incomingTrash += amount;
        // kerro paljonko tulossa roskaa
    }

    void PlaceOwnTrash() {
        int trash = incomingTrash;

        if (trash > nX) {
            trash = nX;
        }
        incomingTrash -= trash;
        for (int i = 0; i < trash; i++) {
            var i2 = (nextTrash + i) % nX;
            if (grid[i2][nY - 1] == PuyoType.None) {
                grid[i2][nY - 1] = PuyoType.Trash;
                var g3 = pc.generator.InstantiatePuyoSprite(PuyoType.Trash);
                float worldX = -(nX - 1) / 2f * gridDistance + i2 * gridDistance;
                float worldY = -(nY - 1) / 2f * gridDistance + pc.spawnY1 * gridDistance;
                g3.transform.position = new Vector3(worldX, worldY) + (transform.position * 1000000000);
                sprites[i2][nY - 1] = g3;
            } else {
                gm.GameOver(pc.playerId);
                return;
            }
        }
        nextTrash = (nextTrash + trash) % nX;
    }

    public IEnumerator DropMatchRemove() { //Pudotetaan tarvittaessa puyot, etsitään ryhmät ja poistetaan 4 tai enemmän samaa puyoa ryhmät.
        int trashFactor = 1; //roskakerroin
        int trash = 0; // roskaa kierroksella
        pc.enabled = false; // poistetaan playerController pois käytöstä kunnes funktio on ajettu(animaation vuoksi, peli "pauselle")
        bool removedGroups = false; //Apumuutuja, jolla seurataan miten pitkään suoritetaan do - while-lauseketta
        // Wait for animation WaitForAnimation();
        do { // Tehdään ainakin kerran, toistetaan niin kauan kuin while-kohdassa oleva ehto on tosi 
            PuyoDropStatus status = PuyoDropStatus.None;
            do {
                status = DropPuyos();
                if (status == PuyoDropStatus.DropAndStop) {
                    yield return new WaitForSeconds(dropTime);
                    audioScript.hitGroundSource.Play();
                }
                if (status == PuyoDropStatus.Drop)
                    yield return new WaitForSeconds(dropTime);
            } while (status != PuyoDropStatus.None);

            var groups = FindPuyoGroups(); //Tallennetaan muuttujaan Funktion palautusarvo, jossa on kaikki puyo-ryhmät(vähintään 1 puyo)
            var groupsToRemove = MoreThanThreeInGroups(groups); //Tallennetaan muuttujaan funktion palautusarvo, jossa on puyo-ryhmät, joissa on vähintään 4 puyoa. Kutsussa annetaan edellisen funktion palautusarvo
            int sum = 0;
            foreach (var group in groupsToRemove) {
                sum += group.Count - 3;
            }
            trash += sum * trashFactor;
            trashFactor++;
            removedGroups = groupsToRemove.Count > 0; //Muuttujan arvo on tosi niin kauan kun listassa on tietueita
            RemoveGroups(groupsToRemove);
            if (removedGroups)
                yield return new WaitForSeconds(destroyDelay);
        } while (removedGroups); // Palataan do-kohtaan niin kauan, että poistettavia ryhmiä ei enään ole.

        otherPG.AddTrash(trash); // push trash to other guy
        PlaceOwnTrash();
        // drop own trash
        bool trashToDrop;
        do {
            trashToDrop = DropPuyos() != PuyoDropStatus.None;
            if (trashToDrop)
                yield return new WaitForSeconds(dropTime);
        } while (trashToDrop);

        if (gm.winner == 0) {
            pc.enabled = true; //Palautetaan playerController toimimaan kun tämä funktio on ajettu.
        }

        yield return null;

    }

    //Katjan
    public PuyoDropStatus DropPuyos() {
        //löytää pudotettavat puyot
        for (int x = 0; x < nX; x++) {
            for (int y = 0; y < nY; ++y) {
                var puyoType = grid[x][y];
                if (puyoType != PuyoType.None) {
                    bool foundEmpty = false;
                    for (int i = y; i >= 0; i--) {
                        var emptyGrid = grid[x][i];
                        if (emptyGrid == PuyoType.None) {
                            foundEmpty = true;
                        }
                    }
                    if (foundEmpty) {
                        dropping.Add(new Vector2(x, y));
                    }
                }
            }
        }
        bool found = dropping.Count > 0;
        bool stop = false;
        while (dropping.Count > 0) {
            var p = dropping[0];
            //print("Alkutilanne: " + p.x + ", " + p.y);
            var droppingPuyo = grid[(int)p.x][(int)p.y];
            grid[(int)p.x][(int)p.y - 1] = droppingPuyo;
            grid[(int)p.x][(int)p.y] = PuyoType.None;
            var GO = sprites[(int)p.x][(int)p.y];

            StartCoroutine(AnimateDrop((int)p.x, (int)p.y, GO));

            //If there's a Puyo/ground below the dropping puyo, return true to play sound
            if (((int)p.y - 2) < 0 || grid[(int)p.x][(int)p.y - 2] != PuyoType.None) stop = true;

            sprites[(int)p.x][(int)p.y - 1] = GO;
            sprites[(int)p.x][(int)p.y] = null;
            dropping.RemoveAt(0);
        }
        if (stop) return PuyoDropStatus.DropAndStop;
        else if (found) return PuyoDropStatus.Drop;
        else return PuyoDropStatus.None;

    }

    List<List<Vector2>> FindPuyoGroups() {
        List<List<Vector2>> groups = new List<List<Vector2>>(); //tehdään kaikista puyoista ryhmä, joihin lisätään viereiset puyot, mikäli ovat samanlaisia.
        // loopataan kaikki x, y läpi alhaalta ylös
        // jokaiselle x, y:
        for (int x = 0; x < nX; x++) {
            for (int y = 0; y < nY; y++) {
                var aPuyo = grid[x][y]; //Nykyinen puyo
                if (aPuyo == PuyoType.None || aPuyo == PuyoType.Trash) //Jos ruudussa ei ole puyoa niin ei tarvitse verrata viereisiin ruutuihin
                    continue;

                bool samePuyoLeft = x > 0 && aPuyo == grid[x - 1][y]; //Sama puyo vasemmalla
                bool samePuyoDown = y > 0 && aPuyo == grid[x][y - 1]; //Sama puyo alhaalla
                bool samePuyoDownLeft = samePuyoLeft && samePuyoDown; //Sama puyo vasemmalla ja alhaalla

                if (samePuyoDownLeft) {
                    //Jos alhaalla ja vasemmalla sama puyo yhdistetään taulukot
                    List<Vector2> gl = null, gd = null;
                    foreach (var g in groups) {
                        if (g.Contains(new Vector2(x - 1, y))) {
                            gl = g;
                        }
                    }
                    foreach (var g in groups) {
                        if (g.Contains(new Vector2(x, y - 1))) {
                            gd = g;
                        }
                    }
                    gl.Add(new Vector2(x, y));
                    if (gl != gd) {
                        gl.AddRange(gd);
                        groups.Remove(gd);
                    }
                } else if (samePuyoDown) {
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
                        bool found = p.Contains(new Vector2(x - 1, y));
                        if (found) {
                            p.Add(new Vector2(x, y));
                            break;
                        }
                    }
                    //Jos ei samoja puyoja alhaalla, eikä ylhäällä niin tehdään taulukko mihin lisätään
                } else {
                    groups.Add(new List<Vector2>());
                    groups[groups.Count - 1].Add(new Vector2(x, y));
                }
            }
        }
        return groups;
    }
    //käydään annettu taulukko läpi ja tarkistetaan onko taulukossa enemmän kuin 3tietuetta
    List<List<Vector2>> MoreThanThreeInGroups(List<List<Vector2>> groups) {
        List<List<Vector2>> groupsToRemove = new List<List<Vector2>>();
        foreach (var g in groups) {
            if (g.Count > 3) {
                groupsToRemove.Add(g);
            }
        }
        return groupsToRemove; // Palautetaan taulukko, jossa on tiedot yli 3 puyoa olevista taulukoista
    }
    //Käydään läpi taulukko, jossa on poistettavat puyo-ryhmät. Poistaa peliobjectin ja muuttaa gridiin tiedon, että ko. ruudussa ei ole enään puyoa.
    void RemoveGroups(List<List<Vector2>> groupsToRemove) {
        for (int i = 0; i < groupsToRemove.Count; i++) {
            var group = groupsToRemove[i];
            while (group.Count > 0) {
                var vector = group[0];
                var GO = sprites[(int)vector.x][(int)vector.y];
                sprites[(int)vector.x][(int)vector.y] = null;
                Animator animator = GO.GetComponent<Animator>();
                animator.Play("Destruction");

                //When we destroy a block we should check if we can destroy a trash block next to it
                if (vector.x - 1 >= 0 && grid[(int)vector.x - 1][(int)vector.y] == PuyoType.Trash) {
                    GameObject trash = sprites[(int)vector.x - 1][(int)vector.y];
                    grid[(int)vector.x - 1][(int)vector.y] = PuyoType.None;
                    sprites[(int)vector.x - 1][(int)vector.y] = null;
                    Destroy(trash);
                }
                if (vector.x + 1 < nX && grid[(int)vector.x + 1][(int)vector.y] == PuyoType.Trash) {
                    GameObject trash = sprites[(int)vector.x + 1][(int)vector.y];
                    grid[(int)vector.x + 1][(int)vector.y] = PuyoType.None;
                    sprites[(int)vector.x + 1][(int)vector.y] = null;
                    Destroy(trash);
                }
                if (vector.y - 1 >= 0 && grid[(int)vector.x][(int)vector.y - 1] == PuyoType.Trash) {
                    GameObject trash = sprites[(int)vector.x][(int)vector.y - 1];
                    grid[(int)vector.x][(int)vector.y - 1] = PuyoType.None;
                    sprites[(int)vector.x][(int)vector.y - 1] = null;
                    Destroy(trash);
                }
                if (vector.y + 1 < nY && grid[(int)vector.x][(int)vector.y + 1] == PuyoType.Trash) {
                    GameObject trash = sprites[(int)vector.x][(int)vector.y + 1];
                    grid[(int)vector.x][(int)vector.y + 1] = PuyoType.None;
                    sprites[(int)vector.x][(int)vector.y + 1] = null;
                    Destroy(trash);
                }

                //We should also check if we should remove a bridge next to the destroyed block

                Destroy(GO, destroyDelay);
                group.RemoveAt(0);
                grid[(int)vector.x][(int)vector.y] = PuyoType.None;
                audioScript.destroySource.Play();
            }
        }
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

    IEnumerator AnimateDrop(int gridX, int gridY, GameObject GO) {
        //We re-use code from PlacePuyo()
        float worldX = -(nX - 1) / 2f * gridDistance + gridX * gridDistance;
        float worldY = -(nY - 1) / 2f * gridDistance + gridY * gridDistance;

        float t = 0f;
        Vector3 oldPlace = new Vector3(worldX, worldY) + transform.position;
        Vector3 newPlace = oldPlace - Vector3.up * gridDistance;

        while (t <= dropTime) {
            t += Time.deltaTime;
            GO.transform.position = Vector3.Lerp(oldPlace, newPlace, Mathf.Clamp01(t / dropTime));
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerGrid : MonoBehaviour {

    public int nX; // Leveys
    public int nY; // Korkeus
    public float destroyDelay;

    public float dropTime = 0.2f;
    public float gridDistance; //Ruutujen keskipisteiden etäisyys toisistaan

    public GameObject[] debugSprites;

    public PlayerController pc;
    public GameManager gm;
    public Audio audioScript;


    [HideInInspector]
    public List<List<PuyoType>> grid; // Lista-taulukko, johon merkitään millainen puyo on kussakin ruududussa
    [HideInInspector]
    public List<List<GameObject>> sprites; // Lista-taulukko, jossa hallinnoidaan mikä palikka on missäkin ruudussa.
    List<Vector2> dropping = new List<Vector2>(); //Lista pudotettavista palikoista


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
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(nX * gridDistance, nY * gridDistance));
    }

    public void AddPuyo(int x, int y, PuyoType puyo, GameObject Sprite) { // Funktio, jossa lisätään taulukkoon tieto puyosta. (x-koordinaatti, y-koordinaatti, millainen puyo, millainen palikka)//(Teemu, Katja + Ykä)

            grid[x][y] = puyo; //Mihin koordinaatteihin lisätään tieto millainen puyo
            sprites[x][y] = Sprite; // Mihin koordinaatteihin lisätään millainen palikka
            PlacePuyo(x, y, Sprite); //Kutsutaan funktio, jolla piirretään palikka

    }
    public IEnumerator DropMatchRemove() { //Pudotetaan tarvittaessa puyot, etsitään ryhmät ja poistetaan 4 tai enemmän samaa puyoa ryhmät.
        pc.enabled = false; // poistetaan playerController pois käytöstä kunnes funktio on ajettu(animaation vuoksi, peli "pauselle")
        bool removedGroups = false; //Apumuutuja, jolla seurataan miten pitkään suoritetaan do - while-lauseketta
        // Wait for animation WaitForAnimation();
        do { // Tehdään ainakin kerran, toistetaan niin kauan kuin while-kohdassa oleva ehto on tosi 
            bool found;
            do {
                found = DropPuyos();
                if (found)
                    yield return new WaitForSeconds(dropTime);
            } while (found);

            var groups = FindPuyoGroups(); //Tallennetaan muuttujaan Funktion palautusarvo, jossa on kaikki puyo-ryhmät(vähintään 1 puyo)
            var groupsToRemove = MoreThanThreeInGroups(groups); //Tallennetaan muuttujaan funktion palautusarvo, jossa on puyo-ryhmät, joissa on vähintään 4 puyoa. Kutsussa annetaan edellisen funktion palautusarvo
            removedGroups = groupsToRemove.Count > 0; //Muuttujan arvo on tosi niin kauan kun listassa on tietueita
            RemoveGroups(groupsToRemove); // Kutsutan funktiota, joka poistaa peliobjektin ja muuttaa grid-taulukkoon tiedon, että ruudussa ei ole enään puyoa.
                                          // if (removedGroups) {
                                          //     DropPuyos();
                                          //}
            if (removedGroups)
                yield return new WaitForSeconds(destroyDelay);
        } while (removedGroups); // Palataan do-kohtaan niin kauan, että poistettavia ryhmiä ei enään ole.

        pc.enabled = true; //Palautetaan playerController toimimaan kun tämä funktio on ajettu.
        yield return null;

    }
    //Katja, kesken
    public bool DropPuyos() {
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
        while (dropping.Count > 0) {
            var p = dropping[0];
            //print("Alkutilanne: " + p.x + ", " + p.y);
            var droppingPuyo = grid[(int)p.x][(int)p.y];
            grid[(int)p.x][(int)p.y - 1] = droppingPuyo;
            grid[(int)p.x][(int)p.y] = PuyoType.None;
            var GO = sprites[(int)p.x][(int)p.y];

            StartCoroutine(AnimateDrop((int)p.x, (int)p.y, GO));

            sprites[(int)p.x][(int)p.y - 1] = GO;
            sprites[(int)p.x][(int)p.y] = null;
            dropping.RemoveAt(0);
        }
        return found;
    }


    public void PlacePuyo(int gridX, int gridY, GameObject Sprite) { // Lasketaan paikka, johon piirretään palikka //(Teemu, Katja + Ykä)
        float worldX = -(nX - 1) / 2f * gridDistance + gridX * gridDistance;
        float worldY = -(nY - 1) / 2f * gridDistance + gridY * gridDistance;

        Sprite.transform.position = new Vector3(worldX, worldY) + transform.position; //Piirretään palikka
    }
    // Update is called once per frame
    void Update() {
        if (dropping.Count != 0) {
            //print("erisuuri");

        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)) {
            DropMatchRemove();
        }
#endif
    }

    List<List<Vector2>> FindPuyoGroups() {
        List<List<Vector2>> groups = new List<List<Vector2>>(); //tehdään kaikista puyoista ryhmä, joihin lisätään viereiset puyot, mikäli ovat samanlaisia.
        // loopataan kaikki x, y läpi alhaalta ylös
        // jokaiselle x, y:
        for (int x = 0; x < nX; x++) {
            for (int y = 0; y < nY; y++) {
                var aPuyo = grid[x][y]; //Nykyinen puyo
                if (aPuyo == PuyoType.None) //Jos ruudussa ei ole puyoa niin ei tarvitse verrata viereisiin ruutuihin
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerGrid grid;
    public GameManager gm;
    public Audio audioScript;
    public PuyoGenerator generator;


    public float spawnX1, spawnX2;
    public float spawnY1, spawnY2;
    int defaultSpawnX1 = 2, defaultSpawnX2 = 3;
    int defaultSpawnY1 = 11, defaultSpawnY2 = 11;
    public float defaultVelocity = 2f;
    public float velocity = 2f;
    public PlayerController other;
    public int playerId;
    PuyoType spawnType1, spawnType2;

    float P1PadAxis1, P1PadAxis2;
    float P2PadAxis1, P2PadAxis2;


    //the falling test sprite
    GameObject g1, g2;

    void Update() {


        //spawnaaminen        }
        if (g1 == null) {
            if (grid.grid[defaultSpawnX1][defaultSpawnY1] != PuyoType.None ||
                grid.grid[defaultSpawnX2][defaultSpawnY2] != PuyoType.None) {
                gm.GameOver(playerId);
                return;
            }
            var generated = generator.GetNextPuyos(playerId);
            spawnType1 = generated[0];
            spawnType2 = generated[1];

            g1 = generator.InstantiatePuyoSprite(spawnType1);
            g2 = generator.InstantiatePuyoSprite(spawnType2);
        }
        ///////////////////// **JOYSTICK** /////////////////////
        var P1PadHorizontal = Input.GetAxisRaw("P1PadHorizontal");
        var P1PadVertical   = Input.GetAxisRaw("P1PadVertical");
        var P2PadHorizontal = Input.GetAxisRaw("P2PadHorizontal");
        var P2PadVertical   = Input.GetAxisRaw("P2PadVertical");

        // Puyo1 siirto oikealle ja vasemmalle, kiihdytys ja paikkojen vaihto
        if (playerId == 1) {
            if (P1PadHorizontal < 0 && P1PadAxis1 != P1PadHorizontal &&
                spawnX1 > 0 && !IsThereObstacleLeft1() && spawnX2 > 0 && !IsThereObstacleLeft2()) {
                audioScript.moveSource.Play();
                spawnX1 = spawnX1 - 1;
                spawnX2 = spawnX2 - 1;
            }
            if (P1PadHorizontal > 0 && P1PadAxis1 != P1PadHorizontal &&
                spawnX1 < grid.nX - 1 && !IsThereObstacleRight1() && spawnX2 < grid.nX - 1 && !IsThereObstacleRight2()) {
                audioScript.moveSource.Play();
                spawnX1 = spawnX1 + 1;
                spawnX2 = spawnX2 + 1;
            }
            if (P1PadVertical > 0) {
                velocity = defaultVelocity * 5;
            }
            else {
                velocity = defaultVelocity;
            }
            if (P1PadVertical < 0 && P1PadAxis2 != P1PadVertical) {
                var tempSt1 = spawnType1;
                spawnType1 = spawnType2;
                spawnType2 = tempSt1;
                var tempG1 = g1;
                g1 = g2;
                g2 = tempG1;
            }
        }
        if (playerId == 2) {
            // Puyo1 siirto oikealle ja vasemmalle, kiihdytys ja kääntö
            if (P2PadHorizontal < 0 && P2PadAxis1 != P2PadHorizontal &&
                spawnX1 > 0 && !IsThereObstacleLeft1() && spawnX2 > 0 && !IsThereObstacleLeft2()) {
                audioScript.moveSource.Play();
                spawnX1 = spawnX1 - 1;
                spawnX2 = spawnX2 - 1;
            }
            if (P2PadHorizontal > 0 && P2PadAxis1 != P2PadHorizontal
                && spawnX1 < grid.nX - 1 && !IsThereObstacleRight1() && spawnX2 < grid.nX - 1 && !IsThereObstacleRight2()) {
                audioScript.moveSource.Play();
                spawnX1 = spawnX1 + 1;
                spawnX2 = spawnX2 + 1;
            }
            if (P2PadVertical > 0) {
                velocity = defaultVelocity * 5;
            }
            else {
                velocity = defaultVelocity;

                // kääntäminen
            }
            if (P2PadVertical < 0 && P2PadAxis2 != P2PadVertical) {
                var tempSt1 = spawnType1;
                spawnType1 = spawnType2;
                spawnType2 = tempSt1;
                var tempG1 = g1;
                g1 = g2;
                g2 = tempG1;
            }
        }
        P1PadAxis1 = P1PadHorizontal; //Muistetaan Padien Axis vanhat arvot
        P2PadAxis1 = P2PadHorizontal; //Muistetaan Padien Axis vanhat arvot
        P1PadAxis2 = P1PadVertical;
        P2PadAxis2 = P2PadVertical;
        ///////////////////// **KEYBOARD** ///////////////////// 
        // Puyo1 siirto oikealle ja vasemmalle, kiihdytys ja paikkojen vaihto
        if (Input.GetButtonDown("p1left") && spawnX1 > 0 && !IsThereObstacleLeft1() && spawnX2 > 0 && !IsThereObstacleLeft2() && playerId == 1) {
            audioScript.moveSource.Play();
            spawnX1 = spawnX1 - 1;
            spawnX2 = spawnX2 - 1;
        }
        if (Input.GetButtonDown("p1right") && spawnX1 < grid.nX - 1 && !IsThereObstacleRight1() && spawnX2 < grid.nX - 1 && !IsThereObstacleRight2() && playerId == 1) {
            audioScript.moveSource.Play();
            spawnX1 = spawnX1 + 1;
            spawnX2 = spawnX2 + 1;
        }
        if (Input.GetButtonDown("p1down") && playerId == 1) {
            velocity *= 5;
        }
        if (Input.GetButtonUp("p1down") && playerId == 1) {
            velocity = defaultVelocity;
        }
        // Puyo1 siirto oikealle ja vasemmalle, kiihdytys ja kääntö
        if (Input.GetButtonDown("p2left") && spawnX1 > 0 && !IsThereObstacleLeft1() && spawnX2 > 0 && !IsThereObstacleLeft2() && playerId == 2) {
            audioScript.moveSource.Play();
            spawnX1 = spawnX1 - 1;
            spawnX2 = spawnX2 - 1;
        }
        if (Input.GetButtonDown("p2right") && spawnX1 < grid.nX - 1 && !IsThereObstacleRight1() && spawnX2 < grid.nX - 1 && !IsThereObstacleRight2() && playerId == 2) {
            audioScript.moveSource.Play();
            spawnX1 = spawnX1 + 1;
            spawnX2 = spawnX2 + 1;
        }
        if (Input.GetButtonDown("p2down") && playerId == 2) {
            velocity *= 5;
        }
        if (Input.GetButtonUp("p2down") && playerId == 2) {
            velocity = defaultVelocity;
        }

        // kääntäminen
        if (Input.GetButtonDown("p1swap") && playerId == 1) {
            var tempSt1 = spawnType1;
            spawnType1 = spawnType2;
            spawnType2 = tempSt1;
            var tempG1 = g1;
            g1 = g2;
            g2 = tempG1;
        }
        if (Input.GetButtonDown("p2swap") && playerId == 2) {
            var tempSt1 = spawnType1;
            spawnType1 = spawnType2;
            spawnType2 = tempSt1;
            var tempG1 = g1;
            g1 = g2;
            g2 = tempG1;
        }



        //Tarkistus onko alhaalla jotain edessä?
        if (IsThereObstacleBelow1() && g1 != null || IsThereObstacleBelow2() && g2 != null) {

            grid.AddPuyo(Mathf.FloorToInt(spawnX1), Mathf.FloorToInt(spawnY1 + 1), spawnType1, g1);
            grid.AddPuyo(Mathf.FloorToInt(spawnX2), Mathf.FloorToInt(spawnY2 + 1), spawnType2, g2);
            Animator animator = g1.GetComponent<Animator>();
            Animator animator2 = g2.GetComponent<Animator>();
            animator.Play("Bounce");
            animator2.Play("Bounce");
            audioScript.hitGroundSource.Play();
            g1 = null;
            g2 = null;
            spawnX1 = defaultSpawnX1;
            spawnY1 = defaultSpawnY1;
            spawnX2 = defaultSpawnX2;
            spawnY2 = defaultSpawnY2;
            StartCoroutine(grid.DropMatchRemove());
            velocity = defaultVelocity;
        }

        if (!IsThereObstacleBelow1() || !IsThereObstacleBelow2()) {
            spawnY1 = spawnY1 - velocity * Time.deltaTime;
            spawnY2 = spawnY2 - velocity * Time.deltaTime; //Ohjataan spriten liikettä y-akselilla

            // piirtäminen (1)
            float worldX1 = -(grid.nX - 1) / 2f * grid.gridDistance + spawnX1 * grid.gridDistance;
            float worldY1 = -(grid.nY - 1) / 2f * grid.gridDistance + spawnY1 * grid.gridDistance;
            float worldX2 = -(grid.nX - 1) / 2f * grid.gridDistance + spawnX1 * grid.gridDistance;
            float worldY2 = -(grid.nY - 1) / 2f * grid.gridDistance + spawnY1 * grid.gridDistance;



            if (g1 != null) {
                g1.transform.position = new Vector3(worldX1, worldY1) + grid.transform.position;          //Liikutetaan spritea
                g2.transform.position = new Vector3(worldX2 + grid.gridDistance, worldY2) + grid.transform.position;
            }
        }
    }
    bool IsThereObstacleBelow1() {
        return grid.grid[(int)spawnX1][(int)spawnY1] != PuyoType.None || spawnY1 < 0;
    }
    bool IsThereObstacleBelow2() {
        return grid.grid[(int)spawnX2][(int)spawnY2] != PuyoType.None || spawnY2 < 0;
    }
    bool IsThereObstacleLeft1() {
        bool rightPuyo = false;
        if (spawnX1 > 0) {
            rightPuyo = grid.grid[(int)spawnX1 - 1][(int)spawnY1] != PuyoType.None;
        }
        return rightPuyo;
    }
    bool IsThereObstacleLeft2() {
        bool rightPuyo = false;
        if (spawnX2 > 0) {
            rightPuyo = grid.grid[(int)spawnX2 - 1][(int)spawnY2] != PuyoType.None;
        }
        return rightPuyo;
    }
    bool IsThereObstacleRight1() {
        bool leftPuyo = false;
        if (spawnX1 < grid.nX) {
            leftPuyo = spawnX1 != grid.nX - 1 && grid.grid[(int)spawnX1 + 1][(int)spawnY1] != PuyoType.None;       //Jos ei tarkisteta, että onko Puyo gridin reunalla niin seuraa ArgumentOutOfRange error kun yritetään tarkistaa onko gridin ulkopuolella este
        }
        return leftPuyo;
    }
    bool IsThereObstacleRight2() {
        bool leftPuyo = false;
        if (spawnX2 < grid.nX) {
            leftPuyo = spawnX2 != grid.nX - 1 && grid.grid[(int)spawnX2 + 1][(int)spawnY2] != PuyoType.None;       //Jos ei tarkisteta, että onko Puyo gridin reunalla niin seuraa ArgumentOutOfRange error kun yritetään tarkistaa onko gridin ulkopuolella este
        }
        return leftPuyo;
    }
}

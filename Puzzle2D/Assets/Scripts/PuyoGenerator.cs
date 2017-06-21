// Ykän mestariteos, 25 min. Ei testattu :-p

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuyoType { None = 0, Trash = 1, Puyo1 = 2, Puyo2 = 3, Puyo3 = 4 } // Millaisia puyoja meillä on olemassa

public class PuyoGenerator : MonoBehaviour {
    public int minimumInQueue = 5;

    public GameObject[] PuyoSpritePrefabs;

    List<List<PuyoType>> p1puyos;
    List<List<PuyoType>> p2puyos;

    static PuyoType[] generatorPool = { PuyoType.Puyo1, PuyoType.Puyo2, PuyoType.Puyo3 };

    void Awake() {
        InitAtLevelStart();
    }

    public void InitAtLevelStart() {
        p1puyos = new List<List<PuyoType>>();
        p2puyos = new List<List<PuyoType>>();
        GenerateEnoughNewPuyos();
    }

    public List<PuyoType> GetNextPuyos(int player) {
        var thisQ = player == 1 ? p1puyos : p2puyos;
        var temp = thisQ[0];
        thisQ.RemoveAt(0);
        GenerateEnoughNewPuyos();
        return temp;
    }

    // true = player 1, false = player 2
    public List<PuyoType> PeekNextPuyos(int player, int index) {
        return (player == 1 ? p1puyos : p2puyos)[index];
    }

    public GameObject InstantiatePuyoSprite(PuyoType pt) {
        return Instantiate(PuyoSpritePrefabs[(int)pt]);
    }

    void GenerateEnoughNewPuyos() {
        while ( p1puyos.Count < minimumInQueue ||
                p2puyos.Count < minimumInQueue) {
            var newPuyoSet = new List<PuyoType>() {
                generatorPool[Random.Range(0, generatorPool.Length)],
                generatorPool[Random.Range(0, generatorPool.Length)]
            };
            var copy = new List<PuyoType>(newPuyoSet);
            p1puyos.Add(newPuyoSet);
            p2puyos.Add(copy);
        }
    }
}

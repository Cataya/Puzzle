using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KPalikoidenVertaus : MonoBehaviour {

    public int[] blockArray1;



    void Update() {
        //        if (block == stop) {
        //        OnkoSamojaVieressa() 
        //     }
        //  if (blockArray1.lenght>= 4){
        //           blink 2 times first
        //            Destroy blocks    
        //          add black block to enemy 4-blockArroy1.lenght

    }

    void OnkoSamojaVieressa(int x, int y) {
        //onko ylhäällä sama, jos on lisätään koordinaatit taulukkoon
        // x+6 / x+1
        //onko alhaalla sama, jos on lisätään koordinaatit taulukkoon
        // x-6 / x-1
        //onko vasemmalla sama, jos on lisätään koordinaatit taulukkoon
        // x-1 / y-1
        //onko oikealla sama, jos on lisätään koordinaatit taulukkoon
        // x+1 / y +1
    }
}
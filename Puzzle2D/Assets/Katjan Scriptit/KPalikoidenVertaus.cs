using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KPalikoidenVertaus : MonoBehaviour {

	public GameObject[] cubeList;
	public string myName; //name of the cube
	public int factor=1; //factor, how many black cube need to add to the enemy's field 


	void OnCollisionEnter ( Collision col){
//		myName = collision.gameObject.name;
//		OnkoSamojaVieressa(myName); 
        //     }
        //  if (cubeList.count >= 4){
        //          blink 2 times
        //          Destroy cubes    
		//          add black cube(s) to enemy field (4-blockList.count * factor)
		//			factor +=1
		// 			if (cubeList.cout == 0){
		//			factor = 1;
		//			}

    }

	void OnkoSamojaVieressa(string myName) {
		// if (myName = col.gameObject.name){
		//		cubeList.add(GameObject);
		//}
    }
}
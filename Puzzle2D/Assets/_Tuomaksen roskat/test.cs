using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    void Start() {
        Debug.Log(Mathf.Floor(10.0F));
        Debug.Log(Mathf.Floor(10.2F));
        Debug.Log(Mathf.Floor(10.7F));
        Debug.Log(Mathf.Floor(-10.0F));
        Debug.Log(Mathf.Floor(-10.2F));
        Debug.Log(Mathf.Floor(-10.7F));
    }
}

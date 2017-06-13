using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public void LoadGame() {
            SceneManager.LoadScene("Main"); //Ladataan kenttä, jonka nimi on muuttujassa
        }
    public void ExitGame() {
        Application.Quit();
    }
}


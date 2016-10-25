using UnityEngine;
using System.Collections;

public class QuitProgram : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onButton()
    {
        Application.Quit();
    }
}

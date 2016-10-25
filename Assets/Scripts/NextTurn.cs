using UnityEngine;
using UnityEngine.UI;

public class NextTurn : MonoBehaviour {

    public Toggle playerOne;
    public Toggle playerTwo;
    public Toggle playerThree;
    public Toggle playerFour;

	// Use this for initialization
	void Start () {
        playerOne = GameObject.Find("P1Toggle").GetComponent<Toggle>();
        playerTwo = GameObject.Find("P2Toggle").GetComponent<Toggle>();
        playerThree = GameObject.Find("P3Toggle").GetComponent<Toggle>();
        playerFour = GameObject.Find("P4Toggle").GetComponent<Toggle>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnButton()
    {
        if (playerOne.isOn)
        {
            playerOne.isOn = false;
            playerTwo.isOn = true;
        }
        else if (playerTwo.isOn)
        {
            playerTwo.isOn = false;
            playerThree.isOn = true;
        }
        else if (playerThree.isOn)
        {
            playerThree.isOn = false;
            playerFour.isOn = true;
        }
        else
        {
            playerFour.isOn = false;
            playerOne.isOn = true;
        }
    }
}

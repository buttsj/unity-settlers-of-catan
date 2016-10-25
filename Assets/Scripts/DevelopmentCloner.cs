using UnityEngine;
using System.Collections;

public class DevelopmentCloner : MonoBehaviour {
    
    ArrayList deck;
    GameObject prefab;

    int currCount = 0;
    int maxCount = 25;

	// Use this for initialization
	void Start () {
        // deck is empty
        ArrayList tmpDeck = new ArrayList();
        for (int i = 0; i < 14; i++)
        {
            tmpDeck.Add("Soldier");
        }
        for (int i = 0; i < 5; i++)
        {
            tmpDeck.Add("Victory");
        }
        tmpDeck.Add("Monopoly");
        tmpDeck.Add("Monopoly");
        tmpDeck.Add("Road");
        tmpDeck.Add("Road");
        tmpDeck.Add("Year");
        tmpDeck.Add("Year");
        // deck is filled
        
        // shuffle deck
        deck = ShuffleList(tmpDeck);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Vector3.Cross(ray.direction, transform.position - ray.origin).magnitude < 2.5f)
            {
                if (currCount < maxCount)
                {
                    string card = (string)deck[0];
                    deck.RemoveAt(0);
                    prefab = (GameObject)Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z - 12), transform.rotation);
                    Destroy(prefab.GetComponent<DevelopmentCloner>());
                    prefab.transform.FindChild("CardUp").gameObject.AddComponent<card_physics>();
                    switch (card)
                    {
                        case "Soldier":
                            prefab.transform.FindChild("CardUp").transform.FindChild("CardDown").GetComponent<Renderer>().material.mainTexture = Resources.Load("Cards/soldierCard") as Texture;
                            break;
                        case "Victory":
                            prefab.transform.FindChild("CardUp").transform.FindChild("CardDown").GetComponent<Renderer>().material.mainTexture = Resources.Load("Cards/vpCard") as Texture;
                            break;
                        case "Monopoly":
                            prefab.transform.FindChild("CardUp").transform.FindChild("CardDown").GetComponent<Renderer>().material.mainTexture = Resources.Load("Cards/monoCard") as Texture;
                            break;
                        case "Road":
                            prefab.transform.FindChild("CardUp").transform.FindChild("CardDown").GetComponent<Renderer>().material.mainTexture = Resources.Load("Cards/roadCard") as Texture;
                            break;
                        case "Year":
                            prefab.transform.FindChild("CardUp").transform.FindChild("CardDown").GetComponent<Renderer>().material.mainTexture = Resources.Load("Cards/yearCard") as Texture;
                            break;
                    }
                    currCount++;
                }
                else
                {
                    gameObject.transform.FindChild("CardUp").GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    private ArrayList ShuffleList(ArrayList inputList)
    {
        ArrayList randomList = new ArrayList();
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = Random.Range(0, inputList.Count - 1); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }
        return randomList; //return the new random list
    }
}

using UnityEngine;

public class CardClone : MonoBehaviour {
    
    GameObject prefab;
    int maxCount = 19;
    int currCount = 0;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Vector3.Cross(ray.direction, transform.position - ray.origin).magnitude < 2.5f)
            {
                if (currCount <= maxCount)
                {
                    prefab = (GameObject)Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z - 12), transform.rotation);
                    Destroy(prefab.GetComponent<CardClone>());
                    prefab.transform.FindChild("CardUp").gameObject.AddComponent<card_physics>();
                    currCount++;
                }
                else
                {
                    gameObject.transform.FindChild("CardUp").GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }
}

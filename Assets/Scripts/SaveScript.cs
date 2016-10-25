using UnityEngine;

public class SaveScript : MonoBehaviour {

    cube_physics[] cubeScripts;
    tile_physics[] tileScripts;
    road_physics[] roadScripts;
    number_physics[] numberScripts;
    building_physics[] buildingScripts;

    // Use this for initialization
    void Start () {
        cubeScripts = FindObjectsOfType(typeof(cube_physics)) as cube_physics[];
        tileScripts = FindObjectsOfType(typeof(tile_physics)) as tile_physics[];
        roadScripts = FindObjectsOfType(typeof(road_physics)) as road_physics[];
        numberScripts = FindObjectsOfType(typeof(number_physics)) as number_physics[];
        buildingScripts = FindObjectsOfType(typeof(building_physics)) as building_physics[];
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnButton()
    {
        foreach (cube_physics script in cubeScripts)
        {
            script.startingX = script.x;
        }
        foreach (tile_physics script in tileScripts)
        {
            script.startingX = script.x;
            script.startingQ = script.q;
        }
        foreach (road_physics script in roadScripts)
        {
            script.startingX = script.x;
        }
        foreach (number_physics script in numberScripts)
        {
            script.startingX = script.x;
        }
        foreach (building_physics script in buildingScripts)
        {
            script.startingX = script.x;
        }
    }
}

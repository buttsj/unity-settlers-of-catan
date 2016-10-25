using UnityEngine;

public class Singleton : MonoBehaviour {

    public GameObject currObject = null;
    public static Singleton _instance;

    public GameObject SetObject
    {
        get
        {
            return currObject;
        }
        set
        {
            currObject = value;
        }
    }

    void Awake()
    {
        _instance = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBG : MonoBehaviour
{
    public GameObject BGObj;
    public float lastGeneretePositionY;

    // Start is called before the first frame update
    void Start()
    {
        lastGeneretePositionY = 40.9f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate()
    {
        
        GameObject background = Instantiate(BGObj, new Vector3(0, lastGeneretePositionY, 0), Quaternion.identity);
        lastGeneretePositionY = lastGeneretePositionY + 40.8f;
    }
    public void Reset()
    {
        lastGeneretePositionY = 40.9f;
    }
}

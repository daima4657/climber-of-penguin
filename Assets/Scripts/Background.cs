using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GManager.instance.progress - transform.position.y > 60)
        {
            Destroy(gameObject);
        }
    }
}

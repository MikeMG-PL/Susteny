using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("o")) transform.position.Set((float)(transform.position.x + 0.3), transform.position.y, transform.position.z);
        if (Input.GetKeyDown("p")) transform.position.Set((float)(transform.position.x - 0.3), transform.position.y, transform.position.z);
    }
}

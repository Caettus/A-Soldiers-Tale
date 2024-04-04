using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camswitchscript : MonoBehaviour

    
{
    public Camera camera1;
    public Camera camera2;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        { camera1.gameObject.active = false;
          camera2.gameObject.active = true; 
        } 
        else if (Input.GetMouseButtonDown(0))
        { camera1.gameObject.active = true; 
          camera2.gameObject.active = false; 
        }

    }
}

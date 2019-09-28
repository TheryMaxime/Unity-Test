using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryGestureListener : MonoBehaviour
{

    private GestureListener_0 gl;
    // Start is called before the first frame update
    void Start()
    {
        gl = Camera.main.GetComponent<GestureListener_0>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

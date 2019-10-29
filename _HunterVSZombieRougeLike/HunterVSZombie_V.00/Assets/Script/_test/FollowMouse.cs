using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    bool debugging = false;
    Vector3 lastMousePos=new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        if (debugging) {
            if (!mousePos.Equals(lastMousePos)) Debug.Log(mousePos.x + " " + mousePos.y);
        }
        transform.position = mousePos;
        lastMousePos = mousePos;
    }
}

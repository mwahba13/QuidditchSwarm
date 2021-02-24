using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isParalyzed;

    // Update is called once per frame
    void Update()
    {
        if (!isParalyzed)
        {
            if(Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.forward);
        
            if(Input.GetKey(KeyCode.A))
                transform.Translate(Vector3.left);
        
            if(Input.GetKey(KeyCode.D))
                transform.Translate(Vector3.right);
        
            if(Input.GetKey(KeyCode.S))
                transform.Translate(Vector3.back);
        
            if(Input.GetKey(KeyCode.LeftControl))
                transform.Translate(Vector3.down);
        
            if(Input.GetKey(KeyCode.Space))
                transform.Translate(Vector3.up);
        
            if(Input.GetKey(KeyCode.E))
                transform.Rotate(Vector3.up,50.0f*Time.deltaTime);
        
            if(Input.GetKey(KeyCode.Q))
                transform.Rotate(Vector3.up,-50.0f*Time.deltaTime);
        }

        
    }

    public void ParalyzePlayer()
    {
        isParalyzed = true;
    }

    public void UnparalyzePlayer()
    {
        isParalyzed = false;
    }


}

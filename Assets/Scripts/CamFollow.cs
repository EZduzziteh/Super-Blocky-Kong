using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 camoffset;
    public float rotspeed;
    public float camlag;
   // public float xoffset;
   // public float yoffset;
    //public float zoffset;
    // Start is called before the first frame update
    void Start()
    {
        camoffset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Quaternion camturnangle = Quaternion.AngleAxis(Input.GetAxis("Mouse X")*rotspeed,Vector3.up);
        camoffset = camturnangle*camoffset;
        Vector3 newPos = target.position + camoffset;
        transform.position = Vector3.Slerp(transform.position, newPos, camlag);

        transform.LookAt(target);
       // this.transform.position = new Vector3(target.transform.localPosition.x + xoffset, target.transform.localPosition.y + yoffset, target.transform.localPosition.z + zoffset); 
       // this.transform.rotation = target.transform.rotation;
    }
}

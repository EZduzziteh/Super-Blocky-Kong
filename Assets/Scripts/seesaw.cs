using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seesaw : MonoBehaviour
{
    Quaternion restposition;
    public List<GameObject> masses=new List<GameObject>();
    public float resistance = 2f;
    public float returnspeed;
    public bool backtostartpos=false;
    GameObject temptrans;
    // Start is called before the first frame update
    void Start()
    {
        temptrans = new GameObject();
        temptrans.transform.parent = this.transform;
        restposition = transform.rotation;//sets initial resting position
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float torquerot = 0;
        //rotation will be in z

        foreach (GameObject mass in masses)
        {
            if (mass)
            {
                temptrans.transform.position = mass.transform.position;


                if (temptrans.transform.localPosition.x < 0)
                {


                    //apply ccw torque
                    torquerot -= CalculateTorque(temptrans, mass.GetComponent<Rigidbody>().mass);

                }
                else
                {

                    torquerot += CalculateTorque(temptrans, mass.GetComponent<Rigidbody>().mass);

                    //apply cw torque
                }

            }
            else
            {
                masses.Remove(mass);
            }
        }

        transform.Rotate(0, 0, torquerot*Time.fixedDeltaTime/resistance, Space.Self);
       

        

        if (masses.Count <= 0&&backtostartpos)
        {
            transform.SetPositionAndRotation(transform.position, Quaternion.Lerp(transform.rotation, restposition, Time.fixedDeltaTime*returnspeed));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall"|| collision.gameObject.tag == "Ground"|| collision.gameObject.tag == "Projectile")
        {

        }
        else
        {
            masses.Add(collision.gameObject);
            if (collision.gameObject.GetComponent<KinematicInput>())
            {
                //then its a player
                collision.gameObject.GetComponent<KinematicInput>().isFalling = false;

            }

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {

        }
        else
        {
            masses.Remove(collision.gameObject);

            if (collision.gameObject.GetComponent<KinematicInput>())
            {
                //then its a player
                //collision.gameObject.GetComponent<KinematicInput>().isFalling = true;

            }


        }
       
    }


    float CalculateTorque(GameObject obj, float mass)
    {
        float torque;
        torque = mass*-9.8f*(Vector3.Distance(transform.position, obj.transform.position));//gets distance from fulcrum/CoM
        
        return torque;
    }
}

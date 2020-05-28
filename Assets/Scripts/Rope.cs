using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public bool ismoving=false;
    public bool isoccupied;
    float halfperiod=0;
    public bool clockwise=true;
    public GameObject occupant;
    public float speed=5f;
    float timer = 0;
    public Vector3 restpos;
    public float returnspeed=2f;
    public Quaternion restrot;
  
  
    // Start is called before the first frame update
    void Start()
    {
        restpos = transform.parent.position;
        restrot = transform.parent.rotation;
        halfperiod = GetPeriod()*0.5f;//sets how long the pendulum should take to go back and forth one time.
       
   
    }

    // Update is called once per frame
   
      
       
    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.gameObject.GetComponent<KinematicInput>().isswinging == false)
            {
                transform.parent.rotation = collision.gameObject.transform.rotation;
                transform.parent.Rotate(0, 90, 0);
                timer = Time.time + halfperiod;//first timer should be half of what it normally would be, since we start at the 0 mark.


                occupant = collision.gameObject;

                isoccupied = true;
                occupant.GetComponent<KinematicInput>().roperef = this;
                occupant.GetComponent<KinematicInput>().isswinging = true;
                occupant.GetComponent<KinematicInput>().swingforwardvector = occupant.transform.forward;
                occupant.GetComponent<KinematicInput>().externalForceVelocity=Vector3.zero;

                occupant.transform.parent = transform.parent;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            
            occupant.GetComponent<KinematicInput>().isswinging = false;
            occupant.transform.parent = null;
            isoccupied = false;
            clockwise = false;
        }
    }

    public void ClearOccupant()
    {
        if (clockwise)
        {
            occupant.GetComponent<KinematicInput>().swingforwardvector = -occupant.GetComponent<KinematicInput>().swingforwardvector;
        }
        else
        {

        }
        occupant.GetComponent<KinematicInput>().isswinging = false;
        //Debug.Log(occupant.GetComponent<KinematicInput>().swingforwardvector * Mathf.Abs(transform.parent.rotation.z) * Mathf.Abs(Vector3.Distance(occupant.transform.position, transform.parent.position)));
        occupant.GetComponent<KinematicInput>().externalForceVelocity += occupant.GetComponent<KinematicInput>().swingforwardvector *5f* Mathf.Abs(transform.parent.rotation.z)* Mathf.Abs(Vector3.Distance(occupant.transform.position, transform.parent.position));
        occupant.transform.parent = null;
        occupant.GetComponent<KinematicInput>().swingforwardvector = Vector3.zero;
        isoccupied = false;
      
        //transform.parent.position = restpos;
        // transform.parent.rotation = restrot;
    }

    /*private void OnTriggerEnter(Collider collision)
    {
       
       
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with rope");
            isoccupied = true;
            collision.gameObject.GetComponent<KinematicInput>().isswinging = true;
        }
    }*/

    private void FixedUpdate()
    {
        
       
        if (isoccupied)
        {
            if (timer <= Time.time)
            {
                ChangeDirection();
                timer = Time.time + halfperiod * 2f;
            }
            if (clockwise)
            {

                transform.parent.Rotate(0, 0, speed*Mathf.Abs(transform.parent.position.y - occupant.transform.position.y) * Time.fixedDeltaTime);
            }
            else
            {
                transform.parent.Rotate(0, 0, -speed * Mathf.Abs(transform.parent.position.y - occupant.transform.position.y) * Time.fixedDeltaTime);
            }
        }
        else
        {
            transform.parent.SetPositionAndRotation(Vector3.Lerp(transform.parent.position, restpos, Time.fixedDeltaTime * returnspeed), Quaternion.Lerp(transform.parent.rotation, restrot, Time.fixedDeltaTime * returnspeed));
        }

    }

    float GetPeriod()//returns period of the pendulum as a float.
                     //pendulum period equation T = 2π * √(L / g)   

    //complicated pendulum period:  T= 2π * √(I/mgD)
    {
        Debug.Log(GetComponent<CapsuleCollider>().height*transform.localScale.y);
        float t = 2f * 3.14f * Mathf.Sqrt(GetComponent<CapsuleCollider>().height*transform.localScale.y / 9.8f);
        return t;
    }

    void ChangeDirection()
    {
        
        occupant.transform.Rotate(0, 180, 0);
        if (clockwise)
        {
            clockwise = false;
        }
        else
        {
            clockwise = true;
        }
    }
}

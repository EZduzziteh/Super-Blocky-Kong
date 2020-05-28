using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//AI script that moves towards a transform specified by path gameobject(which is an array) and then 
//updates the transform it will travel towards when it reaches its destination.
public class Node_Follower_AI : MonoBehaviour
{

    public bool isinverted;
    [Tooltip("integer representing the Node in the path that the ai is travelling towards.")]
    public int current_node;
    [Tooltip("the transform of the current Node in the path that the ai is travellign towards.")]
    public Transform target;
    [Tooltip("float value representing the top speed of the ai")]
    public float maxvelocity;
    [Tooltip("float value representing the acceleration of the ai")]
    public float acceleration;
    [Tooltip("float value representing the time that an ai will wait at each patrol point")]
    public float waittime;
    public float damage = 25;
    private float waittimer;
    [Tooltip("float value representing how close the ai has to be in order to start moving to the next node.")]
    public float nextnodedistance;
   Rigidbody rb;
    [Tooltip("The path of nodes that the ai will follow")]
    public AI_Path path;


    private void Update()
    {
       
            if (Vector3.Distance(transform.position, path.waypoints[current_node].position) <= nextnodedistance)
            {

            if (!isinverted)
            {
                //handles resetting the node counter once the end of the nodes have been reached.
                if (current_node + 1 >= path.waypoints.Count)
                {

                    current_node = 0;
                    target = path.waypoints[current_node];
                }
                //handles incrementing node counter and setting the new target node.
                else if (current_node < path.waypoints.Count + 1)

                {
                    current_node++;
                    target = path.waypoints[current_node];
                }
            }
            else
            {
                if (current_node - 1 < 0)
                {
                    current_node = path.waypoints.Count - 1;
                    target = path.waypoints[current_node];
                }
                else
                {
                    current_node--;
                    target = path.waypoints[current_node];
                }
            }
                rb.velocity = Vector3.zero;

                waittimer = Time.time + waittime;
            }
        
       
       
    }
    void Start()
    {
        //gets reference to the rigidbody attached to this game object.
        rb = GetComponent<Rigidbody>();
        //Sets the initial target point
        target = path.waypoints[current_node];
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("hit");
            other.GetComponent<PlayerControl>().takedamage(damage);
            

        }else if (other.gameObject.tag == "Enemy")
        {
            if (!isinverted)
            {
                isinverted = true;
                if (current_node - 1 < 0)
                {
                    current_node = path.waypoints.Count - 1;
                    target = path.waypoints[current_node];
                }
                else
                {
                    current_node--;
                    target = path.waypoints[current_node];
                }
            }
            else
            {
                isinverted = false;
                //handles resetting the node counter once the end of the nodes have been reached.
                if (current_node + 1 >= path.waypoints.Count)
                {

                    current_node = 0;
                    target = path.waypoints[current_node];
                }
                //handles incrementing node counter and setting the new target node.
                else if (current_node < path.waypoints.Count + 1)

                {
                    current_node++;
                    target = path.waypoints[current_node];
                }
            }
        }
    }
   


    void FixedUpdate()
    {

        
       

       if(waittimer<=Time.time)
       {
            //This forces the ai to point towards its target node.
            transform.LookAt(target.transform);
            //only accelerates if max velocity has not been reached.
            if (rb.velocity.magnitude < maxvelocity)
            {
                rb.AddForce(transform.forward * acceleration);
            }

            

        }
        else
        {

           transform.rotation= Quaternion.Lerp(transform.rotation, path.waypoints[current_node].rotation, Time.deltaTime*waittime);
         
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AI_Corner_Avoider : MonoBehaviour {
    [Tooltip("Float Value for base speed of the AI")]
    public float speed;
    [Tooltip("Float Value representing reaction time to a corner")]
    public float corner_speed;
    [Tooltip("bool value representing Which direction on the track is the ai travelling")]
    public bool is_clockwise;
    Rigidbody rb;
    public Test_AI_Corner_Finder right_corner_finder;
    public Test_AI_Corner_Finder left_corner_finder;

   
    void Start () {
        //gets reference to attached 
        rb = GetComponent<Rigidbody>();
	}
	
	
	void Update () {

        //causes object to move forward  at a constant velocity.
        rb.velocity = transform.forward *speed;


        //If there is a corner detected on the left and right this will detect what way the ai should be moving, and turns based on that.
        if (left_corner_finder.is_corner_coming && right_corner_finder.is_corner_coming)
        {
            if (is_clockwise)
            {
                //turns right if travelling clockwise
                transform.Rotate(new Vector3(0, corner_speed, 0) * Time.deltaTime * speed, Space.World);
            }
            else
            {
                //turns left if travellign counterclockwise.
                transform.Rotate(new Vector3(0, -corner_speed, 0) * Time.deltaTime * speed, Space.World);
            }
        }

        //if corner detected on the left, rotate right.
        else if (left_corner_finder.is_corner_coming)
        {
            transform.Rotate(new Vector3(0, corner_speed, 0) * Time.deltaTime * speed, Space.World);
        }
        //if corner detected on the right, rotate left.
        else if (right_corner_finder.is_corner_coming)
        {
            transform.Rotate(new Vector3(0, -corner_speed, 0) * Time.deltaTime * speed, Space.World);
        }
        
	}

    
}

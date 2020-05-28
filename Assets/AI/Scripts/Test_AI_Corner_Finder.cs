using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AI_Corner_Finder : MonoBehaviour {
    //This script simply detects whether the collider box is in contact with an obstacle 
    //and toggles a bool based on whether or not it is still in contact.

    public bool is_corner_coming;
	

    private void OnTriggerEnter(Collider other)
    {
        
        is_corner_coming = true;
    }

    private void OnTriggerExit(Collider other)
    {
        is_corner_coming = false;
    }

    
}

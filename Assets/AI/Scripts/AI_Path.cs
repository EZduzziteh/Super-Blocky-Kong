using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This holds an array of transforms and draws a line between each one.
public class AI_Path : MonoBehaviour {

    public Color line_color;
    public List<Transform> waypoints = new List<Transform>();

    private void OnDrawGizmos()//Fires every update in scene view(not while running game)
    {
        Gizmos.color = line_color;
        Transform[] path_transforms = GetComponentsInChildren<Transform>();
        waypoints = new List<Transform>();

        for(int i = 0; i < path_transforms.Length; i++)
            //Loops through all transforms and if it is different than our transform, adds it to the waypoints list.
        {
            if (path_transforms[i] != transform)
            {
                waypoints.Add(path_transforms[i]);
            }
        }

        for(int i = 0; i < waypoints.Count; i++)
        {

            Vector3 current_waypoint = waypoints[i].position;
            Vector3 previous_waypoint=Vector3.zero;
            if (i > 0)
            {
            previous_waypoint = waypoints[i - 1].position;
            }else if(i==0 && waypoints.Count > 1)
            {
                previous_waypoint = waypoints[waypoints.Count - 1].position;
            }

            Gizmos.DrawLine(previous_waypoint, current_waypoint);
            Gizmos.DrawWireSphere(current_waypoint, 0.2f);
        }
    }

}

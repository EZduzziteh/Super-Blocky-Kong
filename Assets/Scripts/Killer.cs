using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
      
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("col");
            collision.gameObject.GetComponent<PlayerControl>().takedamage(100);
        }else if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<AI_PlayerChaser>())
            {
                collision.gameObject.GetComponent<AI_PlayerChaser>().takedamage();
            }else if (collision.gameObject.GetComponent<AI_Hopper>())
            {
                collision.gameObject.GetComponent<AI_Hopper>().takedamage();
            }
        }
    }
}

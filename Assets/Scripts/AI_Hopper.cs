using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Hopper : MonoBehaviour
{
    public enum states { Idle,Avoid, Wander }
    states state = states.Idle;
    public GameObject target;
    public Transform wandertarget;
    Rigidbody rb;
    MeshRenderer rend;
    AudioSource aud;
    bool isalive;
    public float walkspeed;
    public float damage = 25;
    public float health = 50;
    public float jumptimer=5;
    public float avoidrange;
    public float jumpheight;
    public float wandertimer;
    public float maxwanderdistance;
    // Start is called before the first frame update
    void Start()
    {
        isalive = true;
        aud = GetComponent<AudioSource>();
        rend = GetComponent<MeshRenderer>();
        wandertarget.transform.position = transform.position;
        rb = GetComponent<Rigidbody>();
        state = states.Wander;
    }

    private void FixedUpdate()
    {



        RaycastHit hit;
        //Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y +1.5f ,transform.position.z));

        if (isalive)
        {
            if (Physics.Linecast(transform.position, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), out hit))
            {
                //Debug.Log("hitenemy");
                if (hit.transform.gameObject.tag == "Player")
                {
                    takedamage();
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (jumptimer <= Time.time)
        {
            rb.AddForce(transform.up * jumpheight);
            jumptimer = Time.time + Random.Range(3f, 8f);
        }


        switch (state)
        {
            case states.Idle:
                if (Vector3.Distance(transform.position, target.transform.position) <= avoidrange)
                {
                    state = states.Avoid;
                }
                break;
            case states.Avoid:
                if (target.GetComponent<PlayerControl>().isalive)
                {
                    
                    if (Vector3.Distance(transform.position, target.transform.position) >= avoidrange)
                    {
                        state = states.Wander;
                    }
                    else
                    {
                       
                        transform.LookAt(2*transform.position- new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                        //rb.MovePosition(Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), Time.deltaTime * walkspeed));
                        rb.AddForce(transform.forward*walkspeed);
                    }
                }
                break;
            case states.Wander:
                if (Vector3.Distance(transform.position, target.transform.position) <= avoidrange)
                {
                    state = states.Avoid;
                }
                else if (Vector3.Distance(transform.position, wandertarget.transform.position) >= avoidrange / 2)
                {
                    transform.LookAt(new Vector3(wandertarget.transform.position.x, transform.position.y, wandertarget.transform.position.z));
                    rb.MovePosition(Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(wandertarget.transform.position.x, transform.position.y, wandertarget.transform.position.z), Time.deltaTime * walkspeed / 2));

                }

                // Debug.Log("new wander target set");

                if (wandertimer <= Time.time)
                {
                    wandertimer = Time.time + Random.Range(3, 7);
                    Vector3 newpos = new Vector3(transform.position.x + Random.Range(-maxwanderdistance, maxwanderdistance),
                                                             transform.position.y,
                                                             transform.position.z + Random.Range(-maxwanderdistance, maxwanderdistance));
                    wandertarget.transform.position = newpos;
                }




                RaycastHit testhit;
                if (Physics.Linecast(transform.position, wandertarget.transform.position, out testhit))
                {
                    Vector3 newpos = new Vector3(testhit.point.x, transform.position.y, testhit.point.z);
                    wandertarget.transform.position = newpos;

                }







                break;
        }
    }

   
    public void takedamage()
    {
        isalive = false;

        rend.enabled = false;

        aud.Play();
        Destroy(gameObject, aud.clip.length);


    }
}

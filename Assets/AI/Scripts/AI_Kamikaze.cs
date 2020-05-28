using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Kamikaze : MonoBehaviour
{

    public GameObject explosionparticle;
    public GameObject sparkparticle;
    public enum states { Idle, Attack, Wander, Destructing }
    states state = states.Idle;
    public GameObject target;
    public float blastradius;
    public Transform wandertarget;
    Rigidbody rb;
    MeshRenderer rend;
    AudioSource aud;
    bool isalive;
    public float walkspeed;
    float destructtimer=0;
    bool isselfdestructing=false;
    public float fuselength = 2f;
    public float damage = 25;
    public float health = 50;
    public float attackrange;
    public float attacktimer;
    public float attacktime;
    public float aggrorange;
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

    // Update is called once per frame
    void Update()
    {
        if (isselfdestructing)
        {
            if (destructtimer <= Time.time)
            {
                SelfDestruct();
            }
        }


        switch (state)
        {
            case states.Idle:
                if (Vector3.Distance(transform.position, target.transform.position) <= aggrorange)
                {
                    state = states.Attack;
                }
                break;
            case states.Attack:
                if (target.GetComponent<PlayerControl>().isalive)
                {
                    if (!isselfdestructing)
                    {
                        if (Vector3.Distance(transform.position, target.transform.position) <= attackrange)
                        {
                            if (attacktimer <= Time.time)
                            {

                                isselfdestructing = true;
                                destructtimer = Time.time + fuselength;
                                sparkparticle.SetActive(true);
                                state = states.Destructing;

                            }
                        }
                        else if (Vector3.Distance(transform.position, target.transform.position) >= aggrorange)
                        {
                            state = states.Wander;
                        }
                        else
                        {
                            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                            rb.MovePosition(Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), Time.deltaTime * walkspeed));
                        }
                    }
                }
                break;
            case states.Destructing:
                rb.velocity = Vector3.zero;
                break;
            case states.Wander:
                if (Vector3.Distance(transform.position, target.transform.position) <= aggrorange)
                {
                    state = states.Attack;
                }
                else if (Vector3.Distance(transform.position, wandertarget.transform.position) >= attackrange / 2)
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

    void SelfDestruct()
    {
        if (isalive)
        {
            explosionparticle.SetActive(true);
            takedamage();
            if (Vector3.Distance(target.transform.position, transform.position) <= blastradius)
            {
                if (target.transform.gameObject.tag == "Player")
                {
                    target.GetComponent<PlayerControl>().takedamage(damage);
                }
            }
            rb.velocity = Vector3.zero;
        }
    }
    public void takedamage()
    {
        isalive = false;

        rend.enabled = false;
        GetComponent<Collider>().enabled = false;
        aud.Play();
        Destroy(gameObject, aud.clip.length);


    }

}

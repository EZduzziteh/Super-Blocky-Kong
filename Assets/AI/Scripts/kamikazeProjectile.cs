using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kamikazeProjectile : MonoBehaviour
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
    float destructtimer = 0;
    bool isselfdestructing = false;
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
        target = FindObjectOfType<PlayerControl>().gameObject;
        isalive = true;
        aud = GetComponent<AudioSource>();
        rend = GetComponent<MeshRenderer>();
        
        rb = GetComponent<Rigidbody>();
    
    }
    private void OnTriggerEnter(Collider other)
    {
        SelfDestruct();
    }
    private void OnCollisionEnter(Collision collision)
    {
        SelfDestruct();
    }
    // Update is called once per frame
    void Update()
    {
        

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

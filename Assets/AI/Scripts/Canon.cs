using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public KinematicInput target;
    public float reloadtime;
    public GameObject Projectile;
    public GameObject spawnpoint;
    public float shotvelocity;
    public float shotscale=0.5f;
    public float reloadtimer=2f;
    public float range=20f;
    private void Start()
    {
        target = FindObjectOfType<KinematicInput>();
    }
    private void Update()
    {
        if (Vector3.Distance(target.transform.position, transform.position)<=range&&reloadtimer<=Time.time)
        {
            Fire();
        }

        Debug.Log(target.transform.position - target.Lastpos);
        transform.LookAt(target.targetPosition+(target.transform.position-target.Lastpos)*Vector3.Distance(transform.position,target.transform.position)*shotscale);
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(Projectile);
        bullet.transform.position = spawnpoint.transform.position;
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * shotvelocity*Vector3.Distance(transform.position, target.transform.position));
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * shotvelocity*Vector3.Distance(transform.position, target.transform.position));
        reloadtimer = Time.time + reloadtime;
        
    }
}

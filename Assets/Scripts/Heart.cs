using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public int healthvalue;
    public AudioSource aud;
    public MeshRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        rend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerControl>().health > 0&& other.gameObject.GetComponent<PlayerControl>().health< other.gameObject.GetComponent<PlayerControl>().maxhealth)
            {
                rend.enabled = false;
                GetComponent<Collider>().enabled = false;
                other.gameObject.GetComponent<PlayerControl>().health += healthvalue;
                if(other.gameObject.GetComponent<PlayerControl>().health> other.gameObject.GetComponent<PlayerControl>().maxhealth)
                {
                    other.gameObject.GetComponent<PlayerControl>().health = other.gameObject.GetComponent<PlayerControl>().maxhealth;
                }
                other.gameObject.GetComponent<PlayerControl>().healthbar.value = other.gameObject.GetComponent<PlayerControl>().health;
                aud.Play();
                Destroy(gameObject, aud.clip.length);

            }
            
        }
    }
}

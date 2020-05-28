using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int gemvalue;
    public AudioSource aud;
    public MeshRenderer rend;
    public GameObject effect;
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
            rend.enabled = false;
            other.gameObject.GetComponent<PlayerControl>().gemcount += gemvalue;
            aud.Play();
            effect.SetActive(true);
            Destroy(gameObject, aud.clip.length);
            GetComponent<Collider>().enabled = false;
            other.gameObject.GetComponent<PlayerControl>().gemtext.text = "x " + other.gameObject.GetComponent<PlayerControl>().gemcount;
        }
    }
    
}

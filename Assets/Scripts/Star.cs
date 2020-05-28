using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public int starvalue;
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
            effect.SetActive(true);
            rend.enabled = false;
            GetComponent<Collider>().enabled = false;
            other.gameObject.GetComponent<PlayerControl>().starcount += starvalue;
            other.gameObject.GetComponent<PlayerControl>().startext.text = "x " + other.gameObject.GetComponent<PlayerControl>().starcount+"/"+ other.gameObject.GetComponent<PlayerControl>().starmax;
            aud.Play();
            
            Destroy(gameObject, aud.clip.length);
         
        }
    }
}

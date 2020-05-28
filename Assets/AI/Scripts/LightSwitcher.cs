using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    bool ison;
    public List<GameObject> lights;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        if (!ison)
        {
            foreach (GameObject light in lights)
            {
                light.gameObject.SetActive(true);
            }
        }
    }
}

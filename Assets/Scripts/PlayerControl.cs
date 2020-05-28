using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
 
    public bool isswinging = false;
    public AudioClip hurtsound;
    public AudioClip deathsound;
    public AudioClip jumpsound;
    public AudioSource aud;
    public GameObject deathtext;
    public Text livestext;
    public int starcount = 0;
    public int starmax = 0;
    public int gemcount = 0;
    public Text gemtext;

    public Text startext;
    public Image starimage;
    public Slider healthbar;
    public Text healthtext;
    Color colorStartMajorDamage = Color.red;
    Color colorNoDamage = Color.blue;
    Color colorStartMediumDamage = Color.yellow;
    float damageDisplayDuration = 0.8f;
    public float health = 100f;
    public float maxhealth = 100f;
    Renderer rend;
    public KinematicInput input;

    public bool isalive;
    public float currentMajorDamageTimer = 0.0f;
    public float currentMediumDamageTimer = 0.0f;

    public float mediumDamageCollisionForce = 200.0f;
    public float majorDamageCollisionForce = 350.0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Star star in FindObjectsOfType<Star>())
        {
            starmax++;
        }

        if (startext)
        {
            startext.text = "x " + starcount + "/" + starmax;
        }

        input = GetComponent<KinematicInput>();
        aud = GetComponent<AudioSource>();
        health = maxhealth;
        if (healthbar)
        {
            healthbar.value = health;
        }
        isalive = true;
        rend = GetComponent<Renderer>();
        colorNoDamage = rend.material.color;

        currentMajorDamageTimer = 0.0f;
        currentMediumDamageTimer = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reloadscene();
        }




        if (currentMajorDamageTimer > 0.0f)
        {
            float lerp = Mathf.PingPong(Time.time, currentMajorDamageTimer) / currentMajorDamageTimer;
            rend.material.color = Color.Lerp(colorStartMajorDamage, colorNoDamage, lerp);

            currentMajorDamageTimer -= Time.deltaTime;
        }
        else if (currentMediumDamageTimer > 0.0f)
        {
            float lerp = Mathf.PingPong(Time.time, currentMediumDamageTimer) / currentMediumDamageTimer;
            rend.material.color = Color.Lerp(colorStartMediumDamage, colorNoDamage, lerp);

            currentMediumDamageTimer -= Time.deltaTime;
        }
        else
        {
            rend.material.color = colorNoDamage;
        }

    }

    public void Win()
    {
        SceneManager.LoadScene("Win");
    }
    void Die()
    {
        aud.clip = deathsound;
        aud.Play();
        deathtext.SetActive(true);
        isalive = false;

    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    void reloadscene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void takedamage(float damage)
    {

        if (isalive)
        {

            aud.clip = hurtsound;
            aud.Play();

            health -= damage;
            healthbar.value = health;
            if (damage >= 20f)
            {
                currentMajorDamageTimer = damageDisplayDuration;
            }
            else
            {
                currentMediumDamageTimer = damageDisplayDuration;
            }
            
            if (health <= 0)
            {
                health = 0;
                healthbar.value = health;
                Die();


            }



        }
    }
}
 

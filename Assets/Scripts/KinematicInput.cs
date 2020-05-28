using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KinematicInput : MonoBehaviour
{
    public Vector3 Lastpos;
    public Vector3 swingforwardvector = Vector3.zero;
    public Rope roperef = null;
    float inputdelaytimer;
    public  float walljumpinputdelay;
    public GameObject jumpparticlesys;
    public GameObject groundparticlesys;
    public GameObject walljumpparticlesys;
    public Vector3 targetPosition;
    // Horizontal movement parameters

    private float speed = 10.0f;
    public float airspeed = 6.0f;
    public float walljumpairspeed;
    public float groundspeed = 10.0f;
    
    public float minimumjumpduration;
    // Jump and Fall parameters
    public float maxJumpSpeed = 1.5f;
    public float maxdoublejumpspeed=1f;
    public float maxregularjumpspeed=1.5f;
    public float maxwalljumpspeed = 10f;
    public float maxFallSpeed = -2.2f;
    public float timeToMaxJumpSpeed = 0.2f;
    public float deccelerationDuration = 0.0f;
    public float maxJumpDuration = 1.2f;
    //Walljump Params
    bool canwalljump;
    bool hasdoublejumped;
    bool candoublejump;
    bool haswalljumped;
 

  
    // Jump and Fall helpers
    public float fallSpeed;
    public float hoverspeed;
    public float hoverlength;
    public float gravityAcceleration = -9.8f;
    public float redfallheight;
    public float yellowfallheight;
    private bool jumpStartRequest = false;
    private bool jumpRelease = false;
    private bool isMovingUp = false;
    public bool isFalling = false;
    private float currentJumpDuration = 0.0f;
    private float hovertimer = 0;
    private Vector3 fallstartpos = Vector3.zero;
    public float headSearchLength = 1.0f;
    public float groundSearchLength = 0.6f;
    RaycastHit currentGroundHit;
    RaycastHit currentCeilingHit;
    public Vector3 walljumpvector = Vector3.zero;
  

    //ExternalForces
    public Vector3 externalForceVelocity = Vector3.zero;


    // Rotation Parameters
    float angleDifferenceForward = 0.0f;

    // Components and helpers
    Rigidbody rigidBody;
    Vector2 input=Vector2.zero;
    Vector3 playerSize=Vector3.zero;
    private PlayerControl playercon;
    public GameObject stepdetector;
    private GameObject lasthitwall;
    private Camera cam;

    // Debug configuration
    public GUIStyle myGUIStyle;

    public bool isswinging = false;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerSize = GetComponent<Collider>().bounds.size;
    }

    void Start()
    {
        candoublejump = true;
        cam = FindObjectOfType<Camera>();
        playercon = GetComponent<PlayerControl>();
        jumpStartRequest = false;
        jumpRelease = false;
        isMovingUp = false;
        isFalling = false;
    }

    void Update()
    {
        if (isswinging && Input.GetKeyDown(KeyCode.Space))
        {
            roperef.ClearOccupant();

            hasdoublejumped = false;
           
        }
        if (isFalling)//update fall speed if falling
        {
            if (hovertimer <= Time.time)
            {
                maxFallSpeed = fallSpeed;

            }
            else
            {
                maxFallSpeed = hoverspeed;
            }
        }

        if (isOnGround())
        {
           //reset horizontal speed on ground
            speed = groundspeed;
            //Reset abilities when ground is hit.
            candoublejump = true;
            hasdoublejumped=false;
            canwalljump = false;
            haswalljumped = false;


        }
        else if (haswalljumped)
        {
            speed = walljumpairspeed;
        }
        else
        {
            //change horizontal speed in air
                
                speed = airspeed;
        }

        if (playercon.isalive&&inputdelaytimer<=Time.time)
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
        }
        else
        {
            //make inputs 0 when dead
            input.x = 0;
            input.y = 0;
        }

        if (Mathf.Abs(input.x) > 0.25 || Mathf.Abs(input.y) > 0.25)//check if there is input
        {
            externalForceVelocity = Vector3.zero;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpStartRequest = true;
            jumpRelease = false;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumpRelease = true;
            jumpStartRequest = false;
        }
    }

    


    void StartFalling()
    {
        if (!isFalling)
        {
            //update hover timer
            hovertimer = Time.time + hoverlength;
            //Get Jump Apex to calculate fall height and damage 
            fallstartpos = transform.position;
            candoublejump = true;
        }
       
        //Set falling variables
        isMovingUp = false;
        isFalling = true;
        currentJumpDuration = 0.0f;
        jumpRelease = false;
    }

    void FixedUpdate()
    {

        // Calculate horizontal movement
        Vector3 movement = cam.transform.right * input.x * speed * Time.deltaTime;
        // Vector3 movement = Vector3.right * input.x * speed * Time.deltaTime; 
        // movement += Vector3.forward * input.y * speed * Time.deltaTime;
        movement += cam.transform.forward* input.y * speed * Time.deltaTime;
        movement.y = 0.0f;
        targetPosition = rigidBody.position + movement+externalForceVelocity*Time.deltaTime;

        // Calculate Vertical movement
        float targetHeight = 0.0f;

        if (!isMovingUp && jumpStartRequest && isOnGround())//regular jump
        {
            if (maxJumpSpeed != maxregularjumpspeed)//Set proper jump speed
            {
                maxJumpSpeed = maxregularjumpspeed;
            }
            isMovingUp = true;
            jumpStartRequest = false;
            currentJumpDuration = 0.0f;
           
        }
      
        if (canwalljump&&jumpStartRequest)//wall jump
        {
            inputdelaytimer = Time.time + walljumpinputdelay;
            
                if (maxJumpSpeed != maxwalljumpspeed)//set proper wall jump speed
                {
                    maxJumpSpeed = maxwalljumpspeed;
                }
            transform.rotation = Quaternion.LookRotation(walljumpvector, Vector3.up);
            //apply external force away from wall normal (walljumpvector is assigned in fixedupdate with the forwardhit raycast, should be normal of the surface of the wall)
            if (haswalljumped == false)
            {
                externalForceVelocity += walljumpvector * 5f;
                haswalljumped = true;
                isMovingUp = true;
                jumpStartRequest = false;
                currentJumpDuration = 0.0f;
                walljumpparticlesys.GetComponent<ParticleSystem>().Play();
            }

            //rotate player to face away from the wall

                
               
            
           

         
        }
        else if (candoublejump && jumpStartRequest)//double jump
        {
            if (!hasdoublejumped)
            {
                
                if (maxJumpSpeed != maxdoublejumpspeed)//set proper double jump speed
                {
                    maxJumpSpeed = maxdoublejumpspeed;
                }
                candoublejump = false;
                isMovingUp = true;
                jumpStartRequest = false;
                currentJumpDuration = 0.0f;
                isFalling = false;
                hasdoublejumped = true;
                jumpparticlesys.GetComponent<ParticleSystem>().Play();
            }
        }

        if (isMovingUp)
        {
            if ((jumpRelease || currentJumpDuration >= maxJumpDuration)&&currentJumpDuration>=minimumjumpduration)
            {

                StartFalling();
            }
            else
            {
                float currentYpos = rigidBody.position.y;
                float newVerticalVelocity = maxJumpSpeed + gravityAcceleration * Time.deltaTime;
                targetHeight =  currentYpos + (newVerticalVelocity * Time.deltaTime) + (0.5f * maxJumpSpeed * Time.deltaTime * Time.deltaTime);
                
                currentJumpDuration += Time.deltaTime;
            }
        }
        else if (!isOnGround())
        {
            
            StartFalling();
        }

        if (isFalling)
        {
            if (isOnGround())
            {

               
               // Debug.Log(fallstartpos.y - transform.position.y);
                // End of falling state. No more height adjustments required, just snap to the new ground position
                if (fallstartpos.y - transform.position.y > redfallheight)
                {
                    //playercon.currentMajorDamageTimer = 0.4f;
                    playercon.takedamage(20.0f);
                   
                    groundparticlesys.GetComponent<ParticleSystem>().Play();
                    //Debug.Log("Trigger red");
                }
                else if (fallstartpos.y - transform.position.y > yellowfallheight)
                {
                    //playercon.currentMediumDamageTimer = 0.4f;
                    playercon.takedamage(10.0f);
                    groundparticlesys.GetComponent<ParticleSystem>().Play();
                    //Debug.Log("Trigger yellow");
                }
                else
                {
                    
                   // Debug.Log("no damage");
                }

                

               
                isFalling = false;
                externalForceVelocity = Vector3.zero;
                targetHeight = currentGroundHit.point.y + (0.5f * playerSize.y);
            }
            else
            {
                
                    float currentYpos = rigidBody.position.y;
                    float currentYvelocity = rigidBody.velocity.y;

                    float newVerticalVelocity = maxFallSpeed + gravityAcceleration * Time.deltaTime;
                    targetHeight = currentYpos + (newVerticalVelocity * Time.deltaTime) + (0.5f * maxFallSpeed * Time.deltaTime * Time.deltaTime);
                
            }
        }

        if ( targetHeight > Mathf.Epsilon)
        {
            // Only required if we actually need to adjust height
            targetPosition.y = targetHeight;
        }

        // Calculate new desired rotation
        Vector3 movementDirection = targetPosition - rigidBody.position;
        movementDirection.y = 0.0f;
        movementDirection.Normalize();

        Vector3 currentFacingXZ = transform.forward;
        currentFacingXZ.y = 0.0f;

        angleDifferenceForward = Vector3.SignedAngle(movementDirection, currentFacingXZ, Vector3.up);
        Vector3 targetAngularVelocity = Vector3.zero;
        targetAngularVelocity.y = angleDifferenceForward * Mathf.Deg2Rad;

        Quaternion syncRotation = Quaternion.identity;
        syncRotation = Quaternion.LookRotation(movementDirection);

        //canwalljump = false;
        RaycastHit targetposhit;//cast ray in travel dirction
        if (Physics.Linecast(transform.position, targetPosition, out targetposhit))
        {
            if (targetposhit.transform.gameObject.tag == "Bouncy")
            {
                Vector3 inputForReflect = targetposhit.point - rigidBody.position;
                targetPosition = rigidBody.position + Vector3.Reflect(inputForReflect, targetposhit.normal);
                // Debug.Log("set target pos");
            }else if(targetposhit.transform.gameObject.tag == "Win"&&playercon.starcount>=playercon.starmax)
            {
                playercon.LoadNextLevel();
            }else if(targetposhit.transform.gameObject.tag == "Panel")
            {
                targetposhit.transform.gameObject.GetComponent<ControlPanel>().Destroytarget();
                targetposhit.transform.gameObject.GetComponent<Collider>().enabled = false;
            }
          

        }


            RaycastHit forwardHit;//cast ray in player forward direction
        
        if(Physics.Raycast(transform.position, transform.forward, out forwardHit, 1f)) { 
            
             if(forwardHit.transform.gameObject.tag == "Wall")
            {

                if (forwardHit.transform.gameObject != lasthitwall)
                {
                    externalForceVelocity = Vector3.zero;
                    lasthitwall = forwardHit.transform.gameObject;
                }
                
                
               
                isFalling = false;
                canwalljump = true;
                haswalljumped = false;
                Debug.DrawRay(forwardHit.point, forwardHit.normal, Color.red, 0.5f);
                walljumpvector = forwardHit.normal;
                targetPosition.x = rigidBody.position.x;
                targetPosition.z = rigidBody.position.z;
               
            }
           
        }



        //Debug.DrawLine(transform.position, targetPosition);

        

        // Finally, update RigidBody   
        
        RaycastHit stephit;
            if (Physics.Linecast(transform.position, stepdetector.transform.position, out stephit))
            {
                if (stephit.collider.gameObject.tag == "Wall")
                {

                }
                else if (stephit.collider.gameObject.tag == "Enemy")
                {

                }
                else
                {
                
                  
                    
                    targetPosition.y += 0.15f; 
           
            }
            }




        Lastpos = transform.position;
        rigidBody.MovePosition(targetPosition);
        
        if (movement.sqrMagnitude > Mathf.Epsilon )
        {
            // Currently we only update the facing of the character if there's been any movement
            rigidBody.MoveRotation(syncRotation);
        }
    }

    
    private bool isHitHead()
    {
        Vector3 lineStart = transform.position;
        Vector3 vectorToSearch = new Vector3(lineStart.x, lineStart.y + headSearchLength, lineStart.z);

        Debug.DrawLine(lineStart, vectorToSearch);

        return Physics.Linecast(lineStart, vectorToSearch, out currentCeilingHit);
    }
    private bool isOnGround()
    {
        Vector3 lineStart = transform.position;
        Vector3 vectorToSearch = new Vector3(lineStart.x, lineStart.y - groundSearchLength, lineStart.z);

        Debug.DrawLine(lineStart, vectorToSearch);

        return Physics.Linecast(lineStart, vectorToSearch, out currentGroundHit);
    }



  

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            canwalljump = true;

        }else if (collision.gameObject.tag == "Win")
        {
            playercon.Win();
        }
       
    }

    
}

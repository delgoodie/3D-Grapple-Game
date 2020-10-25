using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public enum GrenadeType { GRAVITY, VISIBILITY, FREEZE, TELEPORT, HEALTH };
    public GameObject gun;
    public GameObject grapple;

    public GrappleScript grappleScript;
    public GunScript gunScript;
    public GrenadeManagerScript grenadeManagerScript;


    public float jumpForce = 3;
    public bool gunTrigger;
    public bool grappleTrigger;
    public bool jump;
    public GrenadeType grenadeType = GrenadeType.VISIBILITY;
    public int health;
    public float grenadeCoolDown = 2f;

    private float grenadeCoolDownTimer;




    void Start()
    {
        gun = transform.GetChild(0).gameObject;
        grapple = transform.GetChild(1).gameObject;

        gunScript = gun.GetComponent<GunScript>();
        grappleScript = grapple.GetComponent<GrappleScript>();
        grenadeManagerScript = GameObject.Find("Grenade Manager").GetComponent<GrenadeManagerScript>();

        gunTrigger = false;
        grappleTrigger = false;
        jump = false;

        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        gunScript.shootHandler(grappleScript, gunTrigger);
        grappleScript.grappleHandler(grappleTrigger, gunScript.inactive, gun);
        if (grappleTrigger) grappleTrigger = false;
        if (gunTrigger) gunTrigger = false;

        if (health <= 0) Destroy(gameObject);
        grenadeCoolDownTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        grappleScript.grappleHandlerFixed(gameObject, gun);
        jumpHandler();
    }




    void OnCollisionEnter(Collision collision)
    {
        if (grappleScript.grappleState == GrappleScript.GrappleState.REELING && collision.collider.attachedRigidbody.Equals(grappleScript.attachedRb) && collision.impulse.magnitude > 50f)
        {
            grappleScript.grappleState = GrappleScript.GrappleState.RETURNING;
        }
        if (tag == "Bot")
        {
            GetComponent<BotScript>().hitWall();
        }
    }

    public void tossGrenade()
    {
        if (grenadeCoolDownTimer > grenadeCoolDown)
        {
            switch (grenadeType)
            {
                case GrenadeType.GRAVITY:
                    grenadeManagerScript.createGravityGrenade(transform.position + transform.forward * 5, transform.forward);
                    break;
                case GrenadeType.FREEZE:
                    grenadeManagerScript.createFreezeGrenade(transform.position + transform.forward * 5, transform.forward);
                    break;
                case GrenadeType.TELEPORT:
                    grenadeManagerScript.createTeleportGrenade(transform.position + transform.forward * 5, transform.forward, gameObject);
                    break;
                case GrenadeType.VISIBILITY:
                    grenadeManagerScript.createVisibilityGrenade(transform.position + transform.forward * 5, transform.forward);
                    break;
                case GrenadeType.HEALTH:
                    grenadeManagerScript.createHealthGrenade(transform.position + transform.forward * 5, transform.forward);
                    break;
            }
            grenadeCoolDownTimer = 0;
        }
    }

    public void jumpHandler()
    {
        if (jump)
        {
            LayerMask mask = LayerMask.GetMask("Map");
            if (Physics.OverlapBox(transform.position - transform.up * 5, new Vector3(1.5f, 3f, 1.5f), transform.rotation, mask).Length != 0)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }

        }
    }


    public void teleport(Vector3 position)
    {
        grappleScript.grappleState = GrappleScript.GrappleState.REST;
        transform.position = position;
    }
}
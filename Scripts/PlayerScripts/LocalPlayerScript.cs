using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerScript))]
[RequireComponent(typeof(InputScript))]
public class LocalPlayerScript : MonoBehaviour
{
    public GameObject cam;
    public InputScript inputScript;
    public PlayerScript playerScript;
    public GunScript gunScript;
    public GrappleScript grappleScript;
    private float xRotation;
    private float rotationScalar = 150f;
    private float xRotDifference;
    public float forwardTorque = 10f;
    public float sidewaysTorque = 10f;

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        inputScript = GetComponent<InputScript>();
        gunScript = transform.GetComponentInChildren<GunScript>();
        grappleScript = transform.GetComponentInChildren<GrappleScript>();
        xRotation = 0f;
    }

    void Update()
    {
        if (inputScript.spaceKey) playerScript.jump = true; else playerScript.jump = false;
        if (inputScript.leftClick) playerScript.gunTrigger = true; else playerScript.gunTrigger = false;
        if (inputScript.rightClick) playerScript.grappleTrigger = true; else playerScript.grappleTrigger = false;
        if (inputScript.rKey) { gunScript.reloadHandler(grappleScript); }
        grenadeHandler();
    }

    private void grenadeHandler()
    {
        if (inputScript.oneKey)
        {
            playerScript.grenadeType = PlayerScript.GrenadeType.GRAVITY;
            playerScript.tossGrenade();
        }
        if (inputScript.twoKey)
        {
            playerScript.grenadeType = PlayerScript.GrenadeType.VISIBILITY;
            playerScript.tossGrenade();
        }
        if (inputScript.threeKey)
        {
            playerScript.grenadeType = PlayerScript.GrenadeType.TELEPORT;
            playerScript.tossGrenade();
        }
        if (inputScript.fourKey)
        {
            playerScript.grenadeType = PlayerScript.GrenadeType.HEALTH;
            playerScript.tossGrenade();
        }
        if (inputScript.fiveKey)
        {
            playerScript.grenadeType = PlayerScript.GrenadeType.FREEZE;
            playerScript.tossGrenade();
        }
    }

    void FixedUpdate()
    {
        rotationHandler();
    }

    void rotationHandler()
    {
        if (inputScript.aKey) GetComponent<Rigidbody>().AddTorque(transform.forward * sidewaysTorque * rotationScalar * Time.fixedDeltaTime, ForceMode.Force);
        if (inputScript.dKey) GetComponent<Rigidbody>().AddTorque(-transform.forward * sidewaysTorque * rotationScalar * Time.fixedDeltaTime, ForceMode.Force);

        float xOff = 0.0f;
        //if (inputScript.wKey && cam.transform.localRotation.x < Mathf.PI / 5) xOff = -90f * Time.fixedDeltaTime;
        //else if (inputScript.sKey && cam.transform.localRotation.x > -Mathf.PI / 5) xOff = 90f * Time.fixedDeltaTime;


        xRotation = -inputScript.mouseY;
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * Quaternion.Euler(xRotation + (xRotDifference - prevXRotDifference), 0f, 0f), 0.5f);
        transform.localRotation *= Quaternion.Euler(xRotation + xOff, 0, 0); // + (xRotDifference - prevXRotDifference)
        cam.transform.localRotation *= Quaternion.Euler(-xOff, 0, 0);
        transform.Rotate(0, inputScript.mouseX, 0);
    }

}

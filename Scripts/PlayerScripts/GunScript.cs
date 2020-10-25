using UnityEngine;
using UnityEngine.Events;
public class GunScript : MonoBehaviour
{
    public float shotDuration = 30;
    public float gunXrotation = 0;
    public float gunXrotVelocity = 0;
    public ParticleSystem gunParticle;
    public bool inactive;
    public UnityEvent hitEvent;

    public int ammo;

    void Start()
    {
        gunParticle = Instantiate(gunParticle, Vector3.zero, Quaternion.identity);

        ammo = 3;
    }

    public void reloadHandler(GrappleScript grappleScript)
    {
        if (gunXrotVelocity == 0 && (grappleScript.grappleState == GrappleScript.GrappleState.REST || grappleScript.grappleState == GrappleScript.GrappleState.REELING) && ammo < 3)
        {
            gunXrotVelocity = 100;
            ammo = 3;
        }
    }

    public void shootHandler(GrappleScript grappleScript, bool shoot)
    {
        if (gunXrotation < 0)
        {
            gunXrotVelocity += .1f;
            inactive = false;
        }
        else
        {
            gunXrotVelocity = 0;
            inactive = true;
        }
        if (shoot && gunXrotVelocity == 0 && (grappleScript.grappleState == GrappleScript.GrappleState.REST || grappleScript.grappleState == GrappleScript.GrappleState.REELING) && ammo > 0)
        {
            ammo--;
            Physics.Raycast(transform.position + transform.forward, transform.forward, out RaycastHit raycastHit);
            gunParticle.transform.position = raycastHit.point;
            if (raycastHit.collider.tag == "Obstacle")
            {
                Destroy(raycastHit.collider.gameObject);
            }
            else if (raycastHit.collider.tag == "LocalPlayer" || raycastHit.collider.tag == "Bot")
            {
                hitEvent.Invoke();
                if (transform.parent.tag == "Bot")
                {
                    bool hitTeam = raycastHit.collider.GetComponent<BotScript>().spawnIndex < 5;
                    bool myTeam = transform.parent.GetComponent<BotScript>().spawnIndex < 5;
                    if (hitTeam != myTeam)
                    {
                        raycastHit.collider.gameObject.GetComponent<PlayerScript>().health--;
                    }
                }
            }
            gunParticle.Play();
            gunXrotVelocity = -shotDuration;
        }
        gunXrotation += gunXrotVelocity;
        transform.localRotation = Quaternion.Euler(new Vector3(gunXrotation, 0, 0));
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeManagerScript : MonoBehaviour
{
    [Header("Gravity Grenade")]
    public GameObject gravityGrenade;
    public float gLifetime;
    public float gActivationTime;
    public float gRadius;
    public float gEventRadius;
    public float gForce;
    [Space]
    [Header("Visibility Grenade")]
    public GameObject visibilityGrenade;
    public float vLifetime;
    public Material vVisbilityMat;
    [Space]
    [Header("Teleport Grenade")]
    public GameObject teleportGrenade;
    public float tLifetime;
    [Space]
    [Header("Health Grenade")]
    public GameObject healthGrenade;
    public float hLifetime;
    public float hActivationTime;
    public float hRadius;
    [Space]
    [Header("Freeze Grenade")]
    public GameObject freezeGrenade;
    public float fLifetime;
    public float fDuration;
    public float fRadius;
    public float fDrag;
    public Material fActiveMat;


    public void createGravityGrenade(Vector3 position, Vector3 direction)
    {
        GameObject grenade = Instantiate(gravityGrenade, position, Quaternion.identity);
        GravityGrenadeScript grenadeScript = grenade.GetComponent<GravityGrenadeScript>();
        grenadeScript.lifetime = gLifetime;
        grenadeScript.activationTime = gActivationTime;
        grenadeScript.radius = gRadius;
        grenadeScript.eventRadius = gEventRadius;
        grenadeScript.force = gForce;
        grenadeScript.toss(direction * 20);
    }

    public void createVisibilityGrenade(Vector3 position, Vector3 direction)
    {
        GameObject grenade = Instantiate(visibilityGrenade, position, Quaternion.identity);
        VisibilityGrenadeScript grenadeScript = grenade.GetComponent<VisibilityGrenadeScript>();
        grenadeScript.lifetime = vLifetime;
        grenadeScript.visibilityMat = vVisbilityMat;
        grenadeScript.toss(direction * 20);
    }

    public void createTeleportGrenade(Vector3 position, Vector3 direction, GameObject player)
    {
        GameObject grenade = Instantiate(teleportGrenade, position, Quaternion.identity);
        TeleportGrenadeScript grenadeScript = grenade.GetComponent<TeleportGrenadeScript>();
        grenadeScript.lifetime = tLifetime;
        grenadeScript.player = player;
        grenadeScript.toss(direction * 20);
    }

    public void createHealthGrenade(Vector3 position, Vector3 direction)
    {
        GameObject grenade = Instantiate(healthGrenade, position, Quaternion.identity);
        HealthGrenadeScript grenadeScript = grenade.GetComponent<HealthGrenadeScript>();
        grenadeScript.lifetime = hLifetime;
        grenadeScript.activationTime = hActivationTime;
        grenadeScript.radius = hRadius;
        grenadeScript.toss(direction * 20);
    }


    public void createFreezeGrenade(Vector3 position, Vector3 direction)
    {
        GameObject grenade = Instantiate(freezeGrenade, position, Quaternion.identity);
        FreezeGrenadeScript grenadeScript = grenade.GetComponent<FreezeGrenadeScript>();
        grenadeScript.lifetime = fLifetime;
        grenadeScript.duration = fDuration;
        grenadeScript.radius = fRadius;
        grenadeScript.freezeDrag = fDrag;
        grenadeScript.activeMat = fActiveMat;
        grenadeScript.toss(direction * 20);
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportGrenadeScript : MonoBehaviour
{
    public GameObject player;
    public float lifetime;
    public float time;


    void Start()
    {
        time = 0;
    }

    public void toss(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (time > lifetime)
        {
            player.GetComponent<PlayerScript>().teleport(transform.position);
            Destroy(gameObject);
        }
        time += Time.fixedDeltaTime;
    }
}

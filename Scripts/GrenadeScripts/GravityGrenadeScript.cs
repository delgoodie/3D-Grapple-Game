using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGrenadeScript : MonoBehaviour
{
    public float lifetime;
    public float activationTime;
    public float time;

    public float radius;
    public float eventRadius;
    public float force;
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
        if (time > activationTime && time < lifetime)
        {
            LayerMask mask = LayerMask.GetMask("Player", "Map");
            Collider[] bodies = Physics.OverlapSphere(transform.position, radius, mask);
            for (int i = 0; i < bodies.Length; i++)
            {
                Vector3 bodyForce = (transform.position - bodies[i].transform.position).normalized * force;
                bodies[i].attachedRigidbody.AddForce(bodyForce);
                Debug.DrawLine(transform.position, bodies[i].transform.position, Color.green, Time.fixedDeltaTime);
            }

            LayerMask playerMask = LayerMask.GetMask("Player");
            Collider[] players = Physics.OverlapSphere(transform.position, eventRadius, playerMask);
            for (int i = 0; i < players.Length; i++)
            {
                Destroy(players[i].gameObject, 0.1f);
            }

        }
        else if (time > lifetime)
        {
            Destroy(gameObject);
        }
        time += Time.fixedDeltaTime;
    }
}

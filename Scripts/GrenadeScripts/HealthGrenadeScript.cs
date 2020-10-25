using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGrenadeScript : MonoBehaviour
{
    public float lifetime;
    public float activationTime;
    public float time;
    public float radius;

    void Start()
    {
        time = 0;
    }

    public void toss(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

    void Update()
    {
        if (time > activationTime && time < lifetime)
        {
            LayerMask playerMask = LayerMask.GetMask("Player");
            Collider[] players = Physics.OverlapSphere(transform.position, radius, playerMask);
            for (int i = 0; i < players.Length; i++)
            {
                Debug.Log("Health++");
                players[i].GetComponent<PlayerScript>().health++;
                Debug.DrawLine(transform.position, players[i].transform.position, Color.green, Time.deltaTime);
                Destroy(gameObject);
            }

        }
        else if (time > lifetime)
        {
            Destroy(gameObject);
        }
        time += Time.deltaTime;
    }


    void OnCollisionEnter(Collision collision)
    {
        LayerMask playerMask = LayerMask.GetMask("Player");
        Collider[] players = Physics.OverlapSphere(transform.position, radius, playerMask);
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("Health++");
            players[i].GetComponent<PlayerScript>().health++;
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }

}

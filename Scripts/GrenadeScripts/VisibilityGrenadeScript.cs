using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityGrenadeScript : MonoBehaviour
{
    public float lifetime;
    public float time;
    public Material visibilityMat;


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
        if (time > lifetime) Destroy(gameObject);
        time += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<MeshRenderer>().material = visibilityMat;
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeGrenadeScript : MonoBehaviour
{
    public float lifetime;
    public float duration;
    public float currentDuration;
    public float time;
    public float freezeDrag;
    public Material activeMat;
    private Collider[] frozenObject;
    private bool hasCast;
    private float[] previousDrag;

    public float radius;

    void Start()
    {
        time = 0;
        currentDuration = 0;
        hasCast = false;
    }

    public void toss(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (time > lifetime && currentDuration < duration)
        {
            if (!hasCast)
            {
                GetComponent<MeshRenderer>().material = activeMat;
                LayerMask playerMask = LayerMask.GetMask("Player");
                frozenObject = Physics.OverlapSphere(transform.position, radius, playerMask);
                hasCast = true;
                previousDrag = new float[frozenObject.Length];
                GetComponent<Rigidbody>().drag = freezeDrag;
                for (int i = 0; i < frozenObject.Length; i++)
                {
                    previousDrag[i] = frozenObject[i].attachedRigidbody.drag;
                }
                for (int i = 0; i < frozenObject.Length; i++)
                {
                    frozenObject[i].attachedRigidbody.drag = freezeDrag;
                }
            }
            for (int i = 0; i < frozenObject.Length; i++)
            {
                Debug.DrawLine(transform.position, frozenObject[i].transform.position, Color.green, Time.fixedDeltaTime);
            }
            currentDuration += Time.fixedDeltaTime;
        }
        else if (currentDuration > duration)
        {
            for (int i = 0; i < frozenObject.Length; i++)
            {
                frozenObject[i].attachedRigidbody.drag = previousDrag[i];
            }
            Destroy(gameObject);
        }
        time += Time.fixedDeltaTime;
    }
}

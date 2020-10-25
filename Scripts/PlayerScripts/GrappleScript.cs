using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    public enum GrappleState { REST, THROWING, RETURNING, REELING, STRUCK };

    //CONSTANTS
    public float grappleSpeed = 5f;
    public float maxGrappleLength = 300f;
    public float minGrappleLength = 0.5f;
    public float minReelLength = 5f;
    public float grappleForce = 3f;


    //GAME VARIABLES
    public GrappleState grappleState = GrappleState.REST;
    public Vector3 grapplePosition;
    public Vector3 grappleTarget;
    public float grappleLength;
    public Vector3 grapplePositionLocalVector;
    public Rigidbody attachedRb;

    public void grappleHandler(bool grappleTrigger, bool gunReady, GameObject gun)
    {
        switch (grappleState)
        {
            case GrappleScript.GrappleState.REST:
                gameObject.SetActive(false);
                if (grappleTrigger && gunReady)
                {
                    Physics.Raycast(gun.transform.position + gun.transform.forward * 3, gun.transform.forward, out RaycastHit raycastHit);
                    if (raycastHit.transform != null)
                    {
                        grappleLength = 0;
                        grapplePosition = raycastHit.point;
                        grappleState = GrappleScript.GrappleState.THROWING;
                    }
                }
                break;
            case GrappleScript.GrappleState.THROWING:
                gameObject.SetActive(true);
                if (grappleTrigger || grappleLength > maxGrappleLength)
                {
                    grappleState = GrappleScript.GrappleState.RETURNING;
                }
                break;
            case GrappleScript.GrappleState.RETURNING:
                gameObject.SetActive(true);
                break;
            case GrappleScript.GrappleState.REELING:
                gameObject.SetActive(true);
                if (grappleLength < minReelLength || grappleTrigger || grapplePosition == Vector3.zero)
                {
                    //attachedRb.GetComponent<MeshRenderer>().material = unselectedMat;
                    grappleState = GrappleScript.GrappleState.RETURNING;
                }
                break;
            case GrappleScript.GrappleState.STRUCK:
                gameObject.SetActive(true);
                if (!grappleTrigger)
                {
                    grappleState = GrappleScript.GrappleState.REELING;
                }
                break;
        }
    }


    public void grappleHandlerFixed(GameObject player, GameObject gun)
    {
        switch (grappleState)
        {
            case GrappleState.REST:
                break;
            case GrappleState.THROWING:
                transform.position = gun.transform.position;
                grappleLength += grappleSpeed * 100 * Time.fixedDeltaTime;
                transform.LookAt(grapplePosition);
                transform.localScale = new Vector3(0.2f, 0.2f, grappleLength / 3);
                break;
            case GrappleState.RETURNING:
                transform.position = player.transform.position;
                grappleLength -= grappleSpeed * 200 * Time.fixedDeltaTime;
                transform.LookAt(grapplePosition);
                transform.localScale = new Vector3(0.2f, 0.2f, grappleLength / 3);
                if (grappleLength < minGrappleLength)
                {
                    grappleState = GrappleState.REST;
                }
                break;
            case GrappleState.REELING:
                if (attachedRb == null)
                {
                    grappleState = GrappleState.RETURNING;
                    break;
                }
                transform.position = player.transform.position;
                grapplePosition = attachedRb.transform.position + attachedRb.transform.TransformVector(grapplePositionLocalVector);

                transform.LookAt(grapplePosition);
                grappleLength = (grapplePosition - transform.position).magnitude;
                transform.localScale = new Vector3(0.2f, 0.2f, grappleLength / 3);

                attachedRb.AddForceAtPosition((transform.position - grapplePosition).normalized * grappleForce, grapplePosition);
                player.GetComponent<Rigidbody>().AddForce((grapplePosition - transform.position).normalized * grappleForce, ForceMode.Acceleration);
                break;
            case GrappleState.STRUCK:
                break;
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (grappleState == GrappleState.THROWING) // && (other.gameObject.layer == 8)
        {
            grapplePosition = transform.position + transform.forward * grappleLength;
            //LayerMask mask = LayerMask.GetMask("Map");
            Collider[] sphereCast = Physics.OverlapSphere(grapplePosition, Mathf.Sqrt(grappleSpeed) * 5);
            bool verifiedCollision = false;
            Debug.Assert(sphereCast.Length > 0, "Spherecast length = 0");
            for (int i = 0; i < sphereCast.Length; i++)
            {
                if (sphereCast[i].Equals(other))
                {
                    verifiedCollision = true;
                }
                else
                {
                    Debug.Log("detected object that was not collider");
                }
            }
            if (verifiedCollision)
            {
                //if (attachedRb != null) attachedRb.GetComponent<MeshRenderer>().material = unselectedMat;
                attachedRb = other.attachedRigidbody;
                //attachedRb.GetComponent<MeshRenderer>().material = seletectMat;
                grapplePositionLocalVector = attachedRb.transform.InverseTransformPoint(grapplePosition);
                grappleState = GrappleState.STRUCK;
                if (transform.parent.tag == "Bot") transform.parent.GetComponent<BotScript>().grappleStrike();
            }
            else
            {
                Debug.Log("nonverified collision");
                grappleState = GrappleState.RETURNING;
            }
        }
        else if (grappleState == GrappleState.REELING && (other.gameObject.layer == 8))
        {
            if (!other.attachedRigidbody.Equals(attachedRb))
            {
                //Debug.Log("wrong attachedRb");
                grappleState = GrappleState.REST;
            }
        }
    }

}

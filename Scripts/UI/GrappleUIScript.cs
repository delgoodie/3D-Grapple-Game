using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
public class GrappleUIScript : MonoBehaviour
{
    GameObject player;
    GameObject rope;
    GameObject hook;
    private Vector3 initialScale;

    void Start()
    {
        rope = transform.GetChild(0).gameObject;
        hook = transform.GetChild(1).gameObject;
        initialScale = rope.GetComponent<RectTransform>().localScale;
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("LocalPlayer");
        float grapRatio = player.GetComponent<PlayerScript>().grapple.GetComponent<GrappleScript>().grappleLength / player.GetComponent<PlayerScript>().grapple.GetComponent<GrappleScript>().maxGrappleLength;
        rope.GetComponent<RectTransform>().localScale = new Vector3(initialScale.x, grapRatio * initialScale.y, 0);
        hook.transform.localPosition = new Vector3(0, grapRatio * 275, 1);
    }
}

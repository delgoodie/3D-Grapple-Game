using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
public class AmmoBarScript : MonoBehaviour
{
    GameObject player;
    GameObject leftBar;
    GameObject centerBar;
    GameObject rightBar;

    private Vector2 initialScale;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("LocalPlayer");
        leftBar = transform.GetChild(0).gameObject;
        centerBar = transform.GetChild(1).gameObject;
        rightBar = transform.GetChild(2).gameObject;
        initialScale = centerBar.GetComponent<RectTransform>().localScale;
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("LocalPlayer");
        GunScript gunScript = player.GetComponentInChildren<GunScript>();
        leftBar.GetComponent<RectTransform>().localScale = new Vector2(initialScale.x, (gunScript.ammo > 0) ? initialScale.y : 0f);
        centerBar.GetComponent<RectTransform>().localScale = new Vector2(initialScale.x, (gunScript.ammo > 1) ? initialScale.y : 0f);
        rightBar.GetComponent<RectTransform>().localScale = new Vector2(initialScale.x, (gunScript.ammo > 2) ? initialScale.y : 0f);
    }
}

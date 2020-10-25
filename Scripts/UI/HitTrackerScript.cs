using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.Events;
public class HitTrackerScript : MonoBehaviour
{
    public GameObject HitBar;
    GameObject[] bars;
    public int index;

    void Start()
    {
        index = 0;
        bars = new GameObject[30];
    }

    void Update()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<GunScript>().hitEvent.AddListener(addHit);
    }
    Vector3 start = new Vector3(-350, 125, 0);

    void addHit()
    {
        index++;
        //Debug.Log(Instantiate(HitBar));
        //bars[index].GetComponent<Image>().color = new Color32(255, 255, 225, 100);
    }
}


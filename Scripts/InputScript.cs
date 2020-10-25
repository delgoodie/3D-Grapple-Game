using UnityEngine;

public class InputScript : MonoBehaviour
{
    public bool wKey, aKey, sKey, dKey, eKey, rKey;
    public bool oneKey, twoKey, threeKey, fourKey, fiveKey;
    public bool spaceKey;
    public bool rightClick, leftClick;
    public float mouseX, mouseY;
    public float mouseSensitivityX = 800f;
    public float mouseSensitivityY = 1500f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        wKey = false; aKey = false; sKey = false; dKey = false; eKey = false; rKey = false;
        oneKey = false; twoKey = false; threeKey = false; fourKey = false; fiveKey = false;
        spaceKey = false;
        rightClick = false; leftClick = false;
    }

    void Update()
    {
        if (Input.GetKey("w")) wKey = true; else wKey = false;
        if (Input.GetKey("a")) aKey = true; else aKey = false;
        if (Input.GetKey("s")) sKey = true; else sKey = false;
        if (Input.GetKey("d")) dKey = true; else dKey = false;
        if (Input.GetKey("e")) eKey = true; else eKey = false;
        if (Input.GetKey("r")) rKey = true; else rKey = false;
        if (Input.GetKey("space")) spaceKey = true; else spaceKey = false;
        if (Input.GetKey("1")) oneKey = true; else oneKey = false;
        if (Input.GetKey("2")) twoKey = true; else twoKey = false;
        if (Input.GetKey("3")) threeKey = true; else threeKey = false;
        if (Input.GetKey("4")) fourKey = true; else fourKey = false;
        if (Input.GetKey("5")) fiveKey = true; else fiveKey = false;
        if (Input.GetMouseButtonDown(0)) leftClick = true; else leftClick = false;
        if (Input.GetMouseButtonDown(1)) rightClick = true; else rightClick = false;
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;
    }
}

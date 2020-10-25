using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    public GameObject border;
    public bool boxEnabled;
    public int size = 0;
    public int thickness = 0;
    public Material material;
    Vector3 scale;
    GameObject left;
    GameObject right;
    GameObject front;
    GameObject back;
    GameObject top;
    GameObject bottom;
    void Start()
    {
        scale = new Vector3(size, size, thickness);
        if (boxEnabled) createBox();
    }

    void Update()
    {

    }


    void createBox()
    {
        left = createBorder(new Vector3(size / 2, 0, 0), new Vector3(0, 90, 0), scale, material);
        right = createBorder(new Vector3(-size / 2, 0, 0), new Vector3(0, -90, 0), scale, material);
        front = createBorder(new Vector3(0, 0, size / 2), new Vector3(0, 0, 0), scale, material);
        back = createBorder(new Vector3(0, 0, -size / 2), new Vector3(0, 0, 0), scale, material);
        top = createBorder(new Vector3(0, size / 2, 0), new Vector3(90, 0, 0), scale, material);
        bottom = createBorder(new Vector3(0, -size / 2, 0), new Vector3(-90, 0, 0), scale, material);
    }

    GameObject createBorder(Vector3 position, Vector3 rotation, Vector3 scale, Material mat)
    {
        GameObject ret = Instantiate(border, position, Quaternion.Euler(rotation), transform);
        ret.GetComponent<MeshRenderer>().material = mat;
        ret.transform.localScale = scale;
        return ret;
    }
}

using UnityEngine;

public class ObstacleManagerScript : MonoBehaviour
{
    public GameObject obstacle;
    public bool obstaclesEnabled;
    public int minCount = 50;
    public int maxCount = 100;
    public float minSize = 10;
    public float maxSize = 50;
    public Material material;
    public bool autoCalculate = true;
    private int count = 0;
    private int total = 0;
    int positionRange;
    int frameSpacing = 1;

    private bool needNewObstacles = true;


    void Update()
    {

        positionRange = 250 / 2 - 250 / 20;
        total = (int)(250f / Mathf.Sqrt(maxSize));
        /*
        if (count < total * frameSpacing)
        {
            if (count % frameSpacing == 0)
                createObstacle();
            count++;
        }
        */
        if (needNewObstacles)
        {
            for (int i = 0; i < total; i++) createObstacle();
            needNewObstacles = false;
        }
    }

    void createObstacle()
    {
        float xScale = maxSize; //Random.Range(minSize, maxSize);
        float yScale = maxSize; //Random.Range(minSize, maxSize);
        float zScale = maxSize; //Random.Range(minSize, maxSize);
        Vector3 size = new Vector3(xScale, yScale, zScale);
        Quaternion randomAngle = Quaternion.Euler(0, 0, 0);//Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        Vector3 randomPosition = Vector3.zero;
        bool validLocation = false;
        while (!validLocation)
        {
            randomPosition = new Vector3(Random.Range(-220, 220), Random.Range(-positionRange, positionRange), Random.Range(-positionRange, positionRange));
            if (Physics.OverlapBox(randomPosition, size, randomAngle).Length == 0) validLocation = true;
        }

        GameObject obst = Instantiate(obstacle, randomPosition + transform.position, randomAngle, transform);

        obst.transform.localScale = size;
        obst.GetComponent<MeshRenderer>().material = material;
    }
    public void deleteAllObstacles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        count = 0;
        needNewObstacles = true;
    }
}

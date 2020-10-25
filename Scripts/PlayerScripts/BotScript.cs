using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class BotScript : Agent
{
    public GameObject gun;
    public GameObject grapple;

    public PlayerScript playerScript;
    public GrappleScript grappleScript;
    public GunScript gunScript;

    public float grappleAccuracy = 5f;

    public int spawnIndex;
    private Vector3 targetSpace;
    private float totReward;
    private int numEpisodes = 0;



    void Start()
    {
        gun = transform.GetChild(0).gameObject;
        grapple = transform.GetChild(1).gameObject;

        playerScript = GetComponent<PlayerScript>();
        gunScript = gun.GetComponent<GunScript>();
        grappleScript = grapple.GetComponent<GrappleScript>();
    }

    public void FixedUpdate()
    {
        //Debug.Log("Velocity reward: " + Vector3.Dot((targetSpace - transform.position).normalized, GetComponent<Rigidbody>().velocity.normalized) * .01f);
        if (grapple.GetComponent<GrappleScript>().grappleState == GrappleScript.GrappleState.REELING) AddReward(.005f);
        AddReward(Vector3.Dot((targetSpace - transform.position).normalized, GetComponent<Rigidbody>().velocity.normalized) * .01f);
        if ((targetSpace - transform.position).magnitude <= grappleAccuracy)
        {
            AddReward(2f);
            EndEpisode();
        }
        AddReward(-(targetSpace - transform.position).magnitude / 1000000f);
        totReward = GetCumulativeReward();
    }

    public void grappleStrike()
    {
        AddReward(0.01f);
    }

    public void hitWall()
    {
        AddReward(-.05f);
    }

    public void OnGizmosDraw()
    {
        Gizmos.DrawSphere(targetSpace, grappleAccuracy);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = (Input.GetKey(KeyCode.E)) ? 1f : 0f;
        actionsOut[1] = (Input.GetKey(KeyCode.F)) ? 1f : 0f;
        actionsOut[2] = -Input.GetAxis("Vertical");
        actionsOut[3] = Input.GetAxis("Horizontal");
        actionsOut[4] = 0f;

    }

    public override void OnEpisodeBegin()
    {
        numEpisodes++;
        //transform.position = transform.parent.GetComponent<PlayerManagerScript>().getSpawn(spawnIndex);
        //transform.parent.parent.GetChild(1).GetComponent<ObstacleManagerScript>().deleteAllObstacles();
        Debug.Log("Total Reward for Episode " + numEpisodes + ": " + totReward);
        totReward = 0f;

        gunScript.ammo = 3;
        playerScript.health = 3;

        transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = randomSpace();

        targetSpace = randomSpace();
        GameObject.Find("TargetSpace").transform.position = targetSpace;
    }

    public override void CollectObservations(VectorSensor sensor) //5 + 7 + 6 = 18
    {
        //grappleState 5 OBS
        sensor.AddObservation((int)grappleScript.grappleState == 0);
        sensor.AddObservation((int)grappleScript.grappleState == 1);
        sensor.AddObservation((int)grappleScript.grappleState == 2);
        sensor.AddObservation((int)grappleScript.grappleState == 3);
        sensor.AddObservation((int)grappleScript.grappleState == 4);

        //rotation & position 7 OBS
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(transform.position);

        //target position and vector pointing to target 6 OBS
        sensor.AddObservation(targetSpace);
        sensor.AddObservation((targetSpace - transform.position).normalized);


    }

    public override void OnActionReceived(float[] vectorAction)
    { //4 actions
        if (Mathf.FloorToInt(vectorAction[0]) == 1) shootGrapple();
        if (Mathf.FloorToInt(vectorAction[1]) == 1) pullGrapple();
        if (Mathf.FloorToInt(vectorAction[0]) == 1 && Mathf.FloorToInt(vectorAction[1]) == 1) playerScript.grappleTrigger = false;

        transform.Rotate(new Vector3(vectorAction[2], vectorAction[3], vectorAction[4]), Space.Self);
    }

    private Vector3 randomSpace()
    {
        bool validSpace = false;
        Vector3 space = Vector3.zero;
        while (!validSpace)
        {
            space = new Vector3(UnityEngine.Random.Range(-245, 245), UnityEngine.Random.Range(-120, 120), UnityEngine.Random.Range(-120, 120));
            Physics.SphereCast(space, 5f, Vector3.forward, out RaycastHit r, 5f);
            if (r.collider == null)
            {
                validSpace = true;
            }
        }
        return space;
    }

    private void shootGrapple()
    {
        if (grappleScript.grappleState == GrappleScript.GrappleState.REST)
        {
            playerScript.grappleTrigger = true;
            AddReward(0.01f);
        }
        else
        {
            AddReward(-0.005f);
        }
    }

    private void pullGrapple()
    {
        if (grappleScript.grappleState == GrappleScript.GrappleState.REELING)
        {
            playerScript.grappleTrigger = true;
            AddReward(0.001f);
        }
        else
        {
            AddReward(-0.005f);
        }
    }

}

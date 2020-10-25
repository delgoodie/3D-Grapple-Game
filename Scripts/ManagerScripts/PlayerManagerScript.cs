using UnityEngine;
using Unity.MLAgents;
public class PlayerManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject botPrefab;
    public bool playersEnabled;
    public GameObject cam;
    public const int botCount = 10;
    public int range = 100;

    public GameObject localPlayer;
    public GameObject[] bot;

    void Start()
    {
        bot = new GameObject[transform.childCount];
        if (playersEnabled)
        {
            //createLocalPlayer();
            createBots();
        }
    }

    void createLocalPlayer()
    {
        Vector3 position = GameObject.Find("Map").transform.GetChild(0).transform.position;
        localPlayer = Instantiate(playerPrefab, position, Quaternion.Euler(0, 90, 0), transform);
        GameObject localCam = Instantiate(cam, localPlayer.transform.position + new Vector3(0, 2, 0), localPlayer.transform.rotation, localPlayer.transform);

        localPlayer.GetComponent<LocalPlayerScript>().cam = localCam;
    }

    void createBots()
    {
        for (int i = 0; i < bot.Length; i++) // i < 10
        {
            //Vector3 position = GameObject.Find("Map").transform.GetChild(i).transform.position;
            //Quaternion randomAngle = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            //bot[i] = Instantiate(botPrefab, position, randomAngle, transform);
            //bot[i].GetComponent<BotScript>().spawnIndex = i;
            bot[i] = transform.GetChild(i).gameObject;
        }
    }

    public Vector3 getSpawn(int index)
    {
        return transform.parent.parent.GetChild(3).transform.GetChild(index).transform.position;
    }

}

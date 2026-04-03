using UnityEngine;

public class PacmanPelletSpawner : MonoBehaviour
{
    public GameObject prefabPellet;
    public GameObject floorParent;
    private GameObject floor;

    void Start()
    {
        floor = GameObject.Find("Floor");
        int x = (int) floor.transform.lossyScale.x;
        int parentScale = (int)floorParent.transform.lossyScale.x;
        //SpawnPellets(10, 10, 10);
        SpawnPellets(x *  parentScale, (int)floor.transform.lossyScale.z * (int)floor.transform.lossyScale.z, x * parentScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void SpawnPellets(int length, int width, int startingNum)
    {
        Debug.Log("This is length: " + length);
        //base case
        if (length <= 0) return;

        GameObject newObject = Instantiate(prefabPellet);
        if(length%2 == 0)
        {
            newObject.transform.position = new Vector3(length, 1, width);
        }
        else
        {
            newObject.transform.position = new Vector3(length + 5, 1, width);
        }

            Debug.Log("This is width: " + width);
        if(width != 0)
        {
            SpawnPellets(length, width - 1, startingNum);
        }
        else
        {
            SpawnPellets(length - 1, startingNum, startingNum);
        }
            
    }
}

using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject prefabCollectible;
    public int numberToSpawn;
    public float size;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i <= numberToSpawn; i += 2)
        {
            float x = Random.Range(-9.5f * size, 9.5f * size);
            float z = Random.Range(-9.5f * size, 9.5f * size);

            GameObject newObject = Instantiate(prefabCollectible);
            newObject.transform.position = new Vector3(x, 1, z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

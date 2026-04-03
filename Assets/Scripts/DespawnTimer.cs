using System.Collections;
using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
    public float despawnAfter = 3.0f;

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(despawnAfter);
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}

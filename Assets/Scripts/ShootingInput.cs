using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingInput : MonoBehaviour
{
    GameObject projectilePrefab;
    Camera mainCamera;
    public float muzzleVelocity = 30;
    void Start()
    {
        projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
        mainCamera = GetComponentInChildren<Camera>();
    }

    void OnFire(InputValue v)
    {
        GameObject go = Instantiate(projectilePrefab);
        //Debug.Log(mainCamera.transform.forward.normalized);
        go.transform.position = mainCamera.transform.position + mainCamera.transform.forward.normalized * 1.1f;
        go.GetComponent<Rigidbody>().AddForce(mainCamera.transform.forward.normalized * muzzleVelocity, ForceMode.Impulse);
    }

    void Update()
    {
        
    }
}

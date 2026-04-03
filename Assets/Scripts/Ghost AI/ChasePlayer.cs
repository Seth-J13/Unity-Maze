using System;
using UnityEngine;
using System.Collections;
public class ChasePlayer : MonoBehaviour
{
    private Vector3 origin = Vector3.zero;
    public float speed = 1f;
    private float angle = 0;
    private Vector3[] directions = new[] { new Vector3(-1f, 0f, 1f), new Vector3(0f, 0f, 1f), new Vector3(1f, 0f, 1f) }; 
    private Rigidbody rb;
    private bool canChangeDir = true;
    private void Awake()
    {
        origin = transform.position;
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        foreach (Vector3 direction in directions)
        {
                Debug.DrawRay(origin, direction, Color.red, 0.5f);
            if (Physics.Raycast(origin, direction, out RaycastHit info, 3f))
            {
                if (info.collider.name == "Player")
                {
                    Debug.Log("I see the player");
                    transform.LookAt(info.point);
                    Debug.Log("Changing look direction");
                    StartCoroutine(WaitUnit());

                }
                else
                {
                    if(info.collider.tag == "Wall")
                    {
                        Debug.Log("I hit a wall");
                        angle = UnityEngine.Random.Range(-40f + transform.forward.y, 40f + transform.forward.y);
                        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

                        //if (canChangeDir)
                        //{
                        //    canChangeDir = !canChangeDir;
                        //    angle = UnityEngine.Random.Range(-90f + transform.forward.y, 90f + transform.forward.y);
                        //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
                        //    Debug.Log("Changing to random direction");
                        //    StartCoroutine(WaitUnit());
                        //}
                    }


                }
            }
        }

        rb.AddForce(transform.forward * speed);





    }

    IEnumerator WaitUnit()
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(0.5f);
        canChangeDir = !canChangeDir;
        Debug.Log(canChangeDir);
    }

}

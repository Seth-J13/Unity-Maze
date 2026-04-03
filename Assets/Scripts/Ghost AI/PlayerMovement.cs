using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.0f;
    Vector3 movement = Vector3.zero;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue value)
    {
        Vector2 movement2D = value.Get<Vector2>();
        movement = new Vector3(movement2D.x, 0, movement2D.y);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(movement * speed * Time.deltaTime);

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Collectible")
        {
            GameManager.Instance.AddPickup();
            Destroy(other.gameObject);
        }
    }
}

using UnityEngine;

public class ExitLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("I exit, collided");
        if(other.gameObject.tag == "Player")
        {
            GameManagerTwo gm = GameObject.Find("GameManager").GetComponent<GameManagerTwo>();  
            gm.ExitMaze();
            Destroy(this.gameObject);
        }
    }
}

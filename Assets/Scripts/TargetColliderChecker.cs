using TMPro;
using UnityEngine;

public class TargetColliderChecker : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            GameManagerTwo gm = GameObject.Find("GameManager").GetComponent<GameManagerTwo>();
            Destroy(other.gameObject);
            gm.UpdateTargetCount();
            Destroy(this.gameObject);
        }
    }
}

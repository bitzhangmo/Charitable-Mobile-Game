using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoint : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Ball")
        {
            other.gameObject.layer = LayerMask.NameToLayer("Default");
            Ball ball = other.gameObject.GetComponent<Ball>();
            gameManager.gameObject.SendMessage("RemoveFromBalls",ball);
        }
    }
}

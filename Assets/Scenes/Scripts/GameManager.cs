using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public bool isMouseDown = false;
    public Vector2 startPos;
    public Vector2 endPos;
    public Vector2 direction;
    GameObject target;
    public float percent = 0.0f;
    public float maxDistance = 5.0f;

    public Ball chosenBall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
    }

    void UserInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            startPos = Input.mousePosition;

            if (chosenBall != null)
            {
                chosenBall.isChosen = false;
            }

            Ray ray2D = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(ray2D.origin.x, ray2D.origin.y), Vector2.zero);

            if(hit.collider)
            {
                if(hit.transform.gameObject.tag == "Ball")
                {
                    target = hit.transform.gameObject;
                    chosenBall = target.GetComponent<Ball>();
                    chosenBall.isChosen = true;
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            endPos = Input.mousePosition;
            percent = Vector2.Distance(startPos,endPos)/maxDistance;
            direction = startPos - endPos;
            direction.Normalize();
            PushTarget(target, direction, percent);
        }

        // if(isMouseDown)
        // {
        //     percent = Vector2.Distance(startPos,endPos);
        // }
    }

    // void UserInputMobile()
    // {
    //     // Track a single touch as a direction control.
    //     if (Input.touchCount > 0)
    //     {
    //         Touch touch = Input.GetTouch(0);

    //         // Handle finger movements based on TouchPhase
    //         switch (touch.phase)
    //         {
    //             //When a touch has first been detected, change the message and record the starting position
    //             case TouchPhase.Began:
    //                 // Record initial touch position.
    //                 startPos = touch.position;
    //                 // message = "Begun ";
    //                 Ray ray2D = Camera.main.ScreenPointToRay(Input.mousePosition);
    //                 RaycastHit2D hit = Physics2D.Raycast(new Vector2(ray2D.origin.x, ray2D.origin.y), Vector2.zero);

    //                 if(hit.collider)
    //                 {   
    //                     if(hit.transform.gameObject.tag == "Ball")
    //                     {
    //                         target = hit.transform.gameObject;
    //                     }
    //                 }
    //                 break;

    //             //Determine if the touch is a moving touch
    //             case TouchPhase.Moved:
    //                 // Determine direction by comparing the current touch position with the initial one
    //                 direction = touch.position - startPos;
    //                 // message = "Moving ";
    //                 break;

    //             case TouchPhase.Ended:
    //                 // Report that the touch has ended when it ends
    //                 // message = "Ending ";
    //                 break;
    //         }
    //     }
    // }

    void PushTarget(GameObject target,Vector2 direction,float percent)
    {
        Rigidbody2D rb2D = target.GetComponent<Rigidbody2D>();
        rb2D.AddForce(direction*percent);
    }
}

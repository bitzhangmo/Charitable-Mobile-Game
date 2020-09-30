using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    // Start is called before the first frame update
    public Score mScore;

    public int type;

    public Text textOne;
    public Text textTwo;
    public Text textThree;

    public GameObject self;

    void Start()
    {
        if(type == 1)
        {
            mScore = Utils.ReadScoreOne();
        }

        if (type == 2)
        {
            mScore = Utils.ReadScoreTwo();
        }


        textOne.text = mScore.scoreOne + "";
        textTwo.text = mScore.scoreTwo + "";
        textThree.text = mScore.scoreThree + "";
    }

 


    //private void FixedUpdate()
    //{
    //    textOne.text = mScore.scoreOne+"";
    //    textTwo.text = mScore.scoreTwo + "";
    //    textThree.text = mScore.scoreThree + "";
    //}



    public void OnDismiss()
    {
        self.SetActive(false);
    }


    public void show()
    {
        self.SetActive(true);
    }



}

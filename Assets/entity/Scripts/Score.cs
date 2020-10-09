
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Score
{
   
    public int scoreOne;
    public int scoreTwo;
    public int scoreThree;

    public List<int> nums = new List<int>();
    public Score()
    {
       
        scoreOne = 0;
        scoreTwo = 0;
        scoreThree = 0;

        nums.Add(int.MaxValue);
        nums.Add(int.MaxValue);
        nums.Add(int.MaxValue);
    }


    public Score(int one, int two, int three)
    {
      
        scoreOne = one;
        scoreTwo = two;
        scoreThree = three;
        nums.Add(one);
        nums.Add(two);
        nums.Add(three);
    }


    public void insert(int n)
    {
        int tmp;

        // if(n < scoreOne)
        // {
        //     tmp = scoreOne;
        //     scoreOne = n;
        //     n = scoreTwo;
        //     scoreTwo = tmp;
        //     scoreThree = n;
            
        // }
        // else if (n < scoreTwo)
        // {
        //     tmp = scoreTwo;
        //     scoreTwo = n;
        //     scoreThree = tmp;
        // }
        // else if (n < scoreThree)
        // {
        //     scoreThree = n;
        // }
        // else
        // {
        //     return;
        // }
        nums.Add(n);
        nums.Sort();
        scoreOne = (nums[0] == int.MaxValue) ? 0 : nums[0];
        scoreTwo = (nums[1] == int.MaxValue) ? 0 : nums[1];
        scoreThree = (nums[2] == int.MaxValue) ? 0 : nums[2];
        Debug.Log(scoreOne);
        Debug.Log(scoreTwo);
        Debug.Log(scoreThree);
    }
}

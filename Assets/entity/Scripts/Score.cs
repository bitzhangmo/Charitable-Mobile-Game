
using System;

[Serializable]
public class Score
{
   
    public int scoreOne;
    public int scoreTwo;
    public int scoreThree;


    public Score()
    {
       
        scoreOne = 0;
        scoreTwo = 0;
        scoreThree = 0;
    }


    public Score(int one, int two, int three)
    {
      
        scoreOne = one;
        scoreTwo = two;
        scoreThree = three;
    }


    public void insert(int n)
    {
        int tmp;

        if(n > scoreOne)
        {
            tmp = scoreOne;
            scoreOne = n;
            n = scoreTwo;
            scoreTwo = tmp;
            scoreThree = n;
            
        }
        else if (n > scoreTwo)
        {
            tmp = scoreTwo;
            scoreTwo = n;
            scoreThree = tmp;
        }
        else if (n > scoreThree)
        {
            scoreThree = n;
        }
        else
        {
            return;
        }

    }
}

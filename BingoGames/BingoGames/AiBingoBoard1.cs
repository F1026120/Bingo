using System.Collections;
using System.Collections.Generic;


// 賓果Ai
public class AiBingoBoard : BingoBoard
{
    public AiBingoBoard()
    {
    }

    const int bound = 5;
    int[,] pointValue = new int[bound, bound];
    int HightestValueNumber = -1;

    public int GetNextNumber()
    {
        Reset();
        CalcValue();
        UseHighestValue();

        //Debug.Log ("Ai1出牌[" + HighReturnOnInvestmentAddress + "]");
        return HightestValueNumber;
    }

    void Reset()
    {
        for (int c = 0; c < bound; c++)
        {
            for (int r = 0; r < bound; r++)
            {
                pointValue[c, r] = 0;
            }
        }
    }

    void CalcValue()
    {
        CalcAllValue();
        CalcSpecialValue();
    }
    void CalcAllValue()
    {
        for (int c = 0; c < bound; c++)
        {
            for (int r = 0; r < bound; r++)
            {
                if (m_Board[c, r] == 0)
                {
                    AddCol(c);
                    AddRow(r);
                    AddSlash(c, r);
                    AddBackSlash(c, r);
                }
            }
        }
    }
    void CalcSpecialValue()
    {
        CalcSlashValue();
        CalcBackSlashValue();
    }
    void CalcSlashValue()
    {
        for (int c = 0; c < bound; c++)
        {
            for (int r = 0; r < bound; r++)
            {
                if (c == r)
                {
                    pointValue[c, r] += 4;
                }
            }
        }
    }
    void CalcBackSlashValue()
    {
        for (int c = 0; c < bound; c++)
        {
            for (int r = 0; r < bound; r++)
            {
                if ((bound - c - 1) == r)
                {
                    pointValue[c, r] += 4;
                }
            }
        }
    }
    void AddCol(int col)
    {
        for (int r = 0; r < bound; r++)
        {
            pointValue[col, r]++;
        }
    }
    void AddRow(int row)
    {
        for (int c = 0; c < bound; c++)
        {
            pointValue[c, row]++;
        }
    }
    void AddSlash(int col, int row)
    {
        if (col == row)
        {
            for (int c = 0; c < bound; c++)
            {
                for (int r = 0; r < bound; r++)
                {
                    if (c == r)
                    {
                        pointValue[c, r] += 2;
                    }
                }
            }
        }
    }
    void AddBackSlash(int col, int row)
    {
        if ((bound - col - 1) == row)
        {
            for (int c = 0; c < bound; c++)
            {
                for (int r = 0; r < bound; r++)
                {
                    if ((bound - c - 1) == r)
                    {
                        pointValue[c, r] += 2;
                    }
                }
            }
        }
    }

    void UseHighestValue()
    {
        int value = 0;
        for (int c = 0; c < bound; c++)
        {
            for (int r = 0; r < bound; r++)
            {
                if (m_Board[c, r] == 0)
                    continue;
                if (pointValue[c, r] > value)
                {
                    value = pointValue[c, r];
                    HightestValueNumber = m_Board[c, r];
                }
            }
        }
    }


}

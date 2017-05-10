using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 賓果Ai
public class AiBingoBoard : BingoBoard
{
    protected int[] m_LinePoint = new int[12];

    public AiBingoBoard()
    {

    }

    // 決定出牌
    public int GetNextNumber()
    {
        int[,] point = new int[5, 5];
        int NextNumber = -1;
        int col = 5;
        int row = 5;
        //計算每個位置價值(point)
        for (int c = 0; c < 5; c++)
        {
            for (int r = 0; r < 5; r++)
            {
                point[c, r] = 0;
                if (m_Board[c, r] == 0)
                    continue;
                else if (c == 2 && c == r)//在正中央的交點
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (m_Board[c, k] == 0)
                            point[c, r] += 1;
                        if (m_Board[k, r] == 0)
                            point[c, r] += 1;
                        if (m_Board[k, k] == 0)
                            point[c, r] += 1;
                        if (m_Board[k, 4 - k] == 0)
                            point[c, r] += 1;
                    }

                    point[c, r] += 12;//交點加權12
                }
                else if (c == row || (c + r) == 4)//在斜線上的點
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (m_Board[c, k] == 0)
                            point[c, r] += 1;
                        if (m_Board[k, r] == 0)
                            point[c, r] += 1;
                        if (m_Board[k, k] == 0)
                            point[c, r] += 1;
                        if (m_Board[k, 4 - k] == 0)
                            point[c, r] += 1;
                    }
                    point[c, r] += 8;//斜線上加權8
                }
                else
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (m_Board[c, k] == 0)
                            point[c, r] += 1;
                        if (m_Board[k, r] == 0)
                            point[c, r] += 1;
                    }
                }

            }
        }
        //找出最有價值的選擇
        int MaxPrice = -1;
        int MaxPriceNumber = -1;
        for (int c = 0; c < 5; c++)
        {
            for (int r = 0; r < 5; r++)
            {
                if (point[c, r] > MaxPrice)
                {
                    MaxPrice = point[c, r];
                    MaxPriceNumber = m_Board[c, r];
                }
            }
        }

        NextNumber = MaxPriceNumber;


        /*
        int lineindex = 0;
        int point = 0;

        //計算列值
        for (int i = 0; i < 5; i++)
        {
            point = 0;
            for (int j = 0; j < 5; j++)
                if (m_Board[i, j] > 0)
                    point++;
            m_LinePoint[lineindex++] = point;
        }

        //計算行值
        for (int j = 0; j < 5; j++)
        {
            point = 0;
            for (int i = 0; i < 5; i++)
                if (m_Board[i, j] > 0)
                    point++;
            m_LinePoint[lineindex++] = point;
        }

        // TODO:左斜,右斜
        for (int i = 0; i < 5; i++)
        {
            point = 0;
            if (m_Board[i, i] > 0)
                point++;
        }
        m_LinePoint[lineindex++] = point;


        for (int i = 4; i >= 0; i--)
        {
            point = 0;
            if (m_Board[i, 4 - i] > 0)
                point++;
        }
        m_LinePoint[lineindex++] = point;

        // 取得最少分數者
        int MinNum = 6;
        int MinIndex = -1;
        for (lineindex = 0; lineindex < 12; ++lineindex)
            if (m_LinePoint [lineindex] != 0 && m_LinePoint [lineindex] < MinNum) 
            {
                MinNum = m_LinePoint [lineindex];
                MinIndex = lineindex;
            }

        // 決定出牌,列方向
        int NextNumber = 0;
        if( MinIndex<5 )
        {
            // 第一個號碼
            for (int j = 0; j < 5; j++)
                if (m_Board [MinIndex, j] != 0) {
                    NextNumber = m_Board [MinIndex, j];
                    break;
                }
        }
        // 決定出牌,行方向
        else if(MinIndex <10)
        {
            // 第一個號碼
            for (int i = 0; i < 5; i++)
                if (m_Board [i, MinIndex - 5] != 0) {
                    NextNumber = m_Board [i, MinIndex - 5];
                    break;
                }
        }
        // TODO,左斜(左上->右下)
        else if(MinIndex == 10)
        {
            for(int i = 0; i < 5; i++)
            {
                if(m_Board[i,i]!= 0)
                {
                    NextNumber = m_Board[i, i];
                    Debug.Log("電腦選擇左斜");
                    break;
                }
            }
        }
        // TODO,右斜(右上->左下)
        else
        {
            for(int i = 4;i>=0;i--)
                if (m_Board[i, 4 - i] != 0)
                {
                    NextNumber = m_Board[i, 4 - i];
                    Debug.Log("電腦選擇右斜");
                    break;
                }
        }
        */
        Debug.Log("電腦出牌[" + NextNumber + "]" + "價值 " + MaxPrice);
        return NextNumber;
    }
}

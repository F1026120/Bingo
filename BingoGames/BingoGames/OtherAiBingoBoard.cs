using System.Collections;
using System.Collections.Generic;


// 賓果Ai
public class OtherAiBingoBoard : BingoBoard
{
    protected int[] m_LinePoint = new int[12];

    public OtherAiBingoBoard()
    {

    }

    // 決定出牌
    public int GetNextNumber() //Ai level 3
    {
        int[,] point = new int[5, 5];
        int NextNumber = -1;
        int[,] colPrice = new int[5, 5];
        int[,] rowPrice = new int[5, 5];
        int bound = 5; //正方形邊長
        //計算每個位置價值(point)
        for (int c = 0; c < bound; c++)
        {
            for (int r = 0; r < bound; r++)
            {
                point[c, r] = 0;
                colPrice[c, r] = 0;
                rowPrice[c, r] = 0;
                if (m_Board[c, r] == 0)//如果以選過則不用判斷
                    continue;
                for (int k = 0; k < bound; k++)
                {
                    if (m_Board[c, k] == 0) { point[c, r] += 1; rowPrice[c, r] += 1; }//判斷row方向之已選過的點 
                    if (m_Board[k, r] == 0) { point[c, r] += 1; colPrice[c, r] += 1; }//判斷col方向之已選過的點
                    if (c == r || (c + r) == (bound - 1))//如果為斜線上的點
                    {
                        if (c == r)//左上向右下的斜線
                            if (m_Board[k, k] == 0) point[c, r] += 2;
                        if ((c + r) == (bound - 1))//左下向 右上的斜線
                            if (m_Board[k, (bound - 1) - k] == 0) point[c, r] += 2;

                    }
                }
                if (c == r || (c + r) == (bound - 1)) point[c, r] += 8;//斜線上的點加權8
                if (c == r && (c + r) == (bound - 1)) point[c, r] += 4;//中心交點 額外加權 4
            }
        }
        //找出最有價值的選擇
        int MaxPrice = -1;
        int MaxPriceNumber = -1;
        int MaxPry = -1;//優先值 => (rowPrice - colPrice )^2
        for (int c = 0; c < 5; c++)
        {
            for (int r = 0; r < 5; r++)
            {
                if (point[c, r] == MaxPrice)//if 兩點價值相等 比較優先度
                {
                    int thepry = (colPrice[c, r] - rowPrice[c, r]) * (colPrice[c, r] - rowPrice[c, r]);//平方取正
                    if (thepry > MaxPry)
                    {
                        MaxPry = thepry;
                        MaxPrice = point[c, r];
                        MaxPriceNumber = m_Board[c, r];
                    }

                }
                else if (point[c, r] > MaxPrice)//比較價值 
                {
                    MaxPrice = point[c, r];
                    MaxPry = (colPrice[c, r] - rowPrice[c, r]) * (colPrice[c, r] - rowPrice[c, r]);
                    MaxPriceNumber = m_Board[c, r];
                }
            }
        }
        NextNumber = MaxPriceNumber;

        //System.Console.Write
        //Debug.Log("電腦出牌[" + NextNumber + "]" + "價值 " + MaxPrice);
        return NextNumber;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoGames
{
    class Program
    {
        enum WhichOnefirst
        {

            Ai1,
            Ai2,
            GameOver,
        }
        //WhichOnefirst m_WhichOnePlay = WhichOnefirst.Ai1;


        static void Main(string[] args)
        {
            WhichOnefirst m_WhichOnePlay = new WhichOnefirst();
            m_WhichOnePlay = WhichOnefirst.Ai2;
            bool isGameOver = false;
            bool isReachLimit = false;
            int winTimer1 = 0;
            int winTimer2 = 0;
            int tieTimer = 0;
            int allTimer = 0;
            int SetRuntime = 1000000; //設定場數

            // 資料結構
            AiBingoBoard m_ComBoard1 = new AiBingoBoard();
            OtherAiBingoBoard m_ComBoard2 = new OtherAiBingoBoard();

            while (!(isReachLimit))
            {
                m_ComBoard1.InitBoard();
                m_ComBoard2.InitBoard();
                isGameOver = false;
                while (!(isGameOver))
                {
                    if (m_WhichOnePlay == WhichOnefirst.Ai1)
                    {
                        int NextNumber = m_ComBoard1.GetNextNumber();
                        m_ComBoard1.SetNumber(NextNumber);
                        m_ComBoard2.SetNumber(NextNumber);

                        NextNumber = m_ComBoard2.GetNextNumber();
                        m_ComBoard2.SetNumber(NextNumber);
                        m_ComBoard1.SetNumber(NextNumber);
                        m_WhichOnePlay = WhichOnefirst.Ai2;
                    }
                    else if (m_WhichOnePlay == WhichOnefirst.Ai2)
                    {
                        int NextNumber = m_ComBoard2.GetNextNumber();
                        m_ComBoard1.SetNumber(NextNumber);
                        m_ComBoard2.SetNumber(NextNumber);

                        NextNumber = m_ComBoard1.GetNextNumber();
                        m_ComBoard2.SetNumber(NextNumber);
                        m_ComBoard1.SetNumber(NextNumber);
                        m_WhichOnePlay = WhichOnefirst.Ai1;
                    }




                    // 計算雙方分數及顯示		           
                    int ComLine = m_ComBoard1.CountLine();
                    //m_ComLine1.text = string.Format ("目前連線數:{0}", ComLine);
                    int ComLine2 = m_ComBoard2.CountLine();
                    //		m_ComLine2.text = string.Format ("目前連線數:{0}", ComLine2);

                    if (ComLine2 >= 5 && ComLine < 5)
                    {
                        winTimer2++;
                        isGameOver = true;
                        System.Console.WriteLine("第" + allTimer + "場 Ai2勝利 ");
                    }

                    else if (ComLine >= 5 && ComLine2 < 5)
                    {
                        winTimer1++;
                        isGameOver = true;
                        System.Console.WriteLine("第" + allTimer + "場 Ai1勝利 ");
                    }

                    else if (ComLine2 >= 5 && ComLine >= 5)
                    {
                        tieTimer++;
                        isGameOver = true;
                        System.Console.WriteLine("第" + allTimer + "場 平手 ");
                    }
                    allTimer = tieTimer + winTimer1 + winTimer2;


                }
                if (allTimer >= SetRuntime) { isReachLimit = true; }



            }
            System.Console.WriteLine("總計" + allTimer + "場 Ai1:\t" + winTimer1 + "   Ai2: \t" + winTimer2 + "  平手:\t " + tieTimer);
            System.Console.Read();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BingoController : MonoBehaviour 
{    
    // 資料結構
    BingoBoard m_PlayerBoard = new BingoBoard();
    AiBingoBoard m_ComBoard = new AiBingoBoard();
    // 換誰出手
    enum WhichOne
    {
        Player = 0,
        Ai,
		GameOver,
    }
    WhichOne m_WhichOnePlay = WhichOne.Player;
    bool m_bNeedFlush = false;

    // 顯示相關
    GameObject[,] m_ComGrid;    // 電腦使用的Bingo盤
    GameObject[,] m_PlayerGrid;    // 玩家使用的Bingo盤
    Text m_PlayerLine;
    Text m_ComLine; 


	// 開始時
	void Start () 
    {        
        InitComGrid();
        InitPlayGrid();
        GameObject tmpObj = null;
        tmpObj = GameObject.Find("PlayerLineTxt");
        m_PlayerLine = tmpObj.GetComponent<Text>();
        tmpObj = GameObject.Find("ComLineTxt");
        m_ComLine = tmpObj.GetComponent<Text>();

        m_PlayerBoard.InitBoard();
        m_ComBoard.InitBoard();
        m_bNeedFlush = true;
	}
	
	// GameLoop
	void Update () 
    {
		// 換AI出手
        if(m_WhichOnePlay == WhichOne.Ai)
        {
            int NextNumber = m_ComBoard.GetNextNumber();
            m_ComBoard.SetNumber(NextNumber);
            m_PlayerBoard.SetNumber(NextNumber);
            m_bNeedFlush = true;
            m_WhichOnePlay = WhichOne.Player;
        }

        // 顯示雙方賓果盤
        if (m_bNeedFlush)
        {             
            // 計算雙方分數及顯示
            int PlayerLine = m_PlayerBoard.CountLine();
			m_PlayerLine.text = string.Format("目前連線數:{0}", PlayerLine);			           
            int ComLine = m_ComBoard.CountLine();
			m_ComLine.text = string.Format("目前連線數:{0}", ComLine);

			// 判斷勝利
			if(PlayerLine >=5 && ComLine <5 )
			{
				m_PlayerLine.text += "你勝了!!!";
				m_WhichOnePlay = WhichOne.GameOver;
			}

			if(ComLine >=5 && PlayerLine <5)
			{
				m_ComLine.text += "電腦勝了!!!";
				m_WhichOnePlay = WhichOne.GameOver;
			}

			if (PlayerLine >= 5 && ComLine >= 5) 
			{
				m_PlayerLine.text += "平手!!!";
				m_ComLine.text += "平手了!!!";
				m_WhichOnePlay = WhichOne.GameOver;
			}

			// 顯示Board內容
			ShowPlayerBingoBoard();
			if( m_WhichOnePlay == WhichOne.GameOver)
				ShowComBingoBoard( false );
			else
				ShowComBingoBoard( true );
            m_bNeedFlush = false;
        }
	}
   
    // 顯示玩家的賓果盤
    void ShowPlayerBingoBoard()
    {
        for(int i=0;i<5;++i)
            for(int j=0;j<5;++j)
            {                
				Text theText = m_PlayerGrid[i, j].GetComponentInChildren<Text>();
                theText.text = string.Format("{0}", m_PlayerBoard.m_Board[i, j]);
            }
    }

    // 顯示電腦的賓果盤
    void ShowComBingoBoard(bool bStartMode)
    {
        for (int i = 0; i < 5; ++i)
            for (int j = 0; j < 5; ++j)
            {
                Text theText = m_ComGrid[i, j].GetComponentInChildren<Text>();

				if(!(bStartMode))
				{
					theText.text = "";
					if (m_ComBoard.m_Board [i, j] > 0)
						theText.text = "*";
				}
				else
					theText.text = string.Format("{0}", m_ComBoard.m_Board[i, j]);
            }
    }

    // 產生電腦使用的Bingo盤
    void InitComGrid()
    {
        m_ComGrid = new GameObject[5 , 5];
        GameObject Obj = GameObject.Find("ComBtn"); // 參考的按鈕
        
        // 取得按鈕的長寬 
        RectTransform RectInfo = Obj.GetComponent<RectTransform>();
		float BtnWidth = RectInfo.rect.width;
		float BtnHeight = RectInfo.rect.height;
        // 取得位置
        Vector3 PosInfo = Obj.transform.position;
        int GridCount=0;
        for(int i=0;i<5;++i)
            for(int j=0;j<5;++j)
            {
                GameObject NewObj = null;
                if (i == 0 && j == 0)
                    NewObj = Obj;
                else
                    NewObj = GameObject.Instantiate(Obj);

                // 設定位置                
                m_ComGrid[i,j] = NewObj;
                NewObj.name = String.Format("{0}{1}", i, j);
                NewObj.transform.SetParent(Obj.transform.parent);

                // 設定位置
                float Posx = PosInfo.x + (BtnWidth * j);
                float Posy = PosInfo.y + -(BtnHeight * i);
                NewObj.transform.position = new Vector3(Posx,Posy,0);
            }
    }

    // 產生玩家使用的Bingo盤
    void InitPlayGrid()
    {
        m_PlayerGrid = new GameObject[5 , 5];
        GameObject Obj = GameObject.Find("PlayerBtn"); // 參考的按鈕

        // 取得按鈕的長寬 
        RectTransform RectInfo = Obj.GetComponent<RectTransform>();
        float BtnWidth = RectInfo.rect.width;
        float BtnHeight = RectInfo.rect.height;
        // 取得位置
        Vector3 PosInfo = Obj.transform.position;
        int GridCount = 0;
        for (int i = 0; i < 5; ++i)
            for (int j = 0; j < 5; ++j)
            {
                GameObject NewObj = null;
                if (i == 0 && j == 0)                
                    NewObj = Obj;
                else
                    NewObj = GameObject.Instantiate(Obj);
                                    
                // 設定位置                
				m_PlayerGrid[i,j] = NewObj;
                NewObj.name = String.Format("{0}{1}", i, j);
                NewObj.transform.SetParent(Obj.transform.parent);

                // 設定位置
                float Posx = PosInfo.x + (BtnWidth * j);
                float Posy = PosInfo.y + -(BtnHeight * i);
                NewObj.transform.position = new Vector3(Posx, Posy, 0);

                // 設定Button事件
                Button NewButton = NewObj.GetComponent<Button>();
                NewButton.onClick.AddListener(() => OnPlayerBtnClick(NewButton));
            }
    }

    // 玩家按下Btn
    public void OnPlayerBtnClick(Button theButton)
    {
        if (m_WhichOnePlay != WhichOne.Player)
            return;
        //Debug.Log("OnPlayerBtnClick:" + theButton.gameObject.name);

        // 取得按鈕上的值
        Text theText = theButton.GetComponentInChildren<Text>();
        
        // 轉換成數字
        int Number = Int32.Parse(theText.text);
        if (Number > 0)
        { 
            m_PlayerBoard.SetNumber(Number); // 設定為0
            m_ComBoard.SetNumber(Number);
			m_bNeedFlush = true;
			m_WhichOnePlay = WhichOne.Ai;
        }
    }
    public  void ChangeButtonColor(int Number)
    {
        Button theButton;
        for(int c =0;c<5;c++)
            for(int r = 0; r < 5; r++)
            {
                if (m_Board[i, j] == Value)
                    theButton = m_ComGrid[c, r];
            }
        
        //Change Button color
        ColorBlock cb = theButton.colors;
        cb.normalColor = Color.red;
        theButton.colors = cb;
    }
}

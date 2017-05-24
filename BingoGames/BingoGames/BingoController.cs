/*

using System.Collections;
using System.Collections.Generic;
using System;/*
 /*
public class BingoController 
{    

	// 資料結構
	AiBingoBoard m_ComBoard1 = new AiBingoBoard();
	OtherAiBingoBoard m_ComBoard2 = new OtherAiBingoBoard();
	// 換誰出手
	enum WhichOne
	{
		Ai2 = 0,
		Ai1,
		GameOver,
	}
	WhichOne m_WhichOnePlay = WhichOne.Ai2;

	// 顯示相關
	//GameObject[,] m_ComGrid1;    // 電腦使用的Bingo盤
	//GameObject[,] m_ComGrid2;    // 玩家使用的Bingo盤
	//Text m_ComLine1;
	//Text m_ComLine2;
	bool isGameOver = false;
	bool isReachLimit = false;
	int winTimer1 = 0;
	int winTimer2 = 0;
	int tieTimer = 0;

	int useGridTimer1 = 0;
	int useGridTimer2 = 0;
	int avg1 = 0;
	int avg2 = 0;

	// 開始時
	void Start ()
	{        
		InitCom1Grid ();
		InitCom2Grid ();
		GameObject tmpObj = null;
		tmpObj = GameObject.Find ("PlayerLineTxt");
		m_ComLine2 = tmpObj.GetComponent<Text> ();
		tmpObj = GameObject.Find ("ComLineTxt");
		m_ComLine1 = tmpObj.GetComponent<Text> ();

		m_ComBoard1.InitBoard ();
		m_ComBoard2.InitBoard ();
	}

	// GameLoop
	void Update ()
	{
		if (isReachLimit)
			return;
		// 換AI出手
		if (m_WhichOnePlay == WhichOne.Ai1) {
			int NextNumber = m_ComBoard1.GetNextNumber ();
			m_ComBoard1.SetNumber (NextNumber);
			m_ComBoard2.SetNumber (NextNumber);
			m_WhichOnePlay = WhichOne.Ai2;
			useGridTimer1++;
		} else {
			int NextNumber = m_ComBoard2.GetNextNumber ();
			m_ComBoard2.SetNumber (NextNumber);
			m_ComBoard1.SetNumber (NextNumber);
			m_WhichOnePlay = WhichOne.Ai1;
			useGridTimer2++;
		}

		// 顯示雙方賓果盤          
		// 計算雙方分數及顯示		           
		int ComLine = m_ComBoard1.CountLine ();
		//m_ComLine1.text = string.Format ("目前連線數:{0}", ComLine);
		int ComLine2 = m_ComBoard2.CountLine ();
//		m_ComLine2.text = string.Format ("目前連線數:{0}", ComLine2);	

		// 判斷勝利
		if (ComLine2 >= 5 && ComLine < 5) {
			m_WhichOnePlay = WhichOne.GameOver;
			winTimer2++;
			isGameOver = true;
		}

		if (ComLine >= 5 && ComLine2 < 5) {
			m_WhichOnePlay = WhichOne.GameOver;
			winTimer1++;
			isGameOver = true;
		}

		if (ComLine2 >= 5 && ComLine >= 5) {
			m_WhichOnePlay = WhichOne.GameOver;
			tieTimer++;
			isGameOver = true;
		}


		// 顯示Board內容
		ShowComBingoBoard ();
		ShowCom2BingoBoard ();
		m_ComLine2.text = "總共:" + (winTimer1 + winTimer2 + tieTimer) + "場" + "  平手:" + tieTimer + "場";
		if (winTimer1 + winTimer2 + tieTimer > 999) {
			isReachLimit = true;
		} else if (isGameOver) {
			m_ComBoard1.InitBoard ();
			m_ComBoard2.InitBoard ();
			ResetButton ();
			isGameOver = false;
			avg1 += useGridTimer1;
			avg2 += useGridTimer2;
			useGridTimer1 = 0;
			useGridTimer2 = 0;
			WhichOne m_WhichOnePlay = WhichOne.Ai1;
			m_ComLine1.text = "Ai1:  " + winTimer1 + "場(" + string.Format("{0:0.000}", ((float)avg1/(winTimer1+winTimer2))) + ")   versus   " + "Ai2:  " + winTimer2 + "場(" + string.Format("{0:0.000}", ((float)avg2/(winTimer1+winTimer2))) + ")";
		}


//		DateTime time_start = DateTime.Now;//計時開始 取得目前時間
//		DateTime time_end = DateTime.Now;//計時結束 取得目前時間
//		//後面的時間減前面的時間後 轉型成TimeSpan即可印出時間差
//		string result2 = ((TimeSpan)(time_end - time_start)).TotalMilliseconds.ToString();
//		Debug.Log (result2);
	}

	void ResetButton()
	{
		for (int i = 0; i < 5; ++i)
			for (int j = 0; j < 5; ++j) {
				ColorBlock color;

				Button theButton1 = m_ComGrid1 [i, j].GetComponentInChildren<Button> ();
				color = theButton1.colors;
				color.normalColor = Color.white;
				theButton1.colors = color;

				Button theButton2 = m_ComGrid2 [i, j].GetComponentInChildren<Button> ();
				color = theButton2.colors;
				color.normalColor = Color.white;
				theButton2.colors = color;
			}
	
	}

	// 顯示Ai1的賓果盤
	void ShowComBingoBoard()
	{
		for (int i = 0; i < 5; ++i)
			for (int j = 0; j < 5; ++j)
			{
				Text theText = m_ComGrid1[i, j].GetComponentInChildren<Text>();
				Button theButton = m_ComGrid1[i, j].GetComponentInChildren<Button>();
				if (m_ComBoard1.m_Board [i, j] == 0) {
					theText.text = "*";
					ColorBlock color = theButton.colors;
					color.normalColor = Color.blue;
					theButton.colors = color;
				}
				else
					theText.text = string.Format("{0}", m_ComBoard1.m_Board[i, j]);
			}
	}

	// 顯示Ai2的賓果盤
	void ShowCom2BingoBoard()
	{
		for (int i = 0; i < 5; ++i)
			for (int j = 0; j < 5; ++j)
			{
				Text theText = m_ComGrid2[i, j].GetComponentInChildren<Text>();
				Button theButton = m_ComGrid2[i, j].GetComponentInChildren<Button>();
				if (m_ComBoard2.m_Board [i, j] == 0) {
					theText.text = "*";
					ColorBlock color = theButton.colors;
					color.normalColor = Color.red;
					theButton.colors = color;
				}
				else
					theText.text = string.Format("{0}", m_ComBoard2.m_Board[i, j]);
			}
	}

    // 產生電腦使用的Bingo盤
    void InitCom1Grid()
	{
        /*
		m_ComGrid1 = new GameObject[5, 5];
		GameObject Obj = GameObject.Find ("ComBtn"); // 參考的按鈕
        
		// 取得按鈕的長寬 
		RectTransform RectInfo = Obj.GetComponent<RectTransform> ();
		float BtnWidth = RectInfo.rect.width;
		float BtnHeight = RectInfo.rect.height;
		// 取得位置
		Vector3 PosInfo = Obj.transform.position;
		int GridCount = 0;
		for (int i = 0; i < 5; i++)
			for (int j = 0; j < 5; j++) {
				GameObject NewObj = null;
				if (i == 0 && j == 0)
					NewObj = Obj;
				else
					NewObj = GameObject.Instantiate (Obj);

				// 設定位置                
				m_ComGrid1 [i, j] = NewObj;
				NewObj.name = String.Format ("{0}{1}", i, j);
				NewObj.transform.SetParent (Obj.transform.parent);

				// 設定位置
				float Posx = PosInfo.x + (BtnWidth * j);
				float Posy = PosInfo.y + -(BtnHeight * i);
				NewObj.transform.position = new Vector3 (Posx, Posy, 0);
                
			}
            
	}

    // 產生玩家使用的Bingo盤
    /*
     * void InitCom2Grid()
	{
		m_ComGrid2 = new GameObject[5, 5];
		GameObject Obj = GameObject.Find ("PlayerBtn"); // 參考的按鈕

		// 取得按鈕的長寬 
		RectTransform RectInfo = Obj.GetComponent<RectTransform> ();
		float BtnWidth = RectInfo.rect.width;
		float BtnHeight = RectInfo.rect.height;
		// 取得位置
		Vector3 PosInfo = Obj.transform.position;
		int GridCount = 0;
		for (int i = 0; i < 5; i++)
			for (int j = 0; j < 5; j++) {
				GameObject NewObj = null;
				if (i == 0 && j == 0)
					NewObj = Obj;
				else
					NewObj = GameObject.Instantiate (Obj);
                                    
				// 設定位置                
				m_ComGrid2 [i, j] = NewObj;
				NewObj.name = String.Format ("{0}{1}", i, j);
				NewObj.transform.SetParent (Obj.transform.parent);

				// 設定位置
				float Posx = PosInfo.x + (BtnWidth * j);
				float Posy = PosInfo.y + -(BtnHeight * i);
				NewObj.transform.position = new Vector3 (Posx, Posy, 0);
			}
	}
    */


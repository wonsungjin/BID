using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using TMPro;

public class MetaTrendAPI : MonoBehaviour
{
	[SerializeField] TMP_InputField txtInputField;
	[SerializeField] string selectedBettingID;

	[Header("[등록된 프로젝트에서 획득가능한 API 키]")]
	[SerializeField] string API_KEY = "";

	[Header("[Betting Backend Base URL]")]
	[SerializeField] string FullAppsProductionURL = "https://odin-api.browseosiris.com";
	[SerializeField] string FullAppsStagingURL = "https://odin-api-sat.browseosiris.com";

	string getBaseURL()
	{
		// 프로덕션 단계라면
		//return FullAppsProductionURL;

		// 스테이징 단계(개발)라면
		return FullAppsStagingURL;
	}

	Res_UserProfile res_UserProfile = null;
	Res_UserSessionID res_UserSessionID = null;
	Res_BettingSetting res_BettingSetting = null;
	//---------------
	// 유저 정보
	public void OnClick_GetUserProfile()
	{
		StartCoroutine(processRequestGetUserInfo());
	}
	IEnumerator processRequestGetUserInfo()
	{
		// 유저 정보
		yield return requestGetUserInfo((response) =>
		{
			if (response != null)
			{
				Debug.Log("## " + response.ToString());
				res_UserProfile = response;
				Debug.Log("동푸" + res_UserProfile.userProfile.username);
			}
		});
	}
	delegate void resCallback_GetUserInfo(Res_UserProfile response);
	IEnumerator requestGetUserInfo(resCallback_GetUserInfo callback)
	{
		// get user profile
		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8546/api/getuserprofile");
		yield return www.SendWebRequest();
		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_UserProfile res_getUserProfile = JsonUtility.FromJson<Res_UserProfile>(www.downloadHandler.text);
		callback(res_getUserProfile);
	}

	//---------------
	// Session ID
	public void OnClick_GetSessionID()
	{
		StartCoroutine(processRequestGetSessionID());
	}
	IEnumerator processRequestGetSessionID()
	{
		// 유저 정보
		yield return requestGetSessionID((response) =>
		{
			if (response != null)
			{
				Debug.Log("## " + response.ToString());
				res_UserSessionID = response;
			}
		});
	}
	delegate void resCallback_GetSessionID(Res_UserSessionID response);
	IEnumerator requestGetSessionID(resCallback_GetSessionID callback)
	{
		// get session id
		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8546/api/getsessionid");
		yield return www.SendWebRequest();
		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_UserSessionID res_getSessionID = JsonUtility.FromJson<Res_UserSessionID>(www.downloadHandler.text);
		callback(res_getSessionID);
	}

	//---------------
	// 베팅관련 셋팅 정보를 얻어오기
	public void OnClick_Settings()
	{
		StartCoroutine(processRequestSettings());
	}
	IEnumerator processRequestSettings()
	{
		yield return requestSettings((response) =>
		{
			if (response != null)
			{
				Debug.Log("## Settings : " + response.ToString());
				res_BettingSetting = response;
				Debug.Log(res_BettingSetting);
			}
		});
	}
	delegate void resCallback_Settings(Res_BettingSetting response);
	IEnumerator requestSettings(resCallback_Settings callback)
	{
		string url = getBaseURL() + "/v1/betting/settings";


		UnityWebRequest www = UnityWebRequest.Get(url);
		www.SetRequestHeader("api-key", API_KEY);
		yield return www.SendWebRequest();
		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_BettingSetting res = JsonUtility.FromJson<Res_BettingSetting>(www.downloadHandler.text);
		callback(res);
		//UnityWebRequest www = new UnityWebRequest(URL);
	}

	//---------------
	// Zera 잔고 확인
	public void OnClick_ZeraBalance()
	{
		StartCoroutine(processRequestZeraBalance());
	}
	IEnumerator processRequestZeraBalance()
	{
		yield return requestZeraBalance(res_UserSessionID.sessionId, (response) =>
		{
			if (response != null)
			{
				Debug.Log("## Response Zera Balance : " + response.ToString());
			}
		});
	}
	delegate void resCallback_BalanceInfo(Res_ZeraBalance response);
	IEnumerator requestZeraBalance(string sessionID, resCallback_BalanceInfo callback)
	{
		string url = getBaseURL() + ("/v1/betting/" + "zera" + "/balance/" + sessionID);

		UnityWebRequest www = UnityWebRequest.Get(url);
		www.SetRequestHeader("api-key", API_KEY);
		yield return www.SendWebRequest();
		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_ZeraBalance res = JsonUtility.FromJson<Res_ZeraBalance>(www.downloadHandler.text);
		callback(res);
		//UnityWebRequest www = new UnityWebRequest(URL);
	}

	//---------------
	// ZERA 베팅
	public void OnClick_Betting_Zera()
	{
		StartCoroutine(processRequestBetting_Zera());
	}
	IEnumerator processRequestBetting_Zera()
	{
		Res_Initialize resBettingPlaceBet = null;
		Req_Initialize reqBettingPlaceBet = new Req_Initialize();
		reqBettingPlaceBet.players_session_id = new string[] { res_UserSessionID.sessionId };
		reqBettingPlaceBet.bet_id = selectedBettingID;// resSettigns.data.bets[0]._id;
		yield return requestCoinPlaceBet(reqBettingPlaceBet, (response) =>
		{
			if (response != null)
			{
				Debug.Log("## CoinPlaceBet : " + response.message);
				resBettingPlaceBet = response;
			}
		});
	}
	delegate void resCallback_BettingPlaceBet(Res_Initialize response);
	IEnumerator requestCoinPlaceBet(Req_Initialize req, resCallback_BettingPlaceBet callback)
	{
		string url = getBaseURL() + "/v1/betting/" + "zera" + "/place-bet";

		string reqJsonData = JsonUtility.ToJson(req);
		Debug.Log(reqJsonData);


		UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
		byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
		www.uploadHandler = new UploadHandlerRaw(buff);
		www.SetRequestHeader("api-key", API_KEY);
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();

		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_Initialize res = JsonUtility.FromJson<Res_Initialize>(www.downloadHandler.text);
		callback(res);
	}

	//---------------
	// ZERA 베팅-승자
	public void OnClick_Betting_Zera_DeclareWinner()
	{
		StartCoroutine(processRequestBetting_Zera_DeclareWinner());
	}
	IEnumerator processRequestBetting_Zera_DeclareWinner()
	{
		Res_BettingWinner resBettingDeclareWinner = null;
		Req_BettingWinner reqBettingDeclareWinner = new Req_BettingWinner();
		reqBettingDeclareWinner.betting_id = selectedBettingID;// resSettigns.data.bets[0]._id;
		reqBettingDeclareWinner.winner_player_id = res_UserProfile.userProfile._id;
		yield return requestCoinDeclareWinner(reqBettingDeclareWinner, (response) =>
		{
			if (response != null)
			{
				Debug.Log("## CoinDeclareWinner : " + response.message);
				resBettingDeclareWinner = response;
			}
		});
	}
	delegate void resCallback_BettingDeclareWinner(Res_BettingWinner response);
	IEnumerator requestCoinDeclareWinner(Req_BettingWinner req, resCallback_BettingDeclareWinner callback)
	{
		string url = getBaseURL() + "/v1/betting/" + "zera" + "/declare-winner";

		string reqJsonData = JsonUtility.ToJson(req);
		Debug.Log(reqJsonData);


		UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
		byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
		www.uploadHandler = new UploadHandlerRaw(buff);
		www.SetRequestHeader("api-key", API_KEY);
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();

		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_BettingWinner res = JsonUtility.FromJson<Res_BettingWinner>(www.downloadHandler.text);
		callback(res);
	}

	//---------------
	// 베팅금액 반환
	public void OnClick_Betting_Zera_Disconnect()
	{
		StartCoroutine(processRequestBetting_Zera_Disconnect());
	}
	IEnumerator processRequestBetting_Zera_Disconnect()
	{
		Res_BettingDisconnect resBettingDisconnect = null;
		Req_BettingDisconnect reqBettingDisconnect = new Req_BettingDisconnect();
		reqBettingDisconnect.betting_id = selectedBettingID;// resSettigns.data.bets[1]._id;
		yield return requestCoinDisconnect(reqBettingDisconnect, (response) =>
		{
			if (response != null)
			{
				Debug.Log("## CoinDisconnect : " + response.message);
				resBettingDisconnect = response;
			}
		});
	}
	delegate void resCallback_BettingDisconnect(Res_BettingDisconnect response);
	IEnumerator requestCoinDisconnect(Req_BettingDisconnect req, resCallback_BettingDisconnect callback)
	{
		string url = getBaseURL() + "/v1/betting/" + "zera" + "/disconnect";

		string reqJsonData = JsonUtility.ToJson(req);
		Debug.Log(reqJsonData);


		UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
		byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
		www.uploadHandler = new UploadHandlerRaw(buff);
		www.SetRequestHeader("api-key", API_KEY);
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();

		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_BettingDisconnect res = JsonUtility.FromJson<Res_BettingDisconnect>(www.downloadHandler.text);
		callback(res);
	}
}

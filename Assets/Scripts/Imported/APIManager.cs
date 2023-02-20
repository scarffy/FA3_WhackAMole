using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;


[RequireComponent(typeof(DataHandler))]
[RequireComponent(typeof(NetworkHandler))]
public class APIManager : MonoBehaviour
{
    public static APIManager instance;

    public DataHandler dataHandler;
    public NetworkHandler networkHandler;
    public UIHandler uiHandler;

    public bool isRunning;

    object webResponse;

    [TextArea(0,500)]
    public string json;
    [SerializeField] private string fullDataPath;
    private string dataPath = "data.json";

    [DllImport("__Internal")]
    private static extern string GetToken(string key);

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        fullDataPath = System.IO.Path.Combine(Application.streamingAssetsPath, dataPath);

        StartCoroutine(GetMoleData());

        IEnumerator GetMoleData(){
            var wr = UnityWebRequest.Get(fullDataPath);
            wr.SendWebRequest();

            if(wr.error != null)
                yield break;
            else
                //! load the data
                json = wr.downloadHandler.text;
        }

        dataHandler = GetComponent<DataHandler>();
        uiHandler = FindObjectOfType<UIHandler>();
    }

    public IEnumerator PostToGetUserModelCo(string arg = null)
    {
        isRunning = true;
        yield return new WaitUntil(() => !networkHandler.isRunning);

        string token = "QPFJZvrGSgLcpS-vnrNl6OSUhyXnjIMwrRTQ1dEPO6Ekm8hTMBZO13pnMN49DHzzzIAtgZOqcYuqWwnHJ_JGgFRs-daA5UZzcdZKDbCZZIFMpjDZXgovcpdwKjkIcIuR6f0XcfvrBByhBuSNb4laKAiy0bns9PIHIr25pDInIONbdvOOBUjV87ZF97uDoTyRHRZIWlaOkPCMnpJxTv3JB7TF0pmTdyCFQUbLE45SAtDPVoEm";

        CheckUserToken(token);

        string url = "https://my-json-server.typicode.com/KazT452/demo/db";

        uiHandler.ToggleDisplay("Loading", true);

        if (!string.IsNullOrEmpty(arg))
        {
            var jsonText = DecryptUserModel(arg);
            webResponse = jsonText;
        }
        else
        {
            var jsonText = DecryptUserModel(json);
            webResponse = jsonText;
        }

        if (webResponse != null && webResponse.GetType() == typeof(UserModel))
        {
            UserModel response = (UserModel)webResponse;

            dataHandler.userData.userModel.chances = response.chances;

            dataHandler.userData.userModel.moleParam = response.moleParam;

            webResponse = null;
        }
        uiHandler.ToggleDisplay("Loading", false);
        isRunning = false;
    }

    public IEnumerator GetRewardCo(string arg = null)
    {
        isRunning = true;
        yield return new WaitUntil(() => !networkHandler.isRunning);

        //string token = GetToken("Token"); //CHANGE BACK TO THIS WHEN DEPLOY

        string token = "QPFJZvrGSgLcpS-vnrNl6OSUhyXnjIMwrRTQ1dEPO6Ekm8hTMBZO13pnMN49DHzzzIAtgZOqcYuqWwnHJ_JGgFRs-daA5UZzcdZKDbCZZIFMpjDZXgovcpdwKjkIcIuR6f0XcfvrBByhBuSNb4laKAiy0bns9PIHIr25pDInIONbdvOOBUjV87ZF97uDoTyRHRZIWlaOkPCMnpJxTv3JB7TF0pmTdyCFQUbLE45SAtDPVoEm";

        CheckUserToken(token);

        string url = "https://my-json-server.typicode.com/KazT452/demo/db";

        uiHandler.ToggleDisplay("Loading", true);

        if (!string.IsNullOrEmpty(arg))
        {
            var jsonText = DecryptUserModel(arg);
            webResponse = jsonText;
        }
        else
        {
            var jsonText = DecryptUserModel(json);
            webResponse = jsonText;
        }

        if (webResponse != null && webResponse.GetType() == typeof(GameModel))
        {
            GameModel response = (GameModel)webResponse;

            dataHandler.userData.gameModel.rewardType = response.rewardType;

            webResponse = null;
        }
        uiHandler.ToggleDisplay("Loading", false);
        isRunning = false;
    }

    //public IEnumerator PostToGetPrizeResultCo()
    //{
    //    isRunning = true;
    //    yield return new WaitUntil(() => !networkHandler.isRunning);

    //    string token = GetToken("Token"); //CHANGE BACK TO THIS WHEN DEPLOY

    //    //string token = "QPFJZvrGSgLcpS-vnrNl6OSUhyXnjIMwrRTQ1dEPO6Ekm8hTMBZO13pnMN49DHzzzIAtgZOqcYuqWwnHJ_JGgFRs-daA5UZzcdZKDbCZZIFMpjDZXgovcpdwKjkIcIuR6f0XcfvrBByhBuSNb4laKAiy0bns9PIHIr25pDInIONbdvOOBUjV87ZF97uDoTyRHRZIWlaOkPCMnpJxTv3JB7TF0pmTdyCFQUbLE45SAtDPVoEm";

    //    CheckUserToken(token);

    //    string url = "http://glaunch.crococodile.biz/GameScore";
    //    code = dataHandler.userData.code;

    //    uiHandler.ToggleDisplay("Loading", true);

    //    yield return networkHandler.PostRequestCo<GameScoreRS>(token, url, code, true);

    //    if (webResponse != null && webResponse.GetType() == typeof(GameScoreRS))
    //    {
    //        GameScoreRS response = (GameScoreRS)webResponse;

    //        CheckResponseCode(response);

    //        //convert string to double
    //        double chance = double.Parse(response.@return.balance);
    //        //convert string to int
    //        int prizeID = int.Parse(response.@return.prizeId);

    //        dataHandler.userData.chances = chance;
                
    //        dataHandler.wheelData.resultID = prizeID;  //change to int

    //        dataHandler.wheelData.resultName = response.@return.reward;

    //        webResponse = null;
    //    }
    //    uiHandler.ToggleDisplay("Loading", false);
    //    isRunning = false;

    //}

    public IEnumerator PostScoreCo(GameModel score)
    {
        isRunning = true;
        yield return new WaitUntil(() => !networkHandler.isRunning);

        string token = GetToken("Token"); //CHANGE BACK TO THIS WHEN DEPLOY

        //string token = "QPFJZvrGSgLcpS-vnrNl6OSUhyXnjIMwrRTQ1dEPO6Ekm8hTMBZO13pnMN49DHzzzIAtgZOqcYuqWwnHJ_JGgFRs-daA5UZzcdZKDbCZZIFMpjDZXgovcpdwKjkIcIuR6f0XcfvrBByhBuSNb4laKAiy0bns9PIHIr25pDInIONbdvOOBUjV87ZF97uDoTyRHRZIWlaOkPCMnpJxTv3JB7TF0pmTdyCFQUbLE45SAtDPVoEm";

        CheckUserToken(token);

        string url = "Insert URL here";

        uiHandler.ToggleDisplay("Loading", true);

        yield return networkHandler.PostRequestCo<GameModel>(token, url, score, true);

        if (webResponse != null)
        {
            ////convert string to int
            //int prizeID = int.Parse(prizes.ID);

            //dataHandler.userData.chances -= 1;

            //dataHandler.wheelData.resultID = prizeID;

            //dataHandler.wheelData.resultName = prizes.Name;

            webResponse = null;
        }

        uiHandler.ToggleDisplay("Loading", false);
        isRunning = false;
    }

    /// <summary>
    /// This is for communication between website and unity
    /// </summary>
    /// <param name="json">take string as parameter</param>
    /// <returns></returns>
    public IEnumerator PostScoreCo(string json)
    {
        isRunning = true;
        yield return new WaitUntil(() => !networkHandler.isRunning);

        string token = GetToken("Token"); //CHANGE BACK TO THIS WHEN DEPLOY

        //string token = "QPFJZvrGSgLcpS-vnrNl6OSUhyXnjIMwrRTQ1dEPO6Ekm8hTMBZO13pnMN49DHzzzIAtgZOqcYuqWwnHJ_JGgFRs-daA5UZzcdZKDbCZZIFMpjDZXgovcpdwKjkIcIuR6f0XcfvrBByhBuSNb4laKAiy0bns9PIHIr25pDInIONbdvOOBUjV87ZF97uDoTyRHRZIWlaOkPCMnpJxTv3JB7TF0pmTdyCFQUbLE45SAtDPVoEm";

        CheckUserToken(token);

        string url = "Insert URL here";

        uiHandler.ToggleDisplay("Loading", true);

        GameModel score = DecryptGameModel(json);

        yield return networkHandler.PostRequestCo<GameModel>(token, url, score, true);

        if (webResponse != null)
        {
            ////convert string to int
            //int prizeID = int.Parse(prizes.ID);

            //dataHandler.userData.chances -= 1;

            //dataHandler.wheelData.resultID = prizeID;

            //dataHandler.wheelData.resultName = prizes.Name;

            webResponse = null;
        }

        uiHandler.ToggleDisplay("Loading", false);
        isRunning = false;
    }

    //void CheckResponseCode(GameScoreRS response)
    //{
    //    if (response.responseCode == "000000")
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        string errorTxt = $"INTERNAL SERVER ERROR ({response.responseCode})";
    //        networkHandler.HandleError(errorTxt, true);
    //    }
    //}

    //void CheckResponseCode(GameSettingRS response)
    //{
    //    if (response.responseCode == "000000")
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        string errorTxt = $"INTERNAL SERVER ERROR ({response.responseCode})";
    //        networkHandler.HandleError(errorTxt, false);
    //    }
    //}

    public void CheckUserToken(string token)
    {
        if(!string.IsNullOrEmpty(token))
        {
            return;
        }
        else
        {
            string errorTxt = "INTERNAL SERVER ERROR (TOKEN IS NULL OR EMPTY)";
            networkHandler.HandleError(errorTxt, false);
        }
    }

    //void StoreUserData(UserModel response)
    //{
    //    dataHandler.userData.userName = response.userName;
    //    dataHandler.userData.chances = response.chance;
    //}

    public object SetWebResponseResult
    {
        set { webResponse = value; }
    }

    //public SceneHandler SetSceneHandler
    //{
    //    set { uiHandler = value; } 
    //}

    //public IEnumerator GetUserModelCo()
    //{
    //    yield return new WaitUntil(() => !networkHandler.isRunning);

    //    string userName = dataHandler.userData.userName;
    //    string token = GetToken("Token");
    //    string url = "";

    //    sceneHandler.DisplayLoading(true);

    //    yield return networkHandler.GetRequestCo<bool>(token, url);

    //    if (webResponse != null && webResponse.GetType() == typeof(UserModel))
    //    {
    //        UserModel response = (UserModel)webResponse;

    //        StoreUserData(response);
    //        webResponse = null;
    //    }
    //    sceneHandler.DisplayLoading(false);
    //}

    //public IEnumerator GetPrizeListCo()
    //{
    //    yield return new WaitUntil(() => !networkHandler.isRunning);

    //    string token = "insert token here";
    //    string url = "insert url here";

    //    sceneHandler.DisplayLoading(true);

    //    yield return networkHandler.GetRequestCo<GameSettingRS>(token, url);

    //    if (webResponse != null && webResponse.GetType() == typeof(GameSettingRS))
    //    {
    //        GameSettingRS response = (GameSettingRS)webResponse;
    //        if (string.IsNullOrEmpty(response.@return.config))
    //        {
    //            Prize[] items = JsonConvert.DeserializeObject<Prize[]>(response.@return.config);
    //            dataHandler.wheelData.wheelPieces = new Prize[items.Length];

    //            for (int i = 0; i < items.Length; i++)
    //            {
    //                dataHandler.wheelData.wheelPieces[i] = items[i];
    //            }
    //        }
    //        webResponse = null;
    //    }
    //    sceneHandler.DisplayLoading(false);
    //}

    static UserModel DecryptUserModel(string json)
    {
        return JsonUtility.FromJson<UserModel>(json);
    }

    static GameModel DecryptGameModel(string json) => JsonUtility.FromJson<GameModel>(json);
    
}

    



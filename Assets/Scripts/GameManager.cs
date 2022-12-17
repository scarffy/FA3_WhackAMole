using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIHandler uiHandler;
    private DataHandler dataHandler;
    private APIManager apiManager;
    private NetworkHandler networkHandler;
    public enum GameType { Short, Medium, Long };
    public GameType gameType;

    public float accumulatedWeight;
    private List<int> nonZeroChancesIndices = new List<int>();
    [SerializeField] private List<Mole> moles;
    [SerializeField] private GameObject playButton;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool rewardFromAPI = false;
    [Space]
    [Header("Game Parameters")]
    public float startingTime;
    public float timeRemaining;
    public float countDownTime;
    public float chance;
    public float score;
    public MoleParam currMoleParam;
    [HideInInspector] public bool running;
    [HideInInspector] public float cdTime;
    [HideInInspector] public float roundedCredit;
    [HideInInspector] public bool selectingReward;
    [HideInInspector] public bool treasureOut;
    [HideInInspector] public bool treasureOutClose;
    public bool isCountDown = false;
    public bool playable = false;
    public bool playing = false;
    public bool hitTreasure = false;

    private MoleParam[] moleParam;
    private HashSet<Mole> currentMoles = new HashSet<Mole>();
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        uiHandler = FindObjectOfType<UIHandler>();
        networkHandler = FindObjectOfType<NetworkHandler>();
        dataHandler = FindObjectOfType<DataHandler>();
    }

    private void Start()
    {
        ResetGame();
    }

    void Update()
    {
        if (isCountDown)
        {
            if (cdTime > 0f)
            {
                cdTime -= Time.deltaTime;
            }
            else
            {
                isCountDown = false;
                playing = true;
            }
        }

        if (playing)
        {
            Cursor.visible = false;
            timeRemaining -= Time.deltaTime;
            if(timeRemaining <= 0)
            {
                timeRemaining = 0;
                EndGame(0);
            }

            if(timeRemaining < startingTime / 2 && !treasureOutClose)
            {
                treasureOut = true;
            }

            if(currentMoles.Count <= (score / 10))
            {
                int index = UnityEngine.Random.Range(0, moles.Count);
                if (!currentMoles.Contains(moles[index]))
                {
                    if (treasureOut && !treasureOutClose)
                    {
                        SetTreasure();
                    }
                    else
                    {
                        currentMoles.Add(moles[index]);
                        moles[index].Activate(score / 10);
                    }
                }
            }
        }
        else
        {
            Cursor.visible = true;
        }
    }

    public void GameTypeSet(string type) //Button function
    {
        if(type == "Short")
        {
            SetGameType(GameType.Short);
        }
        else if (type == "Medium")
        {
            SetGameType(GameType.Medium);
        }
        else if (type == "Long")
        {
            SetGameType(GameType.Long);
        }
    }

    public void SetGameType(GameType gametype)
    {
        gameType = gametype;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCo());
    }

    public IEnumerator StartGameCo()
    {
        CalculateWeightAndIndices();

        switch (gameType)
        {
            case GameType.Short:
                startingTime = 3f;
                break;

            case GameType.Medium:
                startingTime = 5f;
                break;

            case GameType.Long:
                startingTime = 10f;
                break;
            default:
                startingTime = 10f;
                break;
        }

        uiHandler.ToggleDisplay("PlayBtn", false);
        isCountDown = true;
        cdTime = countDownTime;
        timeRemaining = startingTime;
        score = 0;

        for (int i = 0; i < moles.Count; i++)
        {
            moles[i].Hide();
            moles[i].SetIndex(i);
        }
        currentMoles.Clear();

        yield return new WaitUntil(() => !isCountDown);     
        playing = true;
    }

    public void EndGame(int type)
    {
        foreach(Mole mole in moles)
        {
            mole.StopGame();
        }

        playing = false;
        uiHandler.ToggleDisplay("TimeUp", true);
        StartCoroutine(ShowResult());
        //uiHandler.ToggleDisplay("PlayBtn", true);
    }

    public void ResetGame()
    {
        StartCoroutine(ResetGameCo());
    }

    public IEnumerator ResetGameCo()
    {
        cdTime = countDownTime;
        timeRemaining = startingTime;
        score = 0;
        accumulatedWeight = 0;
        hitTreasure = false;
        treasureOut = false;
        treasureOutClose = false;

        for (int i = 0; i < moles.Count; i++)
        {
            moles[i].Hide();
            moles[i].SetIndex(i);
        }
        currentMoles.Clear();

        uiHandler.ToggleDisplay("Loading", true);

        yield return APIManager.instance.PostToGetUserModelCo();

        uiHandler.ToggleDisplay("Loading", false);

        if (!string.IsNullOrEmpty(networkHandler.errorMessage))
        {
            uiHandler.ToggleDisplay("Error", true, false, networkHandler.errorMessage);
        }
        else
        {
            dataHandler.userData.userModel.chances = chance;
        }

        if(chance <= 0)
        {
            playable = false;
        }
        else
        {
            playable = true;
        }
        uiHandler.ToggleDisplay("PlayBtn", true);
    }

    public IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(2f);
        uiHandler.ToggleDisplay("TimeUp", false);
        //uiHandler.ToggleDisplay("TimeUp", false);
        //uiHandler.ToggleDisplay("Result", true);
        //uiHandler.resultScoreText.text = score.ToString();
        StartCoroutine(SubmitScore());
    }

    public IEnumerator SubmitScore()
    {
        //ScoreModel scoreModel = new ScoreModel
        //{ userName = APIManager.instance.dataHandler.userData.userModel.userName,
        //score = playerScore};
        uiHandler.resultScoreText.text = score.ToString("F2");
        uiHandler.ToggleDisplay("Result", true);
        
        uiHandler.ToggleDisplay("Loading", true);
        //yield return APIManager.instance.PostScoreCo(scoreModel);
        uiHandler.ToggleDisplay("Loading", false);
        if (hitTreasure)
        {
            SelectReward();
            yield return new WaitUntil(() => !selectingReward);
        }
       
        if (!string.IsNullOrEmpty(networkHandler.errorMessage))
        {
            uiHandler.ToggleDisplay("Error", true, false, networkHandler.errorMessage);
        }
        //toberemoved
        yield return null;
    }

    public void AddScore(int moleIndex, MoleParam moleScore)
    {
        //Do something when player hit mole
        score += moleScore.score;
        currentMoles.Remove(moles[moleIndex]);
    }

    public void Missed(int moleIndex, bool isMole)
    {
        //Do something when player missed
        currentMoles.Remove(moles[moleIndex]);
    }

    public void SelectReward()
    {
        selectingReward = true;
        uiHandler.ToggleDisplay("TreasureCanvas", true);
    }

    public void CallReward() //Button function
    {
        StartCoroutine(CallRewardCo());
    }

    public IEnumerator CallRewardCo()
    {
        if (rewardFromAPI)
        {
            running = true;
            yield return apiManager.GetRewardCo();

            switch (dataHandler.userData.gameModel.rewardType)
            {
                case GameModel.RewardType.Credit:
                    float credit = UnityEngine.Random.Range(5f, 10f);
                    roundedCredit = (float)Mathf.Round(credit * 100f) / 100f;
                    score += roundedCredit;
                    break;

                case GameModel.RewardType.Divide:
                    float newScoreDiv = score * 0.5f;
                    score = newScoreDiv;
                    break;

                case GameModel.RewardType.Multiply:
                    float newScoreMult = score * 1.5f;
                    score = newScoreMult;
                    break;
            }
            running = false;
        }
        else //if not getting reward from api
        {
            running = true;
            GameModel.RewardType reward = (GameModel.RewardType)Random.Range(0, System.Enum.GetValues(typeof(GameModel.RewardType)).Length);
            switch (reward)
            {
                case GameModel.RewardType.Credit:
                    float credit = UnityEngine.Random.Range(5f, 10f);
                    roundedCredit = (float)Mathf.Round(credit * 100f) / 100f;
                    score += roundedCredit;
                    break;

                case GameModel.RewardType.Divide:
                    float newScoreDiv = score * 0.5f;
                    score = newScoreDiv;
                    break;

                case GameModel.RewardType.Multiply:
                    float newScoreMult = score * 1.5f;
                    score = newScoreMult;
                    break;
            }
            running = false;
        }
        yield return new WaitUntil(() => !running);
        uiHandler.DisplayRewardUI();
        selectingReward = false;
    }

    public void SetMole()
    {
        running = true;
        float roll = UnityEngine.Random.Range(0, accumulatedWeight);

        for (int i = 0; i < dataHandler.userData.userModel.moleParam.Length; i++)
        {
            roll -= dataHandler.userData.userModel.moleParam[i].chance;
            if (roll < 0)
            {
                currMoleParam = dataHandler.userData.userModel.moleParam[i];
                break;
            }
        }
        running = false;
    }

    public void SetTreasure()
    {
        //currMoleParam.ID = "Treasure";
        //currMoleParam.score = 0f;
        if (treasureOut && !treasureOutClose)
        {
            int index = UnityEngine.Random.Range(0, moles.Count);
            if (!currentMoles.Contains(moles[index]))
            {
                currentMoles.Add(moles[index]);
                moles[index].moleParam.ID = "Treasure";
                moles[index].moleParam.score = 0;
                moles[index].Activate(score / 10, true);
            }
            treasureOutClose = true;
        }
    }

    public void CalculateWeightAndIndices()
    {
        for (int i = 0; i < dataHandler.userData.userModel.moleParam.Length; i++)
        {
            //add weights:
            accumulatedWeight += dataHandler.userData.userModel.moleParam[i].chance;

            //save non zero chance indices:
            if (dataHandler.userData.userModel.moleParam[i].chance > 0)
            {
                nonZeroChancesIndices.Add(i);
            }

            if (nonZeroChancesIndices.Count == 0)
            {
                Debug.LogError("You can't set all chance to zero");
            }
        }
    }

    public void SpawnTreasure()
    {
        int index = UnityEngine.Random.Range(0, moles.Count);
        if (!currentMoles.Contains(moles[index]))
        {
            currentMoles.Add(moles[index]);
            moles[index].Activate(score / 10, true);
        }
    }

    public void MoleReset()
    {
        SetMole();
        for(int i = 0; i < moles.Count; i++)
        {
            moles[i].SetParam();
        }
    }
}

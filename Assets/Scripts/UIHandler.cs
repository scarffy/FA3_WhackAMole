using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    DataHandler dataHandler;
    GameManager gameManager;

    [SerializeField] private Sprite creditSprite;
    [SerializeField] private Sprite divideSprite;
    [SerializeField] private Sprite multiplySprite;

    public GameObject[] UIObjects;
    public GameObject errorButton;
    public GameObject rewardItem; 

    public Button playButton;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI resultScoreText;
    public TextMeshProUGUI tokenText;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI gameTypeText;
    public TextMeshProUGUI rewardItemText;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        dataHandler = FindObjectOfType<DataHandler>();
        gameManager = FindObjectOfType<GameManager>();
        playButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        scoreText.text = gameManager.score.ToString("F2");
        timerText.text = gameManager.timeRemaining.ToString("F2");
        tokenText.text = $"Tokens: {gameManager.chance}";
        CountDown();
    }

    public void CheckPlayButton()
    {
        if(gameManager.playable)
        {
            playButton.enabled = true;
        }
        else
        {
            playButton.enabled = false;
        }
    }

    public void CountDown()
    {
        if (gameManager.isCountDown && gameManager.cdTime > 0)
        {
            countDownText.text = gameManager.cdTime.ToString("F0");
            ToggleDisplay("CountDown", true);
        }
        else if(gameManager.isCountDown && gameManager.cdTime == 0)
        {
            countDownText.text = "GO!";
            ToggleDisplay("CountDown", true);
        }
        else
        {
            ToggleDisplay("CountDown", false);
        }
    }

    public void ToggleDisplay(string name, bool flag)
    {
        for (int i = 0; i < UIObjects.Length; i++)
        {
            if (UIObjects[i].name == name)
            {
                GameObject panel = UIObjects[i];
                if (panel != null)
                {
                    if (flag == true)
                    {
                        panel.SetActive(true);
                    }
                    else
                    {
                        panel.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError($"Object not found: {panel}");
                }
                break;
            }
        }
    }

    public void ToggleDisplay(string name, bool flag, bool withButton, string errorMsg)
    {
        for (int i = 0; i < UIObjects.Length; i++)
        {
            if (UIObjects[i].name == name)
            {
                GameObject panel = UIObjects[i];
                if (panel != null)
                {
                    if (flag == true)
                    {
                        errorButton.SetActive(withButton);
                        errorText.text = errorMsg;
                        panel.SetActive(true);   
                    }
                    else
                    {
                        panel.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError($"Object not found: {panel}");
                }
                break;
            }
        }
    }

    public void SelectDifficulty(string diff)
    {
        switch (diff)
        {
            case "Short":
                gameTypeText.text = "Type: Short \nTime: 3 Seconds \nCost: 1 Token";
                break;
            case "Medium":
                gameTypeText.text = "Type: Medium \nTime: 5 Seconds \nCost: 2 Tokens";
                break;
            case "Long":
                gameTypeText.text = "Type: Long \nTime: 10 Seconds \nCost: 3 Tokens";
                break;
        }
    }

    public void OpenDisplay(GameObject obj) //button function
    {
        StartCoroutine(OpenDisplayCo(obj));
    }

    public IEnumerator OpenDisplayCo(GameObject obj)
    {
        yield return null;
        obj.SetActive(true);
    }

    public void CloseDisplay(GameObject obj) //button function
    {
        StartCoroutine(CloseDisplayCo(obj));
    }

    public IEnumerator CloseDisplayCo(GameObject obj)
    {
        //if (obj.GetComponentInChildren<Animator>() != null)
        //{
        //    TriggerAnimation(obj, "Out");
        //}
        //yield return new WaitForSeconds(0.2f);
        yield return null;
        obj.SetActive(false);
    }

    public void DisplayRewardUI()
    {
        Image rewardItemImg = rewardItem.GetComponent<Image>();

        switch (dataHandler.userData.gameModel.rewardType)
        {
            case GameModel.RewardType.Credit:
                rewardItemImg.sprite = creditSprite;
                rewardItemText.text = $"Credits +{gameManager.roundedCredit.ToString("F2")}";
                break;

            case GameModel.RewardType.Divide:
                rewardItemImg.sprite = divideSprite;
                rewardItemText.text = $"Credits x0.5";
                break;

            case GameModel.RewardType.Multiply:
                rewardItemImg.sprite = multiplySprite;
                rewardItemText.text = $"Credits x1.5";
                break;
        }

        ToggleDisplay("CongratsPanel", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneHandler : MonoBehaviour
{
        [Header("UI")]
        [SerializeField] GameObject errorCanvas;
        [SerializeField] GameObject loadingCanvas;
        [SerializeField] TMP_Text errorText;
        public GameObject errorButton;
        public GameObject rewardPanel;
        public TextMeshProUGUI rewardTxt;
        public Image rewardImg;
        public Button spinBtn;
        public Text userText;
        public Text chanceText;
        public int resultIndex;
        public string resultLink;

        [Header("Hide")]
        public GameObject pointer;
        public GameObject userTxt;
        public GameObject chanceTxt;

        public bool isShowReward;

        //APIManager apiManager;
        //PickerWheel pickerWheel;
        //DrawElement drawElement;
        //NetworkHandler networkHandler;
        DataHandler dataHandler;

        private void Start()
        {
            //apiManager = FindObjectOfType<APIManager>();
            //pickerWheel = FindObjectOfType<PickerWheel>();
            //networkHandler = FindObjectOfType<NetworkHandler>();
            //drawElement = FindObjectOfType<DrawElement>();
            dataHandler = FindObjectOfType<DataHandler>();
            spinBtn.interactable = true;
            //APIManager.instance.SetSceneHandler = this;
        }

        public void Initialize()
        {
            //userTxt.SetActive(true);
            if (!string.IsNullOrEmpty(chanceText.text))
            {
                chanceTxt.SetActive(true);
            }
            else
            {
                chanceTxt.SetActive(false);
            }
        }

        public void ShowReward()
        {
            if (rewardPanel != null && isShowReward == false)
            {
                //for (int i = 0; i < drawElement.wheelPieces.Length; i++)
                //{
                //    if(resultIndex == i)
                //    {
                //        if (pickerWheel.enableAPIMode)
                //        {
                //            rewardTxt.text = dataHandler.wheelData.resultName;
                //            resultLink = drawElement.wheelPieces[i].ImageSrc;
                //            pickerWheel.RetrievePic(resultLink, rewardImg);
                //        }
                //        else
                //        {
                //            rewardTxt.text = drawElement.wheelPieces[i].Name;
                //            rewardImg.sprite = drawElement.wheelPieces[i].Icon;
                //        }
                //    }
                //}
                isShowReward = true;
                //rewardTxt.text = label;
                //rewardImg.sprite = labelImg;
                rewardPanel.SetActive(true);
            }
        }

        public void SetResultIndex(int index)
        {
            resultIndex = index;
        }

        public void Close(GameObject gameObject)
        {
            gameObject.SetActive(false);
            if (gameObject == rewardPanel)
            {
                isShowReward = false;
                //chanceText.text = $"Chances left: {apiManager.dataHandler.userData.chances}";
            }
        }

        public void SpinChance()
        {
            //if (apiManager.dataHandler.userData.chances >= 1)
            //{
            //    spinBtn.interactable = true;
            //}
            //else
            //{
            //    spinBtn.interactable = false;
            //}
        }

        public void DisplayErrorMessage(string errorMsg, bool withButton)
        {
            errorText.text = errorMsg;
            if (withButton)
            {
                errorButton.SetActive(true);
            }
            else
            {
                errorButton.SetActive(false);
            }
            errorCanvas.SetActive(true);
        }

        public void DisplayLoading(bool isLoading)
        {
            if (isLoading)
            {
                loadingCanvas.SetActive(true);
            }
            else
            {
                loadingCanvas.SetActive(false);
            }
        }
}



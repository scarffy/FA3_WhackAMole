using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;



    public class NetworkHandler : MonoBehaviour
    {
        [HideInInspector]
        public string errorMessage;
        [HideInInspector]
        public bool isRunning = false;

        public bool canBypassError = false;
        //public bool isCustStatCode;

        APIManager apiManager;

        void Start()
        {
            apiManager = GetComponent<APIManager>();
        }

        public IEnumerator GetRequestCo<T>(string token, string url, bool withButton)
        {
            isRunning = true;

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                Int32 unixTimeStamp = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                Guid newGuid = Guid.NewGuid();

                if (!string.IsNullOrEmpty(token)) 
                {
                    www.SetRequestHeader("Authorization", "Bearer " + token);
                    www.SetRequestHeader("X-Request-DT", unixTimeStamp.ToString());
                    www.SetRequestHeader("X-Request-ID", newGuid.ToString());
                }
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                CheckResult<T>(www, withButton);
            }
        }

        public IEnumerator PostRequestCo<T>(string token, string url, UserModel code, bool withButton)
        {
            isRunning = true;

            using (UnityWebRequest www = UnityWebRequest.Post(url, code.ToString()))
            {
                Int32 unixTimeStamp = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                Guid newGuid = Guid.NewGuid();

                if (!string.IsNullOrEmpty(token)) 
                { 
                    www.SetRequestHeader("Authorization", "Bearer " + token);
                    www.SetRequestHeader("X-Request-DT", unixTimeStamp.ToString());
                    www.SetRequestHeader("X-Request-ID", newGuid.ToString());
                }
                www.SetRequestHeader("Content-Type", "application/json");

                string json = JsonConvert.SerializeObject(code);
                byte[] rawData = new System.Text.UTF8Encoding().GetBytes(json);
                www.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawData);
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                CheckResult<T>(www, true);
            }
        }

        public IEnumerator PostRequestCo<T>(string token, string url, GameModel score, bool withButton)
        {
            isRunning = true;

            using (UnityWebRequest www = UnityWebRequest.Post(url, score.ToString()))
            {
                Int32 unixTimeStamp = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                Guid newGuid = Guid.NewGuid();

                if (!string.IsNullOrEmpty(token))
                {
                    www.SetRequestHeader("Authorization", "Bearer " + token);
                    www.SetRequestHeader("X-Request-DT", unixTimeStamp.ToString());
                    www.SetRequestHeader("X-Request-ID", newGuid.ToString());
                }
                www.SetRequestHeader("Content-Type", "application/json");

                string json = JsonConvert.SerializeObject(score);
                byte[] rawData = new System.Text.UTF8Encoding().GetBytes(json);
                www.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawData);
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                CheckResult<T>(www, withButton);
            }
        }

        void CheckResult<T>(UnityWebRequest www, bool withButton)
        {
            errorMessage = string.Empty;

            if (www.result == UnityWebRequest.Result.Success)    //success
            {
                print($"{www.responseCode} ({typeof(T)}) : {www.downloadHandler.text} !");

                if (www.responseCode == 200)
                {
                    if (string.IsNullOrEmpty(www.downloadHandler.text))
                    {
                        //Debug.LogWarning($"{www.responseCode} ({typeof(T)}) : www.downloadHandler.text is null or empty!");
                        string errorTxt ="UNKNOWN";
                        HandleError(errorTxt, withButton);
                    }
                    else
                    {
                        T response = JsonConvert.DeserializeObject<T>(www.downloadHandler.text);
                        apiManager.SetWebResponseResult = response;
                    }
                }
                else
                {
                    HandleError(www.responseCode.ToString(), withButton);
                }
            }
            else //Network error                                                      
            {
                HandleError(www.error, withButton);
            }

            isRunning = false;
        }

        public void HandleError(string error, bool withButton)
        {
            if (!canBypassError)
            {
                errorMessage = error;
                //FindObjectOfType<SceneHandler>().DisplayErrorMessage(errorMessage, withButton);
            }
            //Debug.LogError("Error in API: " + error);
        }
    }



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JsonHandler : MonoBehaviour
{
    public static JsonHandler instance;

    private string moleDataJson;
    [TextArea(0, 500)][SerializeField] private string gameDataJson;

    // Data path
    private string fullDataPath;
    private string moleDataPath = "moleMode.json";
    private string gameModePath = "gameMode.json";

    // System Delegate
    public Action<string> moleDataAction;
    public Action<string> gameModeAction;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void GetJsonData()
    {
        StartCoroutine(GetMoleDataCo(moleDataAction));
        StartCoroutine(GetGameModeCo(gameModeAction));
    }

    IEnumerator GetMoleDataCo(Action<string> moleData = null)
    {
        fullDataPath = System.IO.Path.Combine(Application.streamingAssetsPath, moleDataPath);

        WWW wr = new WWW(fullDataPath);
        yield return wr;
        moleDataJson = wr.text;
        if (moleData != null) moleData?.Invoke(wr.text);

        //! Obsolete WWW does work but WR doesn't
        // var wr = UnityWebRequest.Get(fullDataPath);
        // wr.SendWebRequest();

        // if (wr.error != null)
        // {
        //     Debug.Log("Json error occur: " + wr.error);
        //     yield break;
        // }
        // else
        // {
        //     json = wr.downloadHandler.text;
        //     Debug.Log($"JSON data: {json}");
        // }
    }

    IEnumerator GetGameModeCo(Action<string> moleData = null)
    {
        fullDataPath = System.IO.Path.Combine(Application.streamingAssetsPath, gameModePath);

        WWW wr = new WWW(fullDataPath);
        yield return wr;
        gameDataJson = wr.text;
        if (moleData != null) moleData?.Invoke(wr.text);
    }
}

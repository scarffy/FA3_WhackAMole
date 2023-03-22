using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JsonHandler : MonoBehaviour
{
    public static JsonHandler instance;

    private string moleDataJson;
    private string gameDataJson;

    // Data path
    private string fullDataPath;
    private string moleDataPath = "moleMode.json";
    private string gameModePath = "gameMode.json";

    // System Delegate
    public Action<string> moleDataAction;
    public Action<string> gameModeAction;

    public Mole[] moleScript;

    void Awake()
    {
        if (instance == null)
            instance = this;

        GetImages();
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
    }

    IEnumerator GetGameModeCo(Action<string> moleData = null)
    {
        fullDataPath = System.IO.Path.Combine(Application.streamingAssetsPath, gameModePath);

        WWW wr = new WWW(fullDataPath);
        yield return wr;
        gameDataJson = wr.text;
        if (moleData != null) moleData?.Invoke(wr.text);
    }

    void GetImages()
    {
        Texture2D moleNomalTexture = TextureFromStreamingAssets("MoleNormal");
        Texture2D moleHitTexture = TextureFromStreamingAssets("MoleBonked");

        foreach (var item in moleScript)
        {
            item.SetMoleSprite(ConvertTextureToSprite(moleNomalTexture));
            item.SetMoleHitSprite(ConvertTextureToSprite(moleHitTexture));
        }
    }

    public static Texture2D TextureFromStreamingAssets(string textureName)
    {
        string imageFile = Application.streamingAssetsPath + "/Images/Defaults/" + textureName + ".png";
        byte[] pngBytes = System.IO.File.ReadAllBytes(imageFile);
        Texture2D tex = new Texture2D(2, 2);
        ImageConversion.LoadImage(tex, pngBytes);
        return tex;
    }

    public static Sprite ConvertTextureToSprite(Texture2D tex)
    {
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);
        return sprite;
    }
}

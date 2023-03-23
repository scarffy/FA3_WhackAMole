using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private GameMode gameMode;

    public Mole[] moleScript;

    [Space]
    [SerializeField] private Image backgroundImage;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void GetJsonData()
    {
        StartCoroutine(GetMoleDataCo(moleDataAction));
        StartCoroutine(GetGameModeCo(gameModeAction));

        GetImages();
    }

    IEnumerator GetMoleDataCo(Action<string> moleData = null)
    {
        fullDataPath = System.IO.Path.Combine(Application.streamingAssetsPath, moleDataPath);

        WWW wr = new WWW(fullDataPath);
        yield return wr;
        moleDataJson = wr.text;
        if (moleData != null) moleData?.Invoke(wr.text);
    }

    /// <summary>
    /// Get game mode.
    /// There are 3 game modes. 3 seconds (Short), 5 seconds (Medium), and 10 seconds (Long)
    /// </summary>
    /// <param name="moleData"></param>
    /// <returns></returns>
    IEnumerator GetGameModeCo(Action<string> moleData = null)
    {
        fullDataPath = System.IO.Path.Combine(Application.streamingAssetsPath, gameModePath);

        WWW wr = new WWW(fullDataPath);
        yield return wr;
        gameDataJson = wr.text;
        if (moleData != null) moleData?.Invoke(wr.text);

        gameMode = JsonUtility.FromJson<GameMode>(gameDataJson);
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();

        switch (gameMode.gameMode)
        {
            case "short":
                gameManager.SetGameType(GameManager.GameType.Short);
                break;

            case "medium":
                gameManager.SetGameType(GameManager.GameType.Medium);
                break;
            case "Medium":
                gameManager.SetGameType(GameManager.GameType.Medium);
                break;

            case "long":
                gameManager.SetGameType(GameManager.GameType.Long);
                break;
            case "Long":
                gameManager.SetGameType(GameManager.GameType.Long);
                break;
            default:
            case "Short":
                gameManager.SetGameType(GameManager.GameType.Short);
                break;
        }
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

        Texture2D backgroundTexture = TextureFromStreamingAssets("Background");
        backgroundImage.sprite = ConvertTextureToSprite(backgroundTexture);
    }

    #region Static functions
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
    #endregion
}

[System.Serializable]
public class GameMode
{
    public string gameMode;
}

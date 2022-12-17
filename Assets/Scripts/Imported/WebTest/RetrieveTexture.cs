using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RetrieveTexture : MonoBehaviour
{
    public Image image;
    public string imageLink;

    // Start is called before the first frame update
    void Start()
    {
        image.GetComponent<Image>();
        //test
        PrefabRetrievePic();
    }

    public void PrefabRetrievePic()
    {
        StartCoroutine(RetrievePic());
    }
    public IEnumerator RetrievePic()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageLink);

        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Image Failed {www.error}");
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //Texture2D myTexture = Resources.Load<Texture2D>("Images/QuestionMark");
            image.sprite = Sprite.Create (myTexture, new Rect(0,0, myTexture.width, myTexture.height), new Vector2(myTexture.width/2, myTexture.height/2));           
        }
    }
}

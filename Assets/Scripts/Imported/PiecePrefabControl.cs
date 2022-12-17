using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiecePrefabControl : MonoBehaviour
{
    //public Image pieceImage;
    //public SpinWheel.PickerWheel pickerWheel;

    public string imageLink;

    private void Awake()
    {
        //pieceImage.GetComponent<Image>();
        //pickerWheel = FindObjectOfType<SpinWheel.PickerWheel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //if (pickerWheel.enableAPIMode)
        //{
        //    pickerWheel.RetrievePic(imageLink, pieceImage);
        //}
        //else
        //{
        //    pieceImage.color = new Color(pieceImage.color.r, pieceImage.color.g, pieceImage.color.b, 1f);
        //}
    }
}

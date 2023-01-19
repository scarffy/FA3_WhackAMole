using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Festive Name", menuName = "Tools/Festive UI")]
public class FestiveSeason : ScriptableObject
{
    public string festiveName;

    public Sprite playImage;

    public Sprite scoreBackground;
    public Sprite scoreImage;

    public Sprite timerBackground;

    public Sprite timesUpBackground;

    public Sprite resultBackground;

    public Sprite headerImage;

    public Sprite confirmImage;

    public Sprite resultScoreImage;

    public Sprite buttonClose;

    [Header("Hammer")]
    public Sprite hammerBonk;
    public Sprite hammerUpright;

    [Header("Chest")]
    public Sprite chestClose;
    public Sprite chectOpen;

    [Header("Mole")]
    public Sprite moleDefault;
    public Sprite moleBonked;

    [Header("Hole")]
    public Sprite holeBackground;
    public Sprite holeForeground;
    public Sprite holeFull;

    [Header("Special Foreground")]
    public Sprite leftForeground;
    public Sprite rightForeground;
    public Sprite topForeground;
}

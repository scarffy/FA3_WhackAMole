using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISeason : MonoBehaviour
{
    [SerializeField] private FestiveSeason _festiveSeason;

    [SerializeField] private Image _playImage;

    [SerializeField] private Image _scoreBackground;
    [SerializeField] private Image _scoreImage;

    [SerializeField] private Image _timerBackground;
    [SerializeField] private Image _timesUpBackground;

    [SerializeField] private Image _resultBackground;

    [SerializeField] private Image _headerImage;

    [SerializeField] private Image _confirmImage;

    [SerializeField] private Image _resultScoreImage;

    [Header("Hammer")]
    [SerializeField] private Image _hammerBonk;
    [SerializeField] private Image _hammerUpright;

    [Header("Chest")]
    [SerializeField] private Image _chestClose;
    [SerializeField] private Image _chestOpen;

    [Header("Mole Sprite Renderers")]
    [SerializeField] private Mole[] _moles;
    [SerializeField] private SpriteRenderer[] _moleHoleBackground;
    [SerializeField] private SpriteRenderer[] _moleHoleForeground;
    [SerializeField] private SpriteRenderer[] _moleHoleFull;

    [Header("Special Foreground")]
    public Image leftForeground;
    public Image rightForeground;
    public Image topForeground;

    // Start is called before the first frame update
    void Start()
    {
        //!TODO: add different festive seasons

        if (_festiveSeason.playImage != null) _playImage.sprite = _festiveSeason.playImage;

        if (_festiveSeason.scoreBackground != null) _scoreBackground.sprite = _festiveSeason.scoreBackground;
        if (_festiveSeason.scoreImage != null) _scoreImage.sprite = _festiveSeason.scoreImage;

        if (_festiveSeason.timerBackground != null) _timerBackground.sprite = _festiveSeason.timerBackground;
        if (_festiveSeason.timesUpBackground != null) _timesUpBackground.sprite = _festiveSeason.timesUpBackground;

        if (_festiveSeason.resultBackground != null) _resultBackground.sprite = _festiveSeason.resultBackground;

        if (_festiveSeason.headerImage != null) _headerImage.sprite = _festiveSeason.headerImage;

        if (_festiveSeason.confirmImage != null) _confirmImage.sprite = _festiveSeason.confirmImage;

        if (_festiveSeason.resultScoreImage != null) _resultScoreImage.sprite = _festiveSeason.scoreImage;

        if (_festiveSeason.hammerBonk != null) _hammerBonk.sprite = _festiveSeason.hammerBonk;
        if (_festiveSeason.hammerUpright != null) _hammerUpright.sprite = _festiveSeason.hammerUpright;

        //! I can't find the treasure chest image.
        // if (_festiveSeason.chestClose != null) _chestClose.sprite = _festiveSeason.chestClose;
        // if (_festiveSeason.chectOpen != null) _chestOpen.sprite = _festiveSeason.chectOpen;

        if(_festiveSeason.moleDefault != null)
        {
            foreach (var mole in _moles)
            {
                mole.ReplaceMoleSprite(
                    _festiveSeason.moleDefault,
                    _festiveSeason.moleBonked,
                    _festiveSeason.chestClose
                    );
            }
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private GameManager gameManager;
    private bool running;

    //Mole sprite
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite moleHit;
    [SerializeField] private Sprite moleTreasure;

    //Mole position
    private Vector2 startPosition = new Vector2(0f, -0.35f);
    private Vector2 endPosition = new Vector2(0f, 0.25f);
    //How long it takes to show a mole
    private float showDuration = 0.5f;
    private float duration = 1f;

    //Mole param
    private bool hittable = true;
    public enum MoleType { Normal, Treasure };
    [HideInInspector] public MoleParam moleParam;
    private MoleType moleType; //Mole type
    private float specialSpawnRate; //Use when there's special mole
    private int lives; //Mole hp
    private int moleIndex;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Vector2 boxOffset;
    private Vector2 boxSize;
    private Vector2 boxOffsetHidden;
    private Vector2 boxSizeHidden;

    private void Awake()
    {
        hittable = false;
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        boxOffset = boxCollider2D.offset;
        boxSize = boxCollider2D.size;
        boxOffsetHidden = new Vector2(boxOffset.x, -startPosition.y / 2f);
        boxSizeHidden = new Vector2(boxSize.x, 0f);
    }

    public void Activate(float level)
    {
        SetLevel(0);
        CreateNext();
        StartCoroutine(ShowHide(startPosition, endPosition));
    }

    public void Activate(float level, bool isTreasure)
    {
        if (isTreasure)
        {
            SetLevel(0);
            CreateNext(isTreasure);
            StartCoroutine(ShowHide(startPosition, endPosition));
        }
    }

    private void OnMouseDown()
    {
        if (hittable)
        {
            switch (moleType) //Use when there's other mole type
            {
                case MoleType.Normal:
                    spriteRenderer.sprite = moleHit;
                    gameManager.AddScore(moleIndex, moleParam);
                    StopAllCoroutines();
                    StartCoroutine(QuickHide());
                    hittable = false;
                    break;

                case MoleType.Treasure:
                    spriteRenderer.sprite = moleHit;//change to open treasure sprite if available
                    gameManager.hitTreasure = true;
                    gameManager.AddScore(moleIndex, moleParam);
                    gameManager.MoleReset();
                    StopAllCoroutines();
                    StartCoroutine(QuickHide());
                    hittable = false;
                    break;
            }
            gameManager.SetMole();
        }
    }

    private IEnumerator ShowHide(Vector2 start, Vector2 end)
    {
        transform.localPosition = start;

        float elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffsetHidden, boxOffset, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSizeHidden, boxSize, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
        boxCollider2D.offset = boxOffset;
        boxCollider2D.size = boxSize;

        yield return new WaitForSeconds(duration);

        elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffset, boxOffsetHidden, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSize, boxSizeHidden, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = start;
        gameManager.Missed(moleIndex, true);
    }

    private IEnumerator QuickHide()
    {
        yield return new WaitForSeconds(0.25f);

        if (!hittable)
        {
            Hide();
        }
    }

    public void Hide()
    {
        transform.localPosition = startPosition;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;
    }

    private void CreateNext()
    {
        StartCoroutine(CreateNextCo());
    }

    private void CreateNext(bool isTreasure)
    {
        StartCoroutine(CreateNextCo(isTreasure));
    }

    private IEnumerator CreateNextCo()
    {
        gameManager.SetMole();

        yield return new WaitUntil(() => !gameManager.running);

        if (gameManager.currMoleParam.ID == "Treasure")
        {
            moleType = MoleType.Treasure;
            spriteRenderer.sprite = moleTreasure;
            lives = 1;
            SetParam();
        }
        else
        {
            moleType = MoleType.Normal;
            spriteRenderer.sprite = mole;
            lives = 1;
            SetParam();
        }

        hittable = true;
    }

    private IEnumerator CreateNextCo(bool isTreasure)
    {
        if (isTreasure)
        {
            yield return null;
        }

        if (gameManager.currMoleParam.ID == "Treasure")
        {
            moleType = MoleType.Treasure;
            spriteRenderer.sprite = moleTreasure;
            lives = 1;
            //SetParam();
        }
        else
        {
            moleType = MoleType.Normal;
            spriteRenderer.sprite = mole;
            lives = 1;
            SetParam();
        }
        hittable = true;
    }

    private void SetLevel(int level)
    {
        float durationMin = Mathf.Clamp(1 - level * 0.1f, 0.01f, 1f);
        float durationMax = Mathf.Clamp(2 - level * 0.1f, 0.01f, 2f);
        duration = UnityEngine.Random.Range(durationMin, durationMax);
    }

    public void SetIndex(int index)
    {
        moleIndex = index;
    }

    public void SetParam()
    {
        moleParam = gameManager.currMoleParam;
    }

    public void StopGame()
    {
        hittable = false;
        StopAllCoroutines();
    }

    public void SetMoleSprite(Sprite sprite)
    {
        mole = sprite;
    }

    public void SetMoleHitSprite(Sprite sprite)
    {
        moleHit = sprite;
    }
}

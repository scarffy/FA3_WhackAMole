using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private GameManager gameManager;

    public Camera mainCamera;

    public bool isClicked;

    [Header("Hammer Sprite")]
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject hammerBonk;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.playing)
        {
            if(Input.GetMouseButtonDown(0))
            {
                HammerBonk();
                HammerOnPointer();
            }
            else
            {
                if (!isClicked)
                {
                    HammerIdle();
                }
                HammerOnPointer();
            }
        }
        else
        {
            hammer.SetActive(false);
            hammerBonk.SetActive(false);
        }
    }

    public void HammerIdle()
    {
        hammer.SetActive(true);
        hammerBonk.SetActive(false);
    }

    public void HammerBonk()
    {
        hammer.SetActive(false);
        hammerBonk.SetActive(true);
        StartCoroutine(HammerIntervalCo());
    }

    public IEnumerator HammerIntervalCo()
    {
        isClicked = true;
        yield return new WaitForSeconds(0.25f);
        isClicked = false;
    }

    public void HammerOnPointer()
    {
        Vector3 newPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0f;

        transform.position = newPos;
    }
}

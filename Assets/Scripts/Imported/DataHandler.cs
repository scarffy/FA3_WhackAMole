using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public UserData userData;
}

[System.Serializable]
public class UserModel
{
    public float points;
    public float chances;

    public MoleParam[] moleParam;
}

[System.Serializable]
public class MoleParam
{
    public string ID;
    public float chance;
    public float score;
    public int index;

    [HideInInspector] public double weight;
}

[System.Serializable]
public class GameModel
{
    public string userName;
    public int score;

    public enum RewardType {Credit, Divide, Multiply}
    public RewardType rewardType;
}


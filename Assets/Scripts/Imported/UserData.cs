using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UserData : ScriptableObject
{
    [Header("User Data")]
    public UserModel userModel;
    [Header("Game Data")]
    public GameModel gameModel;
}

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameData : ScriptableObject
{
    [JsonIgnore]
    public bool isDesktop;

    [JsonIgnore]
    public string os;
}

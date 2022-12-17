using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Utilities;
using System.Runtime.InteropServices;

public class Utilities : MonoBehaviour
{
    //[DllImport("__Internal")]
    //private static extern void HideLogs();

    //[DllImport("__Internal")]
    //private static extern void ResumeAudio();

    private void Awake()
    {
        //disable logging when outside of editor
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
        //HideLogs();
        //ResumeAudio();
#endif

        //AOTEnforcer();
    }

    //For AOT error
    private void AOTEnforcer()
    {
        //AotHelper.EnsureList<Prize>();
    }

    //[System.Obsolete]
    //private void HideLogs()
    //{
    //    Application.ExternalEval("console.log = function(){}");
    //    Application.ExternalEval("console.warn = function(){}");
    //}

}

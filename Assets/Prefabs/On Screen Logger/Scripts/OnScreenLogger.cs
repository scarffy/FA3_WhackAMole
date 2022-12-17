using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnScreenLogger : Singleton<OnScreenLogger>
{
    public GameObject loggerRowPrefab;
    public Transform logContainer;
    public Color defaultTextColor;
    
    private int logCount = 0;

    public static void Log(string loggerRowName, string message, Color color, bool append = false)
    {
        if (Instance == null)
        {
            return;
        }

        if (Instance.logCount > 20)
        {
            Instance.Clear();
        }

        Transform loggerRow = Instance.logContainer.Find(loggerRowName);
        Text loggerText;
        string appendText = "<color=" + ColorTypeConverter.ToRGBHex(color) + ">" + message + "</color>";

        if (loggerRow != null)
        {
            loggerText = loggerRow.GetComponent<Text>();
            
            loggerText.text = append ? loggerText.text += "\n" + appendText : appendText;

            if (append)
            {
                loggerText.rectTransform.sizeDelta = new Vector2(loggerText.rectTransform.sizeDelta.x, loggerText.rectTransform.sizeDelta.y + 70);

                Instance.logCount++;
            }
        }
        else
        {
            // If specified logger row not found, create a new logger row with that logger row name
            loggerRow = Instance.CreateLoggerRow(loggerRowName).transform;
            loggerText = loggerRow.GetComponent<Text>();
            loggerText.text = appendText;

            Instance.logCount++;
        }
    }

    public static void ClearLog(string logName)
    {
        if (Instance == null)
        {
            return;
        }

        Transform row = Instance.logContainer.Find(logName);
        if (row != null)
        {
            DestroyImmediate(row.gameObject);
        }
    }

    void Clear()
    {
        for(int i = 0; i < logContainer.childCount; i++)
        {
            Destroy(logContainer.GetChild(i).gameObject);
        }

        logCount = 0;
    }

    public static void Log(string loggerRowName, string message, bool append = false)
    {
        if (Instance == null)
        {
            return;
        }

        Log(loggerRowName, message, Instance.defaultTextColor, append);
    }

    GameObject CreateLoggerRow(string loggerRowName)
    {
        GameObject loggerRow = Instantiate(Instance.loggerRowPrefab, Instance.logContainer);
        loggerRow.gameObject.name = loggerRowName;

        return loggerRow;
    }

    public static void Log(string message, Color color, bool append = false)
    {
        Log("default", message, color, append);
    }

    #region Log append

    public static void LogAppend(string message)
    {
        Log("default", message, true);
    }

    public static void LogAppend(string message, Color32 color)
    {
        Log("default", message, color, true);
    }

    public static void LogAppend(string loggerRowName, string message)
    {
        Log(loggerRowName, message, true);
    }

    public static void LogAppend(string loggerRowName, string message, Color color)
    {
        Log(loggerRowName, message, color, true);
    }

    #endregion

    #region Log overwrite

    public static void LogOverwrite(string message)
    {
        Log("default", message, false);
    }

    public static void LogOverwrite(string message, Color color)
    {
        Log("default", message, color);
    }

    public static void LogOverwrite(string loggerRowName, string message)
    {
        Log(loggerRowName, message, false);
    }

    #endregion
}

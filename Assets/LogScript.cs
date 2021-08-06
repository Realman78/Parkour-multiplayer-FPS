using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LogScript : NetworkBehaviour
{
    static Text logText;
    void Start()
    {
        logText = GetComponent<Text>();
    }

    public static void AddToLog(string text) {
        string[] lines = logText.text.Split('\n');
        if(lines.Length > 4) {
            string newText = "";
            for (int i = 1; i < lines.Length - 1; i++)
                newText += lines[i] + "\n";
            logText.text = newText + text;
        } else {
            logText.text += text;
        }

        
    }
}

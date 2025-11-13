using UnityEngine;

public class CustomLogger : MonoBehaviour
{
    string Red = "<b><color=red>";
    string Orange = "<b><color=orange>";
    string Yellow = "<b><color=yellow>";
    string Green = "<b><color=green>";
    string Blue = "<b><color=blue>";
    string Purple = "<b><color=purple>";
    string Pink = "<b><color=pink>";
    string White = "<b><color=white>";
    string Grey = "<b><color=grey>";
    string Black = "<b><color=black>";
    string CloseColor = "</color></b>";
    
    string Bold =  "<b>";
    string CloseBold = "</b>";
    string Italic = "<i>";
    string CloseItalic = "</i>";
    
    public static void LogReminder(string entry)
    {
        Debug.Log($"<color=green><b>Reminder: </b></color> {entry}");
    }
}

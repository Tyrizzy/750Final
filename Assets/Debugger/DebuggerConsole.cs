using TMPro;
using UnityEngine;

public class DebuggerConsole : MonoBehaviour
{
    public static DebuggerConsole instance;

    [SerializeField] RectTransform DisplayRect;
    [SerializeField] TMP_Text DisplayText;

    float initHeight;

    private void Awake()
    {
        if (DebuggerConsole.instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DebuggerConsole.instance = this;
        }
    }

    public void DisplayPos(float NewPos)
    {
        DisplayRect.anchoredPosition = new Vector2(DisplayRect.anchoredPosition.x, initHeight + NewPos);
    }

    public void Log(string message, Color TextColor)
    {
        DisplayText.color = TextColor;
        DisplayText.text = message + "\n" + DisplayText.text;
    }

    public void ClearConsole()
    {
        DisplayText.text = "";
    }
}

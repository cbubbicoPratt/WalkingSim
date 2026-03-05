using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("Dialogue")]
    [TextArea(3, 10)]
    public string[] lines;

    [Header("If there are no choices, we show buttons after line ends")]
    public DialogueChoice[] choices;

    [Header("If no choices, auto-continue to this next node")]
    public DialogueData nextNode;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueData nextNode;
}

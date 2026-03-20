using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManage : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI lineText;
    public Transform choicesContainer; //parent object where choice buttons will spawn
    public Button choiceButtonPrefab;//prefab for single choice button

    [Header("Dialogue")]
    private DialogueData currentNode; //current node we are reading from ScriptableObject (SO)
    private int lineIndex; //which line index we currently on, keeping track of dialogue
    private bool isActive; //are we currently in dialogue?

    public string master = "Welcome!";

    

    private void Start()
    {
        lineText.text = master;
    }

    private void OnEnable()
    {
        CCPlayer.OnDialogueRequested += StartDialogue;
    }

    private void OnDisable()
    {
        CCPlayer.OnDialogueRequested -= StartDialogue;
    }

    private void Update()
    {
        if (!isActive) return; //if no dialogue is active ignore

        if(Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && Cursor.lockState == CursorLockMode.None)
        {
            if (ChoicesAreShowing()) return; //block only when buttons exist
            Advance();
        }
    }

    void StartDialogue(DialogueData dialogueData)
    {
        if (dialogueData == null)
        {
            Debug.Log("Missing NPC Data");
            return;
        }

        //set state
        currentNode = dialogueData;
        lineIndex = 0;
        isActive = true;

        ShowLine();

        //master += dialogueData.dialogue;
        
        //if (lineText != null) lineText.text = master;
        //Debug.Log($"Dialogue: {dialogueData.dialogue}");
    }

    void ShowLine()
    {
        //when showing a line we shouldn't be showing choices
        ClearChoices();
        //if no node, end dialogue
        if(currentNode == null)
        {
            EndDialogue();
            return;
        }

        //if node has no lines, treat it as finished
        if(currentNode.lines == null || currentNode.lines.Length == 0)
        {
            FinishNode();
            return;
        }

        //clamp index so we never go out of bounds
        lineIndex = Mathf.Clamp(lineIndex, 0, currentNode.lines.Length - 1);
        //show line text
        master = currentNode.lines[lineIndex] + master;
        if(lineText != null) lineText.text = master;
    }

    void Choose(DialogueData nextNode)
    {
        //first remove buttons so UI feels responsive
        ClearChoices();

        //if no next node this choice ends convo
        if(nextNode == null)
        {
            EndDialogue();
            return;
        }

        //otherwise go to the chosen node
        currentNode = nextNode;
        lineIndex = 0;
        ShowLine();
    }

    bool HasChoices(DialogueData node) //check the data
    {
        //does this dialogue node contain choice data?
        return node != null && node.choices != null && node.choices.Length > 0;
    }

    void Advance()
    {
        //if node finished, end dialogue
        if(currentNode  == null)
        {
            EndDialogue();
            return;
        }

        //move to next line
        lineIndex++;

        //if we still have lines to read in this node show the next one
        if(currentNode != null && lineIndex < currentNode.lines.Length)
        {
            //if we have something
            if(lineText != null)
            {
                //takes the text of our TMP obj and changes it to whatever the current line is (dependent on lineIndex)
                master = currentNode.lines[lineIndex] + master;
                lineText.text = master;
                return;
            }
        }
        //otherwise we have reached the end
        FinishNode();
    }

    void ShowChoices(DialogueChoice[] choices)
    {
        ClearChoices();
        if(choicesContainer == null || choiceButtonPrefab == null)
        {
            Debug.Log("Choices aren't wired!");
            return;
        }

        foreach (DialogueChoice choice in choices)
        {
            //spawn the button as a child of the container
            Button bttn = Instantiate(choiceButtonPrefab, choicesContainer);

            //set visible button text
            TextMeshProUGUI tmp = bttn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = choice.choiceText;

            //cache next node in a local variable
            DialogueData next = choice.nextNode;

            //lambda
            //this is like onclick on our buttons
            //we are saying add a listener when the button is clicked run this function
            bttn.onClick.AddListener(() =>
            {
                Choose(next);
            });
        }
    }

    void FinishNode()
    {
        //1. if choice exists, show choices
        //2. else if next node exists continue automatically
        //3. else end dialogue

        //1.
        if (HasChoices(currentNode))
        {
            ShowChoices(currentNode.choices);
            return;

        }

        //2.
        if(currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            lineIndex = 0;
            ShowLine();
            return;
        }

        //3.
        EndDialogue();
    }

    //are the choices displaying in the UI? do we see them on screen?
    bool ChoicesAreShowing()
    {
        return choicesContainer != null && choicesContainer.childCount > 0;

        //bool showing = choicesContainer != null && choicesContainer.childCount > 0;
        //Debug.Log(showing);
        // return
    }

    void ClearChoices()
    {
        //if we don't have choices container, exit function
        if (choicesContainer == null) return;
        for(int i = choicesContainer.childCount - 1; i >= 0; i--)
        {
            //for every child of the choice container (which is a button) subtract until we clear them all
            Destroy(choicesContainer.GetChild(i).gameObject);
        }
    }

    void EndDialogue()
    {

        isActive = false; //no longer in dialogue
        currentNode = null; //we don't have a node next (SO)
        lineIndex = 0;

        ClearChoices();
    }
}

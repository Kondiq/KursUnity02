using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueComponent : MonoBehaviour
{
    public GameObject player; //player to het stats from

    public string dialogueText; //text above buttons/dialogue options
    public List<DialogueOption> optionsList = new List<DialogueOption>(); //list of choosable dialogue options

    public Canvas canvas; //prefab
    public Button button; //prefab

    Canvas dialogueWindow; //handle for hiding and showing dialogue

    public enum ConditionType
    {
        none,
        strength,
        agility,
        intelligence
    }

    public struct DialogueOption
    {
        public string text; //text on the button
        public ConditionType conditionType; //none,strength,agility,intelligence
        public int conditionValue; //value needed to perform action
        public bool triggerEvent; //if option triggers event, false - nothing happens

        public DialogueOption(string text, ConditionType conditionType, int conditionValue, bool triggerEvent)
        {
            this.text = text;
            this.conditionType = conditionType;
            this.conditionValue = conditionValue;
            this.triggerEvent = triggerEvent;
        }
    }


    // Start is called before the first frame update
    public virtual void Start()
    {
        SetDialogueText("Wybierz jedną z opcji");

        AddDialogueOption(optionsList, "Opcja 1", ConditionType.none, 0, false);
        AddDialogueOption(optionsList, "Opcja 2", ConditionType.none, 0, false);
        AddDialogueOption(optionsList, "Opcja 3", ConditionType.strength, 2, true);
        AddDialogueOption(optionsList, "Opcja 4", ConditionType.agility, 10, true);

        DrawDialogue();
        HideDialogue();
    }

    void DrawDialogue()
    {
        Button dialogueButton=null;

        dialogueWindow = Instantiate(canvas);
        if (dialogueWindow.transform.GetChild(1).transform.name == "InnerCanvas")
        {
            dialogueWindow.transform.GetChild(1).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = dialogueText;
            foreach (DialogueOption opt in optionsList)
            {
                if (dialogueButton != null) //if not first button, set position of new one below previous one
                    dialogueButton = Instantiate(button, new Vector3(dialogueButton.transform.position.x, dialogueButton.transform.position.y - dialogueButton.GetComponent<RectTransform>().rect.height, dialogueButton.transform.position.z), Quaternion.identity, dialogueWindow.transform.GetChild(1));
                else
                    dialogueButton = Instantiate(button, dialogueWindow.transform.GetChild(1), false); //first button
                dialogueButton.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = opt.text + CheckCondition(opt, player, dialogueButton); //text on button
                if (opt.triggerEvent)
                    dialogueButton.onClick.AddListener(delegate { RunEvent(); }); //run event if button is trigger
                else
                    dialogueButton.onClick.AddListener(delegate { HideDialogue(); }); //close window if button without trigger
            }
        }
    }

    void SetDialogueText(string text)
    {
        dialogueText = text;
    }

    void AddDialogueOption(List<DialogueOption> optionsList, string text, ConditionType conditionType, int value, bool isTrigger)
    {
        DialogueOption option = new DialogueOption(text, conditionType, value, isTrigger);
        optionsList.Add(option);
    }

    /*makes button inactive if condition not met, returns text playerstatValue/conditionValue */
    string CheckCondition(DialogueOption opt, GameObject player, Button dButton)
    {
        PlayerSkillsComponent psc = player.GetComponent<PlayerSkillsComponent>();
        switch (opt.conditionType)
        {
            case ConditionType.none:
                return "";
            case ConditionType.strength:
                if (psc.strenght < opt.conditionValue) dButton.interactable = false;
                return " [STR " + psc.strenght + "/" + opt.conditionValue + "]";
            case ConditionType.agility:
                if (psc.agility < opt.conditionValue) dButton.interactable = false;
                return " [AGT " + psc.agility + "/" + opt.conditionValue + "]";
            case ConditionType.intelligence:
                if (psc.intelligence < opt.conditionValue) dButton.interactable = false;
                return " [INT " + psc.intelligence + "/" + opt.conditionValue + "]";
        }
        return "";
    }

    private void OnTriggerEnter(Collider other)
    {
        ShowDialogue();
    }

    void ShowDialogue()
    {
        dialogueWindow.enabled = true;
    }

    void HideDialogue()
    {
        dialogueWindow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void RunEvent()
    {
        GetComponentInParent<ParticleSystem>().Play();
        HideDialogue();
    }
}

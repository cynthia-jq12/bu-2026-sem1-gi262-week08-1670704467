using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcText;
    public Transform choiceContainer;
    public Button choiceButtonPrefab;
    public GameObject closeButtonDialogue;
    private DialogueSequen InteractNpcSequen;

    private List<Button> activeButtons = new List<Button>();

    public void Setup(DialogueSequen sequen)
    {
        if (sequen == null)
        {
            Debug.LogError("DialogueUI: sequen is null!");
            return;
        }

        InteractNpcSequen = sequen;

        if (InteractNpcSequen.tree == null)
        {
            InteractNpcSequen.LoadConversations();
        }

        if (InteractNpcSequen.tree == null || InteractNpcSequen.tree.root == null)
        {
            Debug.LogError("DialogueUI: DialogueTree or root is null!");
            return;
        }

        DialogueNode currentNode = InteractNpcSequen.tree.root;
        ShowDialogue(currentNode);

        // Show UI
        gameObject.SetActive(true);
        dialoguePanel.SetActive(true);
        closeButtonDialogue.SetActive(false);
    }

    public void ShowDialogue(DialogueNode node)
    {
        if (node == null) return;

        if (InteractNpcSequen != null)
        {
            InteractNpcSequen.currentNode = node;
        }

        // แสดงข้อความของ NPC
        if (npcText != null)
        {
            npcText.text = node.text;
        }

        // ล้างปุ่มตัวเลือกเดิม
        ClearChoices();

        // สร้างปุ่มตัวเลือกใหม่จาก nexts
        if (node.nexts != null)
        {
            var choices = new List<string>(node.nexts.Keys);
            for (int i = 0; i < node.nexts.Count; i++)
            {
                string choiceText = choices[i];
                CreateChoiceButton(choiceText, i);
            }
        }
    }

    private void CreateChoiceButton(string text, int index)
    {
        if (choiceButtonPrefab == null || choiceContainer == null) return;

        Button newButton = Instantiate(choiceButtonPrefab, choiceContainer);

        // ตั้งค่าข้อความบนปุ่ม
        var textComp = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null)
        {
            textComp.text = text;
        }

        // เพิ่ม Listener เมื่อกดเลือก
        newButton.onClick.AddListener(() => OnChoiceSelected(index));

        activeButtons.Add(newButton);
    }

    private void ClearChoices()
    {
        foreach (Button button in activeButtons)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }
        activeButtons.Clear();
    }

    private void OnChoiceSelected(int index)
    {
        if (InteractNpcSequen != null)
        {
            InteractNpcSequen.SelectChoice(index);
        }
    }

    public void ShowCloseButtonDialog()
    {
        if (closeButtonDialogue != null)
        {
            closeButtonDialogue.gameObject.SetActive(true);
        }
    }

    public void HideDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        ClearChoices();
    }
}

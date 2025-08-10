using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewDialogue" , menuName = "Dialiogue/DialogueData" )]
public class EndDialogeDataSO : ScriptableObject
{
    [Header("대화내용")]
    [TextArea(3, 10)]
    public List<string> dialogueLines = new List<string>();
}

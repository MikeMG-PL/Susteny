using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public string nameInDialogue;

    [TextArea(minLines: 50, maxLines: 50)]
    public string[] texts;
}

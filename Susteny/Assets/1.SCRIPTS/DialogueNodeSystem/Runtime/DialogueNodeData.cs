using System;
using UnityEngine;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class DialogueNodeData
    {
        public bool QuitNode;
        public bool WasChosen;
        public bool PlayerText;
        public string NodeGUID;
        public string DialogueText;
        public Vector2 Position;
    }
}
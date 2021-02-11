using System;
using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class DialogueNodeData
    {
        public bool QuitNode;
        public bool PlayerText;
        public string NodeGUID;
        public string DialogueText;
        public string AlternateText;
        public Vector2 Position;
    }
}
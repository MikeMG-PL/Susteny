using System;
using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class DialogueNodeData
    {
        public List<bool> WasChosenPorts;
        public List<bool> GrayOutPorts;
        public bool QuitNode;
        public bool PlayerText;
        public string NodeGUID;
        public string DialogueText;
        public Vector2 Position;
    }
}
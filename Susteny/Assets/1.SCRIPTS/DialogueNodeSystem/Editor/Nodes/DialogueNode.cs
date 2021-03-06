﻿#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Subtegral.DialogueSystem.Editor
{
    public class DialogueNode : Node
    {
        public bool QuitNode;
        public bool PlayerText;
        public string DialogueText;
        public string AlternateText;
        public string GUID;
        public bool EntyPoint = false;
    }
}
#endif
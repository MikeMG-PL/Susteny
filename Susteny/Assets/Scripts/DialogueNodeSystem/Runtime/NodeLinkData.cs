﻿using System;
using System.Linq;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class NodeLinkData
    {
        public string BaseNodeGUID;
        public string Sentence;
        public string PortName;
        public string TargetNodeGUID;
    }
}
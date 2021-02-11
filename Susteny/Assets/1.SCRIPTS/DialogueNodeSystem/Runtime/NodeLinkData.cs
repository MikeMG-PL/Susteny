using System;
using System.Linq;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class NodeLinkData
    {
        public bool WasChosen;
        public bool GrayOut;
        public string BaseNodeGUID;
        public string Sentence;
        public string PortName;
        public string TargetNodeGUID;
    }
}
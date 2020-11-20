using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;

public class GraphSaveUtility
{
    DialogueGraphView _targetGraphView;
    DialogueContainer _containerCache;

    List<Edge> Edges => _targetGraphView.edges.ToList();
    List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        if (!Edges.Any()) return;
        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        var connectedPorts = Edges.Where(Edge => Edge.input.node != null).ToArray();

        for(int i = 0; i<connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        foreach(var dialogueNode in Nodes.Where(node=>!node.EntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                Guid = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources/Dialogues"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/Dialogues/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {
        _containerCache = Resources.Load<DialogueContainer>($"Dialogues/{fileName}");
        if(_containerCache == null)
        {
            EditorUtility.DisplayDialog("File not found", "Target dialogue graph file does not exist.", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    void ClearGraph()
    {
        Nodes.Find(DialogueNode => DialogueNode.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;
        foreach (var node in Nodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(Edges => Edges.input.node == node).ToList().ForEach(edge=>_targetGraphView.RemoveElement(edge));

            _targetGraphView.RemoveElement(node);
        }
    }

    void CreateNodes()
    {
        foreach(DialogueNodeData nodeData in _containerCache.DialogueNodeData)
        {
            var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            var nodePorts = _containerCache.NodeLinks.Where(NodeLinkData => NodeLinkData.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(NodeLinkData => _targetGraphView.AddChoicePort(tempNode, NodeLinkData.PortName));
        }
    }

    void ConnectNodes()
    {

    }
}

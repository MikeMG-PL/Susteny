#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Subtegral.DialogueSystem.DataContainers;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Subtegral.DialogueSystem.Editor
{
    public class StoryGraphView : GraphView
    {
        public List<UnityEngine.UIElements.Toggle> grayOutToggles;
        public readonly Vector2 DefaultNodeSize = new Vector2(200, 150);
        public readonly Vector2 DefaultCommentBlockSize = new Vector2(300, 200);
        public DialogueNode EntryPointNode;
        public Blackboard Blackboard = new Blackboard();
        public List<ExposedProperty> ExposedProperties { get; private set; } = new List<ExposedProperty>();
        private NodeSearchWindow _searchWindow;

        public StoryGraphView(StoryGraph editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("NarrativeGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(GetEntryPointNodeInstance());

            AddSearchWindow(editorWindow);
        }


        private void AddSearchWindow(StoryGraph editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }


        public void ClearBlackBoardAndExposedProperties()
        {
            ExposedProperties.Clear();
            Blackboard.Clear();
        }

        public Group CreateCommentBlock(Rect rect, CommentBlockData commentBlockData = null)
        {
            if (commentBlockData == null)
                commentBlockData = new CommentBlockData();
            var group = new Group
            {
                autoUpdateGeometry = true,
                title = commentBlockData.Title
            };
            AddElement(group);
            group.SetPosition(rect);
            return group;
        }

        public void AddPropertyToBlackBoard(ExposedProperty property, bool loadMode = false)
        {
            var localPropertyName = property.PropertyName;
            var localPropertyValue = property.PropertyValue;
            if (!loadMode)
            {
                while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
                    localPropertyName = $"{localPropertyName}(1)";
            }

            var item = ExposedProperty.CreateInstance();
            item.PropertyName = localPropertyName;
            item.PropertyValue = localPropertyValue;
            ExposedProperties.Add(item);

            var container = new VisualElement();
            var field = new BlackboardField { text = localPropertyName, typeText = "string" };
            container.Add(field);

            var propertyValueTextField = new TextField("Value:")
            {
                value = localPropertyValue
            };
            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                var index = ExposedProperties.FindIndex(x => x.PropertyName == item.PropertyName);
                ExposedProperties[index].PropertyValue = evt.newValue;
            });
            var sa = new BlackboardRow(field, propertyValueTextField);
            container.Add(sa);
            Blackboard.Add(container);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var startPortView = startPort;

            ports.ForEach((port) =>
            {
                var portView = port;
                if (startPortView != portView && startPortView.node != portView.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public void CreateNewDialogueNode(string nodeName, Vector2 position, bool quit, bool pText, List<bool> gout)
        {
            AddElement(CreateNode(nodeName, position, quit, pText, gout));
        }

        public DialogueNode CreateNode(string nodeName, Vector2 position, bool quit, bool pText, List<bool> gout)
        {
            var tempDialogueNode = new DialogueNode()
            {
                title = nodeName,
                DialogueText = nodeName,
                GUID = Guid.NewGuid().ToString(),
                QuitNode = quit,
                PlayerText = pText,
                GrayOutPorts = gout
            };
            tempDialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
            var inputPort = GetPortInstance(tempDialogueNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            tempDialogueNode.inputContainer.Add(inputPort);
            tempDialogueNode.RefreshExpandedState();
            tempDialogueNode.RefreshPorts();
            tempDialogueNode.SetPosition(new Rect(position,
                DefaultNodeSize)); //To-Do: implement screen center instantiation positioning

            var quitNode = new UnityEngine.UIElements.Toggle()
            {
                text = "quits dialogue",
                value = tempDialogueNode.QuitNode
            };
            quitNode.RegisterValueChangedCallback((x) => tempDialogueNode.QuitNode = x.newValue);
            quitNode.SetValueWithoutNotify(quitNode.value);
            tempDialogueNode.mainContainer.Add(quitNode);

            var playerText = new UnityEngine.UIElements.Toggle()
            {
                text = "player's text",
                value = tempDialogueNode.PlayerText
            };
            playerText.RegisterValueChangedCallback((x) => tempDialogueNode.PlayerText = x.newValue);
            playerText.SetValueWithoutNotify(playerText.value);
            tempDialogueNode.mainContainer.Add(playerText);

            /*var grayOut = new UnityEngine.UIElements.Toggle()
            {
                text = "gray out these options when chosen again?",
                value = tempDialogueNode.GrayOut
            };
            grayOut.RegisterValueChangedCallback((x) => tempDialogueNode.GrayOut = x.newValue);
            grayOut.SetValueWithoutNotify(grayOut.value);
            tempDialogueNode.mainContainer.Add(grayOut);*/

            var textField = new TextField();
            textField.multiline = true;

            textField.RegisterValueChangedCallback(evt =>
            {
                tempDialogueNode.DialogueText = evt.newValue;
                tempDialogueNode.title = evt.newValue;
            });
            textField.SetValueWithoutNotify(tempDialogueNode.title);
            tempDialogueNode.mainContainer.Add(textField);

            var button = new Button(() => { AddChoicePort(tempDialogueNode, "[Please enter the sentence]"); })
            {
                text = "Add Choice"
            };
            tempDialogueNode.titleButtonContainer.Add(button);
            return tempDialogueNode;
        }


        public void AddChoicePort(DialogueNode nodeCache, string overriddenPortName = "")
        {
            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            var portLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(portLabel);

            var outputPortCount = nodeCache.outputContainer.Query("connector").ToList().Count();
            var outputPortName = string.IsNullOrEmpty(overriddenPortName)
                ? $"Option {outputPortCount + 1}"
                : overriddenPortName;
            //var sentenceToShow = "[Do not modify to use \"Option\" as a text]";
            /*var sentenceToShow = string.IsNullOrEmpty(title) ? "[Do not modify to use \"Option\" as a text]" : title;

            generatedPort.contentContainer.Add(new Label("  "));
            var sentence = new TextField()
            {
                name = string.Empty,
                value = sentenceToShow
            };
            sentence.multiline = true;

            sentence.RegisterValueChangedCallback((x) => generatedPort.name = x.newValue);
            generatedPort.contentContainer.Add(sentence);
            generatedPort.contentContainer.Add(new Label("| Sentence:"));*/

            var textField = new TextField()
            {
                name = string.Empty,
                value = outputPortName
            };
            textField.multiline = true;
            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);

            generatedPort.contentContainer.Add(new Label("  "));
            generatedPort.contentContainer.Add(textField);

            var toggle = GrayOutHandling(nodeCache, generatedPort);

            var deleteButton = new Button(() => RemovePort(nodeCache, generatedPort, toggle))
            {
                text = "Delete choice"
            };
            generatedPort.contentContainer.Add(new Label(" | Option:"));
            generatedPort.contentContainer.Add(deleteButton);
            generatedPort.portName = outputPortName;
            nodeCache.outputContainer.Add(generatedPort);

            nodeCache.RefreshPorts();
            nodeCache.RefreshExpandedState();
        }

        UnityEngine.UIElements.Toggle GrayOutHandling(DialogueNode nodeCache, Port generatedPort)
        {
            // Inicjalizacja tablicy i dodanie elementów - biorąc pod uwagę stworzenie Toggle'a w międzyczasie
            if (nodeCache.GrayOutPorts == null || nodeCache.GrayOutPorts.Count == 0)
            {
                nodeCache.GrayOutPorts = new List<bool>();
                grayOutToggles = new List<UnityEngine.UIElements.Toggle>();
            }
            nodeCache.GrayOutPorts.Add(true);

            var grayOut = new UnityEngine.UIElements.Toggle()
            {
                text = $"debug: {nodeCache.GrayOutPorts.Count}, change back to: \"gray out?\"",
                value = true,
                userData = new IDInfo { ID = nodeCache.GrayOutPorts.Count - 1 }
            };
            grayOutToggles.Add(grayOut);
            //////////////////////////////////////////////

            dynamic userData = grayOut.userData;
            int ID = userData.ID;

            //debugging
            grayOut.text = ID.ToString();

            grayOut.RegisterValueChangedCallback((x) => nodeCache.GrayOutPorts[ID] = x.newValue);
            grayOut.SetValueWithoutNotify(grayOut.value);
            generatedPort.contentContainer.Add(grayOut);

            return grayOut;
        }

        private void RemovePort(Node node, Port socket, UnityEngine.UIElements.Toggle grayOutToggle)
        {
            var dialogueNode = (DialogueNode)node;
            var targetEdge = edges.ToList()
                .Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }

            node.outputContainer.Remove(socket);

            dynamic userData = grayOutToggle.userData;
            int ID = userData.ID;

            // tutaj usuwa z tablicy
            if (dialogueNode.GrayOutPorts.Count > 0)
            {
                dialogueNode.GrayOutPorts.RemoveAt(ID);
                grayOutToggles.RemoveAt(ID);

                for (int i = ID; i < grayOutToggles.Count; i++)
                {
                    dynamic data = grayOutToggles[i].userData;
                    var newID = data.ID - 1;
                    IDInfo idInfo = new IDInfo { ID = newID };
                    grayOutToggles[i].userData = idInfo;

                    // this one is for debugging
                    grayOutToggles[i].text = $"{newID}";
                }
            }
            //////////////////////////////////////////////

            node.RefreshPorts();
            node.RefreshExpandedState();
        }

        private Port GetPortInstance(DialogueNode node, Direction nodeDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }

        private DialogueNode GetEntryPointNodeInstance()
        {
            var nodeCache = new DialogueNode()
            {
                title = "START",
                GUID = Guid.NewGuid().ToString(),
                DialogueText = "ENTRYPOINT",
                EntyPoint = true
            };

            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            generatedPort.portName = "Next";
            nodeCache.outputContainer.Add(generatedPort);

            nodeCache.capabilities &= ~Capabilities.Movable;
            nodeCache.capabilities &= ~Capabilities.Deletable;

            nodeCache.RefreshExpandedState();
            nodeCache.RefreshPorts();
            nodeCache.SetPosition(new Rect(100, 200, 100, 150));
            return nodeCache;
        }
    }

    class IDInfo
    {
        public int ID;
    }
}
#endif
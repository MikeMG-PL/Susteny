using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using System;

public class DialogueGraph : GraphViewEditorWindow
{
    DialogueGraphView _graphView;
    string _filename = "New Narrative";

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent(text: "Dialogue Graph");
    }

    void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    void ConstructGraphView()
    {
        _graphView = new DialogueGraphView
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField(label: "File Name:");
        fileNameTextField.SetValueWithoutNotify(_filename);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterCallback((EventCallback<ChangeEvent<string>>)(evt => _filename = evt.newValue));
        toolbar.Add(fileNameTextField);

        var nodeCreateButton = new Button(clickEvent: () => { _graphView.CreateNode("Dialogue Node"); });
        nodeCreateButton.text = "Create node";

        toolbar.Add(nodeCreateButton);

        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(false)) { text = "Load Data" });

        rootVisualElement.Add(toolbar);
    }

    void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_filename))
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        if (save)
            saveUtility.SaveGraph(_filename);
        else
            saveUtility.LoadGraph(_filename);
    }

    void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }
}

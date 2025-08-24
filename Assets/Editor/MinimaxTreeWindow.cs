using UnityEditor;
using UnityEngine;

public class MinimaxTreeWindow : EditorWindow
{
    private Vector2 scrollPos; // Add this field

    [MenuItem("Window/Minimax Tree Viewer")]
    public static void ShowWindow()
    {
        GetWindow<MinimaxTreeWindow>("Minimax Tree");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Minimax Tree Viewer");

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos); // Begin scroll view

        // Use MinimaxTreeNode as the root
        if (MinimaxTreeNode.LastTreeRoot != null)
        {
            DrawNode(MinimaxTreeNode.LastTreeRoot, 0);
        }
        else
        {
            EditorGUILayout.LabelField("No tree data available.");
        }

        EditorGUILayout.EndScrollView(); // End scroll view
    }

    void DrawNode(MinimaxTreeNode node, int indent)
    {
        EditorGUI.indentLevel = indent;
        EditorGUILayout.LabelField($"{node.Label} | Depth: {node.Depth} | Score: {node.Score}");
        foreach (var child in node.Children)
        {
            DrawNode(child, indent + 1);
        }
    }
}
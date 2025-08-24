using System.Collections.Generic;

public class MinimaxTreeNode
{
    public string Label;
    public int Depth;
    public int Score;
    public List<MinimaxTreeNode> Children = new();
    public MinimaxTreeNode Parent;

    public MinimaxTreeNode(string label, int depth, int score, MinimaxTreeNode parent = null)
    {
        Label = label;
        Depth = depth;
        Score = score;
        Parent = parent;
    }

    public static MinimaxTreeNode LastTreeRoot;
}


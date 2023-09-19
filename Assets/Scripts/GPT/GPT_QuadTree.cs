using System.Collections.Generic;
using UnityEngine;

public class GPT_QuadTree
{
    public Rect bounds;
    public List<GPT_Node> nodes;
    public GPT_QuadTree[] children;
    public int maxNodes;

    public GPT_QuadTree(Rect _bounds, int maxNodesPerQuad)
    {
        bounds = _bounds;
        nodes = new List<GPT_Node>();
        children = null;
        maxNodes = maxNodesPerQuad;
    }

    public void Insert(GPT_Node node)
    {
        if (children != null)
        {
            int index = GetChildIndex(node);
            if (index != -1)
            {
                children[index].Insert(node);
                return;
            }
        }

        nodes.Add(node);
        if (nodes.Count > maxNodes && children == null)
        {
            Split();
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                int index = GetChildIndex(nodes[i]);
                if (index != -1)
                {
                    children[index].Insert(nodes[i]);
                    nodes.RemoveAt(i);
                }
            }
        }
    }

    private int GetChildIndex(GPT_Node node)
    {
        int index = -1;
        float verticalMidpoint = bounds.x + bounds.width * 0.5f;
        float horizontalMidpoint = bounds.y + bounds.height * 0.5f;

        // Check which quadrant the node belongs to
        bool topQuadrant = node.worldPos.z > horizontalMidpoint;
        bool bottomQuadrant = node.worldPos.z < horizontalMidpoint;
        bool leftQuadrant = node.worldPos.x < verticalMidpoint;
        bool rightQuadrant = node.worldPos.x > verticalMidpoint;

        // Determine the index of the child quadrant
        if (topQuadrant)
        {
            if (leftQuadrant)
                index = 1;
            else if (rightQuadrant)
                index = 0;
        }
        else if (bottomQuadrant)
        {
            if (leftQuadrant)
                index = 2;
            else if (rightQuadrant)
                index = 3;
        }

        return index;
    }

    private void Split()
    {
        float subWidth = bounds.width * 0.5f;
        float subHeight = bounds.height * 0.5f;
        float x = bounds.x;
        float y = bounds.y;

        children = new GPT_QuadTree[4];
        children[0] = new GPT_QuadTree(new Rect(x + subWidth, y, subWidth, subHeight), maxNodes);
        children[1] = new GPT_QuadTree(new Rect(x, y, subWidth, subHeight), maxNodes);
        children[2] = new GPT_QuadTree(new Rect(x, y + subHeight, subWidth, subHeight), maxNodes);
        children[3] = new GPT_QuadTree(new Rect(x + subWidth, y + subHeight, subWidth, subHeight), maxNodes);
    }

    public List<GPT_Node> RetrieveNodesInRegion(Rect region)
    {
        List<GPT_Node> result = new List<GPT_Node>();
        if (!bounds.Overlaps(region))
            return result;

        foreach (GPT_Node node in nodes)
        {
            if (region.Contains(node.worldPos))
                result.Add(node);
        }

        if (children != null)
        {
            for (int i = 0; i < 4; i++)
            {
                result.AddRange(children[i].RetrieveNodesInRegion(region));
            }
        }

        return result;
    }
}

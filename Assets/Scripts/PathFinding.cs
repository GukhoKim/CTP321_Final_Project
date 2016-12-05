using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataStructure;
using System.Linq;

public class PathFinding : MonoBehaviour {
    public List<Vector2> Dijkstra2D(Graph<Vector2> graph, Vertex<Vector2> source, Vertex<Vector2> target)
    {
        List<Vertex<Vector2>> vertices = graph.GetVertices();
        int leftVertices = graph.Length();

        List<Vertex<Vector2>> closed = new List<Vertex<Vector2>> { };
        List<Vertex<Vector2>> open = new List<Vertex<Vector2>> { };

        // Initializng step
        for (int i = 0; i < leftVertices; i++)
            vertices[i].Clear();
        source.SourceClear();
        open.Add(source);

        while (!open.Contains(target))
        {
            // Get vertex that has minimum distance
            int index = FindMinIndex(open);
            Vertex<Vector2> u = open[index];
            open.RemoveAt(index);

            // For each vertex who adjacent from vertex u, update distance and prev vertex
            List<Edge<Vector2>> neighbors = u.GetNeighbors();
            int nLength = neighbors.Count;
            for (int i = 0; i < nLength; i++)
            {
                Vertex<Vector2> v = neighbors[i].NextVertex();
                if (v.GetDist() > u.GetDist() + neighbors[i].GetWeight())
                {
                    v.SetDist(u.GetDist() + neighbors[i].GetWeight());
                    v.SetPrev(u);
                    if (!open.Contains(v))
                        open.Add(v);
                }
            }
        }
        // Get Vector2 path from computed graph
        List<Vector2> path = GetPathFromGraph(source, target);
        return path;
    }

    public List<Vector2> A_star2D(Graph<Vector2> graph, Vertex<Vector2> source, Vertex<Vector2> target)
    {
        List<Vertex<Vector2>> vertices = graph.GetVertices();
        int leftVertices = graph.Length();

        List<Vertex<Vector2>> closed = new List<Vertex<Vector2>> { };
        List<Vertex<Vector2>> open = new List<Vertex<Vector2>> { };

        // Initializng step
        for (int i = 0; i < leftVertices; i++)
            vertices[i].Clear();
        source.SourceClear();
        open.Add(source);

        while (!open.Contains(target))
        {
            // Get vertex that has minimum distance
            int index = AstarFindMinIndex(open, target);
            Vertex<Vector2> u = open[index];
            open.RemoveAt(index);

            // For each vertex who adjacent from vertex u, update distance and prev vertex
            List<Edge<Vector2>> neighbors = u.GetNeighbors();
            int nLength = neighbors.Count;
            for (int i = 0; i < nLength; i++)
            {
                Vertex<Vector2> v = neighbors[i].NextVertex();
                if (v.GetDist() > u.GetDist() + neighbors[i].GetWeight())
                {
                    v.SetDist(u.GetDist() + neighbors[i].GetWeight());
                    v.SetPrev(u);
                    if (!open.Contains(v))
                        open.Add(v);
                }
            }
        }
        // Get Vector2 path from computed graph
        List<Vector2> path = GetPathFromGraph(source, target);
        return path;
    }

    public List<Vector2> GetPathFromGraph(Vertex<Vector2> source, Vertex<Vector2> target)
    {
        List<Vector2> path = new List<Vector2> { };

        while (!target.Equals(source))
        {
            path.Add(target.GetData());
            target = target.GetPrev();
        }
        path.Add(target.GetData());

        List<Vector2> reversed = new List<Vector2> { };
        for (int i = path.Count - 1; i >= 0; i--)
            reversed.Add(path[i]);

        return reversed;
    }

    public int FindMinIndex(List<Vertex<Vector2>> list)
    {
        int index = -1;
        float min = float.PositiveInfinity;

        int len = list.Count;
        for (int i = 0; i<len; i++)
        {
            if (min < list[i].GetDist())
            {
                min = list[i].GetDist();
                index = i;
            }
        }
        return index;
    }

    public int AstarFindMinIndex(List<Vertex<Vector2>> list, Vertex<Vector2> target)
    {
        int index = -1;
        float min = float.PositiveInfinity;

        int len = list.Count;
        for (int i = 0; i < len; i++)
        {
            float dis = list[i].GetDist() + huristic2D(list[i], target);
            if (min < dis)
            {
                min = dis;
                index = i;
            }
        }
        return index;
    }

    public float huristic2D(Vertex<Vector2> current, Vertex<Vector2> target)
    {
        Vector2 cVector = current.GetData();
        Vector2 tVector = target.GetData();
        return (cVector - tVector).magnitude;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DataStructure
{
    public class Graph<T>
    {
        private List<Vertex<T>> vertices;
        private int length;

        public Graph()
        {
            vertices = new List<Vertex<T>> { };
            length = 0;
        }

        public List<Vertex<T>> GetVertices()
        {
            return vertices;
        }

        public int Length()
        {
            return length;
        }

        public void AddVertex(T data)
        {
            Vertex<T> newVertex = new Vertex<T>(data);
            vertices.Add(newVertex);
            length++;
        }

        public void AddEdge(Vertex<T> from, Vertex<T> to, float weight)
        {
            Edge<T> newEdge = new Edge<T>(from, to, weight);
            Vertex<T> fromVertex = FindVertex(from.GetData());
            fromVertex.AddNeighbor(newEdge);
        }

        public Vertex<T> FindVertex(T data)
        {
            for (int i = 0; i < length; i++)
            {
                if (vertices[i].GetData().Equals(data))
                    return vertices[i];
            }
            return null;
        }
    }

    public class Vertex<T>
    {
        private T _data;
        private List<Edge<T>> _neighborEdges;

        private bool _visit = false;
        private Vertex<T> _prev = null;
        private float _dist = 0;

        public Vertex(T d)
        {
            _data = d;
            _neighborEdges = new List<Edge<T>> { };
        }

        public T GetData()
        {
            return _data;
        }           
       
        public void AddNeighbor(Edge<T> newEdge)
        {
            _neighborEdges.Add(newEdge);
        }

        public List<Edge<T>> GetNeighbors()
        {
            return _neighborEdges;
        }

        public void SourceClear()
        {
            _visit = false;
            _dist = 0;
            _prev = null;
        }

        public void Clear()
        {
            _visit = false;
            _dist = float.PositiveInfinity;
            _prev = null;
        }

        public void Visit()
        {
            _visit = true;
        }

        public bool IsVisit()
        {
            return _visit;
        }

        public Vertex<T> GetPrev()
        {
            return _prev;
        }

        public void SetPrev(Vertex<T> newPrev)
        {
            _prev = newPrev;
        }

        public float GetDist()
        {
            return _dist;
        }

        public void SetDist(float newDist)
        {
            _dist = newDist;
        }

    }

    public class Edge<T>
    {
        private Vertex<T> _from;
        private Vertex<T> _to;
        private float _weight;

        public Edge(Vertex<T> f, Vertex<T> t, float w)
        {
            _from = f;
            _to = t;
            _weight = w;
        }

        public Vertex<T> NextVertex()
        {
            return _to;
        }

        public float GetWeight()
        {
            return _weight;
        }
    }
}


using UnityEngine;
using TI4.Raw;
using System.Linq;
using System;
using System.Collections.Generic;

namespace TI4
{
    public class LevelGraph : MonoBehaviour
    {

        public static event Action<KeyValuePair<Vector3, Vector3>[]> OnMapVertexClicked;
        public static event Action<int[]> OnPathFound;
        public static event Action OnHardPathSelected;

        public static bool CanInteract = true;

        public static int CurrentVertex = 1;
        public static int ToVertex;

        [SerializeField] private Vertex[] _vertex;
        [SerializeField] private Edge[] _connections;

        private LineRenderer[] pathRender;
        [SerializeField] int pathWidth;
        [SerializeField] Material pathMaterial;
        [SerializeField] Edge[] hardPaths;

        [System.Serializable]
        public class Vertex
        {
            [SerializeField] private int id;
            [SerializeField] private Transform vertex;

            public int GetVertexId()
            {
                return id;
            }

            public Vector3 GetVertexWorldPosition()
            {
                return vertex.localPosition;
            }

            public void AddRender(GameObject target, Vector3 from, Vector3 to, int lineWidth, Material material)
            {
                var render = target.AddComponent<LineRenderer>();
                render.SetPositions(new Vector3[] { from, to });
                render.startWidth = lineWidth;
                render.endWidth = lineWidth;
                render.material = material;
            }
        }

        [System.Serializable]
        public class Edge
        {
            public int FromVertex;
            public int ToVertex;
            public int weight;
        }

        void Start()
        {
            GFG.Setup(rawData: this.ToString());
            // SetupPathRenderer();
        }

        void Update()
        {
            DetectLevelVertex();
        }

        void SetupPathRenderer()
        {
            pathRender = new LineRenderer[_connections.Count()];
            for (int i = 0; i < _connections.Length; i++)
            {
                Vector3 from = _vertex.Where(vert => vert.GetVertexId() == _connections[i].FromVertex).FirstOrDefault().GetVertexWorldPosition();
                Vector3 to = _vertex.Where(vert => vert.GetVertexId() == _connections[i].ToVertex).FirstOrDefault().GetVertexWorldPosition();
                GameObject connection = new GameObject("connection_" + i);
                _vertex.Where(vert => vert.GetVertexId() == _connections[i].FromVertex).FirstOrDefault().AddRender(connection, from, to, pathWidth, pathMaterial);
            }
        }

        void DetectLevelVertex()
        {
            if (Input.GetMouseButtonDown(0) && CanInteract)
            {
                Camera levelCamera = Game.GetLevelCamera();
                Ray ray = levelCamera.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out RaycastHit hit, 1000);
                LevelVertex vertex = hit.transform.gameObject.GetComponent<LevelVertex>();
                if (vertex != null)
                {
                    ToVertex = vertex.GetId();
                    CurrentVertex = Mathf.Max(CurrentVertex, 1);
                    Debug.Log($"{CurrentVertex} - {ToVertex}");
                    var shortest = GetShortestPath(CurrentVertex, ToVertex);

                    if (IsHardPath(CurrentVertex, ToVertex))
                    {
                        OnHardPathSelected?.Invoke();
                    }

                    int[] ids = new int[shortest.Length];

                    for (int i = 0; i < ids.Length; i++)
                    {
                        ids[i] = shortest[i].GetVertexId();
                    }

                    List<KeyValuePair<Vector3, Vector3>> finalPath = new List<KeyValuePair<Vector3, Vector3>>();

                    for (int i = 0; i < shortest.Length; i++)
                    {
                        if (i + 1 < shortest.Length)
                        {
                            finalPath.Add(new KeyValuePair<Vector3, Vector3>(shortest[i].GetVertexWorldPosition() + Vector3.up * 1.2f, shortest[i + 1].GetVertexWorldPosition() + Vector3.up * 1.2f));
                        }
                    }

                    OnMapVertexClicked?.Invoke(finalPath.ToArray());
                    OnPathFound?.Invoke(ids);
                }
            }
        }

        bool IsHardPath(int from, int to)
        {
            return hardPaths.Any(path => path.FromVertex == from && path.ToVertex == to);
        }

        public Vertex[] GetShortestPath(int from, int to)
        {
            var pathList = GFG.GetShortestPath(from, to);
            Vertex[] path = new Vertex[pathList.Count];
            for (int i = 0; i < pathList.Count; i++)
            {
                path[i] = _vertex[pathList[i]];
            }

            Array.Reverse(path);

            return path;
        }

        public override string ToString()
        {
            string final = $"{_vertex.Length}  {_connections.Length}\n";
            foreach (Edge connection in _connections)
            {
                final += $"{connection.FromVertex};{connection.ToVertex}      {connection.weight}\n";
            }

            Debug.Log(final);

            return final;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.normal.textColor = Color.blue;
            labelStyle.fontStyle = FontStyle.Bold;

            for (int i = 0; i < _vertex.Length; i++)
            {
                Vector3 lablePosition = _vertex[i].GetVertexWorldPosition() + Vector3.up * .5f;
                UnityEditor.Handles.Label(lablePosition, $"Vert ({ i + 1 })", labelStyle);
            }

            for (int i = 0; i < _connections.Length; i++)
            {
                Vector3 from = _vertex.Where(vert => vert.GetVertexId() == _connections[i].FromVertex).FirstOrDefault().GetVertexWorldPosition();
                Vector3 to = _vertex.Where(vert => vert.GetVertexId() == _connections[i].ToVertex).FirstOrDefault().GetVertexWorldPosition();

                Vector3 offset = Vector3.up / 2f;
                float centerX = (from.x + to.x) / 2;
                float centery = (from.z + to.z) / 2;
                Vector3 center = new Vector3(centerX, 1, centery);

                UnityEditor.Handles.Label(center, _connections[i].weight.ToString(), labelStyle);

                if (IsHardPath(_vertex.Where(vert => vert.GetVertexId() == _connections[i].FromVertex).FirstOrDefault().GetVertexId(), _vertex.Where(vert => vert.GetVertexId() == _connections[i].ToVertex).FirstOrDefault().GetVertexId()))
                    Gizmos.color = Color.red;

                Gizmos.DrawLine(from + offset, to + offset);

                Gizmos.color = Color.cyan;
            }
        }

        [UnityEditor.CustomEditor(typeof(LevelGraph), editorForChildClasses: true)]
        class LevelGraphCustomInspector : UnityEditor.Editor
        {
            int debug_from;
            int debug_to;

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Space(10);
                GUILayout.Label("Debug");

                if (GUILayout.Button("Refresh path"))
                {
                    LevelGraph current = target as LevelGraph;
                    if (current == null)
                    {
                        return;
                    }

                    current.SetupPathRenderer();
                }

                debug_from = UnityEditor.EditorGUILayout.IntField("from", debug_from);
                debug_to = UnityEditor.EditorGUILayout.IntField("to", debug_to);

                if (GUILayout.Button("Find shortest path"))
                {
                    LevelGraph current = target as LevelGraph;
                    if (current == null)
                    {
                        return;
                    }

                    var result = current.GetShortestPath(debug_from, debug_to);
                    foreach (var data in result)
                    {
                        Debug.Log(data.GetVertexId());
                    }
                }
                GUILayout.EndVertical();
            }
        }
#endif
    }

}
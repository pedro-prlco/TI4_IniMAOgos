using UnityEngine;

using DG.Tweening;

using System.Collections.Generic;

namespace TI4
{
    public class MapActor : CharacterBase
    {

        [SerializeField] float walkSpeed;

        KeyValuePair<Vector3, Vector3>[] currentPath;
        int[] ids;
        
        int currentWalkId = -1;
        bool IsHardPath;

        void Start()
        {
            LevelGraph.OnMapVertexClicked += Walk;
            LevelGraph.OnPathFound += OnIdFound;
            LevelGraph.OnHardPathSelected += OnHardMapSelected;
        }

        void OnDisable()
        {
            LevelGraph.OnMapVertexClicked -= Walk;
            LevelGraph.OnPathFound -= OnIdFound;
            LevelGraph.OnHardPathSelected -= OnHardMapSelected;
        }
        
        void OnIdFound(int[] ids)
        {
            this.ids = ids;
        }

        void OnHardMapSelected()
        {
            IsHardPath = true;
        }

        public override void Walk(KeyValuePair<Vector3, Vector3>[] path)
        {
            if(currentWalkId != -1)
            {
                return;
            }
            
            currentPath = path;
            currentWalkId = 0;

            Game.GetUIController().SetPanel<UIPanel_Map>(UI.PanelType.Map).HidePlayButton();
        }

        void Update()
        {
            if(currentWalkId == -1)
            {
                return;
            }

            SetState(State.Walk);
            KeyValuePair<Vector3, Vector3> path = currentPath[currentWalkId];
            float distance = Vector3.Distance(transform.position, path.Value);

            transform.LookAt(path.Value, Vector3.up);

            if(distance > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path.Value, Time.deltaTime * walkSpeed);
            }
            else
            {
                transform.position = path.Value;
                currentWalkId++;
                LevelGraph.CurrentVertex = ids[currentWalkId];
                Debug.Log("CurrentVertex is " + LevelGraph.CurrentVertex);


                if(currentWalkId >= currentPath.Length)
                {
                    SetState(State.Idle);
                    currentWalkId = -1;

                    if(LevelInfo.current != null && Vector3.Distance(transform.position, LevelInfo.current.transform.position) <= 5f)
                    {
                        Game.GetUIController().SetPanel<UIPanel_Map>(UI.PanelType.Map).DisplayPlayButton();
                    }

                    if(IsHardPath)
                    {
                        Game.GetUIController().SetPanel<UIPanel_Map>(UI.PanelType.Map).DisplayWarning();
                        IsHardPath = false;
                    }
                }
            }
        }
    }
}
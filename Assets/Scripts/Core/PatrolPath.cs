using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointGizmoRadius = 0.4f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Vector3 v = GetWayPointLocation(i);
                Gizmos.DrawSphere(v, waypointGizmoRadius);
                Gizmos.DrawLine(GetWayPointLocation(i), GetWayPointLocation(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            else
            {
                return i + 1;
            }
        }

        public Vector3 GetWayPointLocation(int i)
        {
            return new Vector3(transform.GetChild(i).position.x, transform.GetChild(i).position.y, 0);
        }
    }

}
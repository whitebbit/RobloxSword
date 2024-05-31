using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Detectors.LineOfSightSystem
{
    public abstract class LineOfSight<T> : BaseDetector<T>
    {
        [SerializeField] private MeshFilter meshFilter;
        [Space] public float viewRadius;
        [Range(0, 360)] public float viewAngle;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public List<T> visibleTargets = new();
        private Mesh viewMesh;
        public float meshResolution;


        private readonly Collider[] overlapResults = new Collider[32];
        private int overlapResultsCount;

        private void Awake()
        {
            viewMesh = new Mesh { name = "View Mesh" };
            meshFilter.mesh = viewMesh;
        }

        private void Start()
        {
            StartCoroutine(FindTargetsWithDelay(.2f));
        }

        private void FindVisibleTargets()
        {
            CallOnFound(default);

            if (!ObjectsDetected()) return;

            for (var i = 0; i < overlapResultsCount; i++)
            {
                if (overlapResults[i].TryGetComponent(out T findable) == false) continue;

                var target = overlapResults[i].transform;
                var dirToTarget = (target.position - transform.position).normalized;
                if (!(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)) continue;
                var dstToTarget = Vector3.Distance(transform.position, target.position);
                if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) continue;

                CallOnFound(findable);
            }
        }

        private IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        private void LateUpdate()
        {
            DrawFieldOfView();
        }

        private void DrawFieldOfView()
        {
            var stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
            var stepAngleSize = viewAngle / stepCount;
            var viewPoints = new List<Vector3>();
            float maxDist = -1;
            for (var i = 0; i <= stepCount; i++)
            {
                var angle = -viewAngle / 2 + stepAngleSize * i;
                var newViewCast = ViewCast(angle);
                if (newViewCast.Dst > maxDist)
                {
                    maxDist = newViewCast.Dst;
                }

                viewPoints.Add(newViewCast.Point);
            }

            var vertexCount = viewPoints.Count + 1;
            var vertices = new Vector3[vertexCount];
            var triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;
            for (var i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
                if (i >= vertexCount - 2) continue;
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        private ViewCastInfo ViewCast(float globalAngle)
        {
            var dir = DirFromAngle(globalAngle);
            var t = transform;
            dir = t.rotation * dir;

            return Physics.Raycast(t.position, dir, out var hit, viewRadius, obstacleMask)
                ? new ViewCastInfo(true, hit.point, hit.distance, globalAngle)
                : new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }

        public static Vector3 DirFromAngle(float angleInDegrees)
        {
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        private struct ViewCastInfo
        {
            public bool Hit;
            public readonly Vector3 Point;
            public readonly float Dst;
            public float Angle;

            public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
            {
                Hit = hit;
                Point = point;
                Dst = dst;
                Angle = angle;
            }
        }

        public override bool ObjectsDetected()
        {
            overlapResultsCount =
                Physics.OverlapSphereNonAlloc(transform.position, viewRadius, overlapResults, targetMask);
            return overlapResultsCount > 0;
        }
    }
}
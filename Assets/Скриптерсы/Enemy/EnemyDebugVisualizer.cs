using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Скриптерсы.Enemy
{
#if UNITY_EDITOR
    [ExecuteAlways]
    public class EnemyDebugVisualizer : MonoBehaviour
    {
        [SerializeField] private EnemyBase enemy;
        [SerializeField] private bool showGizmos = true;

        private void OnValidate()
        {
            if (enemy == null)
                enemy = GetComponent<EnemyBase>();

            if (enemy != null)
                enemy.UpdateAnchorsFromContainer();
        }

        private void Update()
        {
            if (!Application.isPlaying && enemy != null && enemy.GetContainer() != null)
            {
                enemy.UpdateAnchorsFromContainer();
            }
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos || enemy == null || enemy.anchors == null || enemy.anchors.Length == 0)
                return;

            DrawPathLines();
            DrawAnchorPoints();
        }

        private void DrawPathLines()
        {
            Gizmos.color = Color.red;

            // Линии между точками
            for (int i = 0; i < enemy.anchors.Length - 1; i++)
            {
                if (enemy.anchors[i] != null && enemy.anchors[i + 1] != null)
                    Gizmos.DrawLine(enemy.anchors[i].position, enemy.anchors[i + 1].position);
            }

            // Замкнутый круг
            if (enemy.LoopPath && enemy.anchors.Length > 2 && 
                enemy.anchors[0] != null && enemy.anchors[^1] != null)
            {
                Gizmos.DrawLine(enemy.anchors[^1].position, enemy.anchors[0].position);
            }
        }

        private void DrawAnchorPoints()
        {
            Handles.color = Color.green;

            for (int i = 0; i < enemy.anchors.Length; i++)
            {
                var point = enemy.anchors[i];
                if (point == null) continue;

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(point.position, 0.1f);

                Handles.Label(point.position + Vector3.up * 0.15f, $"{i + 1}", new GUIStyle
                {
                    normal = new GUIStyleState { textColor = Color.green },
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 50,
                });
            }
        }
    }
#endif
}
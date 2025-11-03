using UnityEngine;

public class InfiniteScroller : MonoBehaviour
{
    public Transform[] segments;  // 2 или 3 сегмента окружения
    public float scrollSpeed = 10f;
    public float segmentLength = 50f;

    void Update()
    {
        foreach (var seg in segments)
        {
            seg.Translate(Vector3.back * scrollSpeed * Time.deltaTime, Space.World);

            // Если сегмент уходит за камеру — перемещаем его вперёд
            if (seg.position.z < -segmentLength)
            {
                float newZ = seg.position.z + segmentLength * segments.Length;
                seg.position = new Vector3(seg.position.x, seg.position.y, newZ);
            }
        }
    }
}

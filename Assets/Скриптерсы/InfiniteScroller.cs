using UnityEngine;

public class InfiniteScroller : MonoBehaviour
{
    [Tooltip("Сегменты окружения, стоящие в ряд по оси X")]
    public Transform[] segments;

    [Tooltip("Скорость движения фона влево")]
    public float scrollSpeed = 10f;

    [Tooltip("Длина одного сегмента по оси X")]
    public float segmentLength = 50f;

    void Update()
    {
        foreach (var seg in segments)
        {
            // Двигаем сегмент влево
            seg.Translate(Vector3.left * scrollSpeed * Time.deltaTime, Space.World);

            // Если сегмент полностью ушёл за левый край — переносим его вперёд
            if (seg.position.x < -segmentLength)
            {
                // Новый X = текущий X + общая длина всех сегментов
                float newX = seg.position.x + segmentLength * segments.Length;
                seg.position = new Vector3(newX, seg.position.y, seg.position.z);
            }
        }
    }
}

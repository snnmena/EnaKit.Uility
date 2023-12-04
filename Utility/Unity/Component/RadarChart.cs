using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Yoziya
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class RadarChart : Graphic
    {
        public float[] values;
        public int numOfPoints;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            if (values == null || values.Length < 3) return;

            float angleStep = 360f / numOfPoints;
            Vector2 center = rectTransform.rect.center;
            UIVertex vert = UIVertex.simpleVert;

            for (int i = 0; i < numOfPoints; i++)
            {
                float angleRad = Mathf.Deg2Rad * (i * angleStep);
                float radius = values[i % values.Length];

                vert.position = new Vector2(center.x + radius * Mathf.Cos(angleRad), center.y + radius * Mathf.Sin(angleRad));
                vert.color = color;
                vh.AddVert(vert);

                int idx1 = i;
                int idx2 = (i + 1) % numOfPoints;
                int idx3 = (i + 2) % numOfPoints;

                vh.AddTriangle(idx1, idx2, idx3);
            }
        }
    }

}
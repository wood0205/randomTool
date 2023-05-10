using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Loop Horizontal Scroll Rect", 50)]
    [DisallowMultipleComponent]
    public class LoopHorizontalScrollRect : LoopScrollRect
    {
        protected override float GetSize(RectTransform item)
        {
            float size = contentSpacing;
            if (m_GridLayout != null)
            {
                size += m_GridLayout.cellSize.x;
            }
            else
            {
                size += LayoutUtility.GetPreferredWidth(item);
            }
            return size;
        }

        protected override float GetDimension(Vector2 vector)
        {
            return vector.x;
        }

        protected override Vector2 GetVector(float value)
        {
            return new Vector2(-value, 0);
        }

        protected override void Awake()
        {
            base.Awake();
            directionSign = 1;

            GridLayoutGroup layout = content.GetComponent<GridLayoutGroup>();
            if (layout != null && layout.constraint != GridLayoutGroup.Constraint.FixedRowCount)
            {
                Debug.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
            }
        }

        protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
        {
            bool changed = false;
            if (viewBounds.max.x > contentBounds.max.x- paddingHorizontal)
            {
                float size = NewItemAtEnd(), totalSize = size;
                while (size > 0 && viewBounds.max.x > contentBounds.max.x + totalSize - paddingHorizontal)
                {
                    size = NewItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            else if (viewBounds.max.x < contentBounds.max.x - threshold - paddingHorizontal)
            {
                float size = DeleteItemAtEnd(), totalSize = size;
                while (size > 0 && viewBounds.max.x < contentBounds.max.x - threshold - totalSize - paddingHorizontal)
                {
                    size = DeleteItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }

            if (viewBounds.min.x < contentBounds.min.x+paddingHorizontal)
            {
                float size = NewItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.min.x < contentBounds.min.x - totalSize + paddingHorizontal)
                {
                    size = NewItemAtStart();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            else if (viewBounds.min.x > contentBounds.min.x + threshold + paddingHorizontal)
            {
                float size = DeleteItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.min.x > contentBounds.min.x + threshold + totalSize + paddingHorizontal)
                {
                    size = DeleteItemAtStart();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            return changed;
        }
    }
}
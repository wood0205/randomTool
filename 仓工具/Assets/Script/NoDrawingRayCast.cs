using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    /// <summary>
    /// 空对象启用射线检测
    /// </summary>
    public class NoDrawingRayCast : Graphic
    {
        public override void SetMaterialDirty()
        {
        }

        public override void SetVerticesDirty()
        {
        }

        [Obsolete]
        protected override void OnFillVBO(List<UIVertex> vbo)
        {
            vbo.Clear();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using NodeInspector;


namespace PaintCraft.Tools.Filters.MaterialFilter{
    /// <summary>
    /// Bind start image (from page config) texture to _TileTex point material
    /// </summary>
    
    [NodeMenuItemAttribute("Material/BindTileTexToStartImage")]
    public class BindTileTexToStartImage : FilterWithNextNode {
        #region implemented abstract members of FilterWithNextNode
        public override bool FilterBody(BrushContext brushLineContext)
        {            
            if (brushLineContext.IsFirstPointInLine && brushLineContext.Canvas.PageConfig.StartImageTexture != null){                
                SetTextureParameters(brushLineContext.BasePoints.First, 
                    brushLineContext.Canvas.PageConfig.StartImageTexture);
                SetTextureParameters(brushLineContext.Points.First, 
                    brushLineContext.Canvas.PageConfig.StartImageTexture);                
            }
            return true;
        }
        #endregion

        void SetTextureParameters(LinkedListNode<Point> nodePoint, Texture2D texture)
        {
            if (nodePoint == null)
            {
                return;
            }
            nodePoint.Value.Material.SetTexture("_TileTex", texture);
            nodePoint.Value.Material.SetTextureOffset("_TileTex", Vector2.zero);
            nodePoint.Value.Material.SetTextureScale ("_TileTex", Vector2.one);
        }        
    }
}
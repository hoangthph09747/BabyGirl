using System.Collections.Generic;
using UnityEngine;
using NodeInspector;

namespace PaintCraft.Tools.Filters.Randomizers
{
    [NodeMenuItem("Randomizers/RandomColor")]
    public class RandomColor : FilterWithNextNode
    {
		private Color[] colors = new Color[2];

		void InitColorsArray(){
//			colors [0] = new Color (255,0,0,255);
//			colors [1] = new Color (251,255,0,255);
//			colors [2] = new Color (0,255,12,255);
//			colors [3] = new Color (62,183,255,255);
//			colors [4] = new Color (255,0,216,255);
//			colors [5] = new Color (139,83,0,255);
//			colors [6] = new Color (0,0,0,255);
			colors [0] = new Color (27,79,64,255);
			colors [1] = new Color (120,147,251,255);
//			colors [9] = new Color (255,62,63,255);
//			colors [10] = new Color (251,19,20,255);
//			colors [11] = new Color (177,19,18,255);
//			colors [12] = new Color (255,245,134,255);
//			colors [13] = new Color (255,238,64,255);
//			colors [14] = new Color (253,211,2,255);
//			colors [15] = new Color (255,159,36,255);
//			colors [16] = new Color (244,102,19,255);
//			colors [17] = new Color (192,255,162,255);
//			colors [18] = new Color (126,254,107,255);
//			colors [19] = new Color (0,207,57,255);
//			colors [20] = new Color (0,165,0,255);
//			colors [21] = new Color (0,106,18,255);
//			colors [22] = new Color (150,254,255,255);
//			colors [23] = new Color (0,241,252,255);
//			colors [24] = new Color (0,200,252,255);
//			colors [25] = new Color (0,154,254,255);
//			colors [26] = new Color (34,83,237,255);
//			colors [27] = new Color (255,200,255,255);
//			colors [28] = new Color (254,143,237,255);
//			colors [29] = new Color (254,77,243,255);
//			colors [30] = new Color (200,43,238,255);
//			colors [31] = new Color (216,39,214,255);
//			colors [32] = new Color (255,226,186,255);
//			colors [33] = new Color (248,196,113,255);
//			colors [34] = new Color (220,132,44,255);
//			colors [35] = new Color (184,100,27,255);
//			colors [36] = new Color (158,73,42,255);
////			colors [37] = new Color (255,255,255,255);
//			colors [37] = new Color (210,210,210,255);
//			colors [38] = new Color (130,130,130,255);
//			colors [39] = new Color (82,82,82,255);
//			colors [40] = new Color (55,55,55,255);
		}

        #region implemented abstract members of FilterWithNextNode

        public override bool FilterBody(BrushContext brushLineContext)
        {
//			InitColorsArray ();
//			int randomIndex = Random.Range (0, 2);
            LinkedListNode<Point> node = brushLineContext.Points.Last;
            while (node != null && node.Value.Status != PointStatus.CopiedToCanvas)
            {
                node.Value.PointColor.Color = new Color(Random.value,Random.value,Random.value, 1.0f);
//				node.Value.PointColor.Color = colors[randomIndex];
                node = node.Previous;
//				Debug.Log ("Index: " + colors[randomIndex] + randomIndex);
//				Debug.Log ("Index: " + node.Value.PointColor.Color);
            }
            return true;
        }

        #endregion


    }
}
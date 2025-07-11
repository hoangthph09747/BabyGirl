using System;
using UnityEngine;
using Observer;
namespace PaintCraft.Tools{
	[Serializable]
	public class LineConfig : MonoBehaviour {

        public static LineConfig instance;

        public Brush      Brush;
        public PointColor Color = PointColor.White;	
        public float      Spacing = 1.0f;
        [Range(0.0f,1.0f)]
		public float	  Scale  = 1.0f;
        public Texture    Texture;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        Action<object> _OnChangeColor, _OnChangePattern, _OnChangeColorGlliter;
        void Start(){
            _OnChangeColor = (param) => OnChangeColor((Color)param);
            _OnChangeColorGlliter = (param) => OnChangeColorGlliter((Color)param);
            _OnChangePattern = (param) => OnChangePattern((int)param);
            this.RegisterListener(EventID.OnChangeColor, _OnChangeColor);
            this.RegisterListener(EventID.OnChangeColorGlliter, _OnChangeColorGlliter);
            this.RegisterListener(EventID.OnChangePattern, _OnChangePattern);
            if (Brush == null){
                Debug.LogError("you must provide default Brush tool here ", gameObject);
            }
        }

        private void OnDestroy()
        {
            this.RemoveListener(EventID.OnChangeColor, _OnChangeColor);
            this.RemoveListener(EventID.OnChangeColorGlliter, _OnChangeColorGlliter);
            this.RemoveListener(EventID.OnChangePattern, _OnChangePattern);
            instance = null;
        }

        private void OnChangeColor(Color param)
        {
            Color.Color = param;
            Texture = null;
        }

        private void OnChangeColorGlliter(Color param)
        {
            Color.Color = param;
            Texture = null;
        }

        private void OnChangePattern(int param)
        {
            Color.Color = UnityEngine.Color.white;
            Texture = Resources.Load<Texture2D>($"PatternBucket/" + param);
        }
    }
}

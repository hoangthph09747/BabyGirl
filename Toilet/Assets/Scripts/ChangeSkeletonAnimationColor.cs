//using QuanUtilities;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HongQuan
{
    public class ChangeSkeletonAnimationColor : MonoBehaviour
    {
        public SkeletonAnimation anim;
        public Color color = Color.white;

        void Start()
        {
            anim = GetComponent<SkeletonAnimation>();
            SetColor(color);
        }

        public void SetColor(Color color)
        {
          //  anim.SetColor(color);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ChangeSkeletonAnimationColor)), CanEditMultipleObjects]
    public class SpineColorChangerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            // Lấy tham chiếu đến đối tượng
            ChangeSkeletonAnimationColor spineColorChanger = (ChangeSkeletonAnimationColor)target;

            // Tạo Color field cho biến animationColor
            Color newColor = EditorGUILayout.ColorField("Animation Color", spineColorChanger.color);

            // Nếu màu đã thay đổi, cập nhật giá trị và gọi phương thức thay đổi màu của Spine
            if (newColor != spineColorChanger.color)
            {
                spineColorChanger.color = newColor;
                spineColorChanger.SetColor(newColor);

                // Đảm bảo rằng các thay đổi được ghi nhận
                EditorUtility.SetDirty(spineColorChanger);
            }

            // Vẽ phần còn lại của Inspector mặc định
        }
    }
#endif
}

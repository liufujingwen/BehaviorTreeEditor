using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class BaseNodeDesigner
    {
        public string Name = string.Empty;//名字
        public Rect Rect;

        //public Rect Rect
        //{
        //    get
        //    {
        //        return new Rect(TitleRect.x, TitleRect.y, ContentRect.width, TitleRect.height + ContentRect.height);
        //    }
        //}

        

       

        public BaseNodeDesigner(string name, Rect rect)
        {
            Name = name;
            Rect = rect;
            rect.height = Math.Max(rect.height, EditorUtility.TitleNodeHeight * 2);

        }


       

        /// <summary>
        /// 指定点是否在控件范围内
        /// </summary>
        /// <param name="point">指定点</param>
        /// <returns>true:在控件范围内，false:不在控件范围内</returns>
        public bool IsContains(Vector2 point)
        {
            return Rect.Contains(point);
        }

        public void AddPoint(Vector2 delta)
        {
            Rect.x += delta.x;
            Rect.y += delta.y;
        }
    }
}

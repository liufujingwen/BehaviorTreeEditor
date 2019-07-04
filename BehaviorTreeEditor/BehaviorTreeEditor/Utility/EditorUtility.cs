using BehaviorTreeEditor.Properties;
using System.Collections.Generic;
using System.Drawing;

namespace BehaviorTreeEditor
{
    public static class EditorUtility
    {
        static EditorUtility()
        {
            NameStringFormat.LineAlignment = StringAlignment.Center;
            NameStringFormat.Alignment = StringAlignment.Center;
            //框选范围用虚线
            SelectionModePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        #region ==================Background===================
        //画布中心点
        public static Vec2 Center = new Vec2(5000f, 5000f);
        //视图缩放最小值
        public static float ZoomScaleMin = 0.5f;
        //视图缩放最大值
        public static float ZoomScaleMax = 2.0f;
        //普通格子线 画笔
        public static Pen LineNormalPen = new Pen(Color.FromArgb(172, 172, 172), 1);
        //粗格子线 画笔
        public static Pen LineBoldPen = new Pen(Color.FromArgb(172, 172, 172), 2);

        #endregion

        #region ==================Transition===================

        //节点普通连线 画笔
        public static Pen TransitionNormalPen = new Pen(Color.Blue, 2);
        //节点普通连线 画笔
        public static Pen TransitionSelectedPen = new Pen(Color.Orange, 2);
        //普通箭头 笔刷
        public static Brush ArrowNormalBrush = new SolidBrush(Color.Blue);
        //普通箭头 笔刷
        public static Brush ArrowSelectedBrush = new SolidBrush(Color.Orange);
        //箭头宽度像素
        public static int ArrowWidth = 17;
        //箭头高度度像素
        public static int ArrowHeight = 10;

        #endregion

        #region  =================节点=====================

        //开始节点标记高度
        public static int StartNodeHeight = 8;
        //开始节点标记 笔刷
        public static Brush StartNodeLogoBrush = new SolidBrush(Color.Green);
        //连接点直径
        public static int NodeLinkPointSize = 12;
        //节点连接点 笔刷
        public static Brush NodeLinkPointBrush = new SolidBrush(Color.FromArgb(255, 54, 74, 85));
        //节点选中 画笔
        public static Pen NodeSelectedPen = new Pen(Color.Orange, 4);
        //框选笔刷
        public static Brush SelectionModeBrush = new SolidBrush(Color.FromArgb(50, Color.Green));
        //框选范围 画笔
        public static Pen SelectionModePen = new Pen(Color.Green, 1.5f);

        //节点字体
        public static Font NodeFont = new Font("宋体", 15, FontStyle.Regular);
        public static Brush NodeBrush = new SolidBrush(Color.White);
        //节点错误 笔刷
        public static Brush NodeErrorBrush = new SolidBrush(Color.Red);

        //标题节点高
        public static int TitleNodeHeight = 30;
        //节点最小宽度
        public static int NodeWidth = 150;
        //节点最小高度
        public static int NodeHeight = 60;
        //普通状态图片
        public static Brush NodeTitleBrush = new SolidBrush(Color.FromArgb(255, 54, 74, 85));
        public static Brush NodeContentBrush = new TextureBrush(Resources.NodeBackground_Light);//普通状态图片
        public static StringFormat NameStringFormat = new StringFormat(StringFormatFlags.NoWrap);

        #endregion

        //节点标题Rect
        public static Rect GetTitleRect(NodeDesigner node, Vec2 offset)
        {
            return new Rect(node.Rect.x - offset.x, node.Rect.y - offset.y, node.Rect.width, EditorUtility.TitleNodeHeight);
        }

        //节点内容Rect
        public static Rect GetContentRect(NodeDesigner node, Vec2 offset)
        {
            return new Rect(node.Rect.x - offset.x, node.Rect.y + EditorUtility.TitleNodeHeight - offset.y, node.Rect.width, node.Rect.height - EditorUtility.TitleNodeHeight);
        }

        //左边连接点
        public static Vec2 GetLeftLinkPoint(NodeDesigner node, Vec2 offset)
        {
            return new Vec2(node.Rect.x - offset.x, node.Rect.y + EditorUtility.TitleNodeHeight / 2.0f - offset.y);
        }

        //右边连接点
        public static Vec2 GetRightLinkPoint(NodeDesigner node, Vec2 offset)
        {
            return new Vec2(node.Rect.x + node.Rect.width - offset.x, node.Rect.y + EditorUtility.TitleNodeHeight / 2.0f - offset.y);
        }

        /// <summary>
        /// 画格子线
        /// </summary>
        /// <param name="graphics">graphics</param>
        /// <param name="rect">画线总区域</param>
        /// <param name="gridSize">间距</param>
        /// <param name="offset">偏移</param>
        public static void DrawGridLines(Graphics graphics, Rect rect, int gridSize, Vec2 offset, bool normal)
        {
            Pen pen = normal ? EditorUtility.LineNormalPen : EditorUtility.LineBoldPen;
            for (float i = rect.x + (offset.x < 0 ? gridSize : 0) + offset.x % gridSize; i < rect.x + rect.width; i = i + gridSize)
            {
                DrawLine(graphics, pen, new Vec2(i, rect.y), new Vec2(i, rect.y + rect.height));
            }
            for (float j = rect.y + (offset.y < 0 ? gridSize : 0) + offset.y % gridSize; j < rect.y + rect.height; j = j + gridSize)
            {
                DrawLine(graphics, pen, new Vec2(rect.x, j), new Vec2(rect.x + rect.width, j));
            }
        }

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="graphics">graphics</param>
        /// <param name="pen">画笔</param>
        /// <param name="p1">起始坐标</param>
        /// <param name="p2">结束坐标</param>
        public static void DrawLine(Graphics graphics, Pen pen, Vec2 p1, Vec2 p2)
        {
            graphics.DrawLine(pen, p1, p2);
        }

        /// <summary>
        /// 绘制节点连接点
        /// </summary>
        public static void DrawNodeLinkPoint(Graphics graphics, NodeDesigner node, Vec2 offset)
        {
            Vec2 leftPoint = EditorUtility.GetLeftLinkPoint(node, offset);
            Vec2 rightPoint = EditorUtility.GetRightLinkPoint(node, offset);

            float halfSize = EditorUtility.NodeLinkPointSize / 2.0f;
            leftPoint.x -= halfSize;
            leftPoint.y -= halfSize;

            rightPoint.x -= halfSize;
            rightPoint.y -= halfSize;

            if (node.StartNode)
            {
                graphics.FillEllipse(EditorUtility.NodeLinkPointBrush, new RectangleF(rightPoint.x, rightPoint.y, EditorUtility.NodeLinkPointSize, EditorUtility.NodeLinkPointSize));
            }
            else if (node.NodeType == NodeType.Composite || node.NodeType == NodeType.Decorator)
            {
                graphics.FillEllipse(EditorUtility.NodeLinkPointBrush, new RectangleF(leftPoint.x, leftPoint.y, EditorUtility.NodeLinkPointSize, EditorUtility.NodeLinkPointSize));
                graphics.FillEllipse(EditorUtility.NodeLinkPointBrush, new RectangleF(rightPoint.x, rightPoint.y, EditorUtility.NodeLinkPointSize, EditorUtility.NodeLinkPointSize));
            }
            else
            {
                graphics.FillEllipse(EditorUtility.NodeLinkPointBrush, new RectangleF(leftPoint.x, leftPoint.y, EditorUtility.NodeLinkPointSize, EditorUtility.NodeLinkPointSize));
            }
        }

        /// <summary>
        /// 绘制开始节点
        /// </summary>
        public static void DrawStartNode(Graphics graphics, NodeDesigner node, Vec2 offset)
        {
            Rect rect = EditorUtility.GetTitleRect(node, offset);

            if (!node.StartNode)
            {
                return;
            }

            graphics.FillRectangle(EditorUtility.StartNodeLogoBrush, new Rectangle((int)rect.x, (int)(rect.y - EditorUtility.StartNodeHeight), (int)rect.width, EditorUtility.StartNodeHeight));
        }

        /// <summary>
        /// 绘制节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="graphics">graphics</param>
        /// <param name="offset">偏移</param>
        /// <param name="on">是否选中</param>
        public static void Draw(NodeDesigner node, Graphics graphics, Vec2 offset, bool on)
        {
            Rect titleRect = GetTitleRect(node, offset);
            Rect contentRect = GetContentRect(node, offset);

            //绘制开始节点
            if (node.StartNode)
                DrawStartNode(graphics, node, offset);
            //画连接点
            DrawNodeLinkPoint(graphics, node, offset);
            //画标题底框
            //graphics.DrawImage(Resources.NodeBackground_Dark, titleRect);
            graphics.FillRectangle(EditorUtility.NodeTitleBrush, titleRect);
            //标题
            graphics.DrawString(node.ClassType, EditorUtility.NodeFont, EditorUtility.NodeBrush, titleRect.x + titleRect.width / 2, titleRect.y + titleRect.height / 2 + 1, EditorUtility.NameStringFormat);
            //画内容底框
            graphics.FillRectangle(EditorUtility.NodeContentBrush, contentRect);

            //graphics.DrawRectangle(EditorUtility.NodeNormalPen, node.Rect - offset);

            //选中边框
            if (on)
            {
                graphics.DrawRectangle(EditorUtility.NodeSelectedPen, node.Rect - offset);
            }

            //处理错误
            bool hasError = false;
            int errorCount = 0;

            if (node.StartNode && node.ParentNode != null)
            {
                hasError = true;
                errorCount++;
                graphics.DrawString("开始节点不能没有父节点", EditorUtility.NodeFont, EditorUtility.NodeErrorBrush, contentRect.x, contentRect.yMax + (errorCount * 20));
            }

            if (!node.StartNode && node.ParentNode == null)
            {
                hasError = true;
                errorCount++;
                graphics.DrawString("没有父节点", EditorUtility.NodeFont, EditorUtility.NodeErrorBrush, contentRect.x, contentRect.yMax + (errorCount * 20));
            }

            if ((node.NodeType == NodeType.Composite || node.NodeType == NodeType.Decorator) && node.Transitions.Count == 0)
            {
                hasError = true;
                errorCount++;
                graphics.DrawString("没有子节点", EditorUtility.NodeFont, EditorUtility.NodeErrorBrush, contentRect.x, contentRect.yMax + (errorCount * 20));
            }

            if (node.NodeType == NodeType.Decorator && node.Transitions.Count > 1)
            {
                hasError = true;
                errorCount++;
                graphics.DrawString("装饰节点只能有一个子节点", EditorUtility.NodeFont, EditorUtility.NodeErrorBrush, contentRect.x, contentRect.yMax + (errorCount * 20));
            }

            if (hasError)
            {
                graphics.DrawImage(Resources.conflict, new PointF(contentRect.center.x - Resources.conflict.Width / 2.0f, contentRect.center.y - Resources.conflict.Height / 2.0f));
            }
        }

        /// <summary>
        /// 获取字段对应的名字
        /// </summary>
        /// <returns></returns>
        public static string GetFieldTypeName(FieldType fieldType)
        {
            string content = string.Empty;
            switch (fieldType)
            {
                case FieldType.IntField:
                    content = "int";
                    break;
                case FieldType.LongField:
                    content = "long";
                    break;
                case FieldType.FloatField:
                    content = "float";
                    break;
                case FieldType.DoubleField:
                    content = "double";
                    break;
                case FieldType.StringField:
                    content = "string";
                    break;
                case FieldType.ColorField:
                    content = "color";
                    break;
                case FieldType.Vector2:
                    content = "vector2";
                    break;
                case FieldType.Vector3:
                    content = "vector3";
                    break;
                case FieldType.EnumField:
                    content = "enum";
                    break;
                case FieldType.BooleanField:
                    content = "bool";
                    break;
                case FieldType.RepeatIntField:
                    content = "int[]";
                    break;
                case FieldType.RepeatLongField:
                    content = "long[]";
                    break;
                case FieldType.RepeatFloatField:
                    content = "float[]";
                    break;
                case FieldType.RepeatVector2Field:
                    content = "vector2[]";
                    break;
                case FieldType.RepeatVector3Field:
                    content = "vector3[]";
                    break;
                case FieldType.RepeatStringField:
                    content = "string[]";
                    break;
            }
            return content;
        }

        /// <summary>
        /// 通过节点模板创建FieldDesigner
        /// </summary>
        /// <param name="nodeField">节点模板</param>
        /// <returns></returns>
        public static FieldDesigner CreateFieldByNodeField(NodeField nodeField)
        {
            FieldDesigner field = new FieldDesigner();
            field.FieldName = nodeField.FieldName;
            field.FieldType = nodeField.FieldType;
            field.Describe = nodeField.Describe;
            switch (nodeField.FieldType)
            {
                case FieldType.IntField:
                    IntFieldDesigner intFieldDesigner = field.Field as IntFieldDesigner;
                    IntDefaultValue intDefaultValue = nodeField.DefaultValue as IntDefaultValue;
                    intFieldDesigner.Value = intDefaultValue.DefaultValue;
                    break;
                case FieldType.LongField:
                    LongFieldDesigner longFieldDesigner = field.Field as LongFieldDesigner;
                    LongDefaultValue longDefaultValue = nodeField.DefaultValue as LongDefaultValue;
                    longFieldDesigner.Value = longDefaultValue.DefaultValue;
                    break;
                case FieldType.FloatField:
                    FloatFieldDesigner floatFieldDesigner = field.Field as FloatFieldDesigner;
                    FloatDefaultValue floatDefaultValue = nodeField.DefaultValue as FloatDefaultValue;
                    floatFieldDesigner.Value = floatDefaultValue.DefaultValue;
                    break;
                case FieldType.DoubleField:
                    DoubleFieldDesigner doubleFieldDesigner = field.Field as DoubleFieldDesigner;
                    DoubleDefaultValue doubleDefaultValue = nodeField.DefaultValue as DoubleDefaultValue;
                    doubleFieldDesigner.Value = doubleDefaultValue.DefaultValue;
                    break;
                case FieldType.StringField:
                    StringFieldDesigner stringFieldDesigner = field.Field as StringFieldDesigner;
                    StringDefaultValue stringDefaultValue = nodeField.DefaultValue as StringDefaultValue;
                    stringFieldDesigner.Value = stringDefaultValue.DefaultValue;
                    break;
                case FieldType.ColorField:
                    ColorFieldDesigner colorFieldDesigner = field.Field as ColorFieldDesigner;
                    ColorDefaultValue colorDefaultValue = nodeField.DefaultValue as ColorDefaultValue;
                    colorFieldDesigner.R = colorDefaultValue.R;
                    colorFieldDesigner.G = colorDefaultValue.G;
                    colorFieldDesigner.B = colorDefaultValue.B;
                    colorFieldDesigner.A = colorDefaultValue.A;
                    break;
                case FieldType.Vector2:
                    Vector2FieldDesigner vector2FieldDesigner = field.Field as Vector2FieldDesigner;
                    Vector2DefaultValue vector2DefaultValue = nodeField.DefaultValue as Vector2DefaultValue;
                    vector2FieldDesigner.X = vector2DefaultValue.X;
                    vector2FieldDesigner.Y = vector2DefaultValue.Y;
                    break;
                case FieldType.Vector3:
                    Vector3FieldDesigner vector3FieldDesigner = field.Field as Vector3FieldDesigner;
                    Vector3DefaultValue vector3DefaultValue = nodeField.DefaultValue as Vector3DefaultValue;
                    vector3FieldDesigner.X = vector3DefaultValue.X;
                    vector3FieldDesigner.Y = vector3DefaultValue.Y;
                    vector3FieldDesigner.Z = vector3DefaultValue.Z;
                    break;
                case FieldType.EnumField:
                    EnumFieldDesigner enumFieldDesigner = field.Field as EnumFieldDesigner;
                    EnumDefaultValue enumDefaultValue = nodeField.DefaultValue as EnumDefaultValue;
                    enumFieldDesigner.EnumType = enumDefaultValue.EnumType;
                    enumFieldDesigner.Value = enumDefaultValue.DefaultValue;
                    break;
                case FieldType.BooleanField:
                    BooleanFieldDesigner booleanFieldDesigner = field.Field as BooleanFieldDesigner;
                    BooleanDefaultValue booleanDefaultValue = nodeField.DefaultValue as BooleanDefaultValue;
                    booleanFieldDesigner.Value = booleanDefaultValue.DefaultValue;
                    break;
                case FieldType.RepeatIntField:
                    break;
                case FieldType.RepeatLongField:
                    break;
                case FieldType.RepeatFloatField:
                    break;
                case FieldType.RepeatVector2Field:
                    break;
                case FieldType.RepeatVector3Field:
                    break;
                case FieldType.RepeatStringField:
                    break;
            }
            return field;
        }

        //复制节点辅助类
        public class CopyNode
        {
            public NodeDesigner Node;
            public List<CopyNode> ChildNode = new List<CopyNode>();

            public static void FreshTransition(CopyNode node)
            {
                if (node.ChildNode.Count > 0)
                {
                    node.Node.Transitions.Clear();
                    for (int i = 0; i < node.ChildNode.Count; i++)
                    {
                        CopyNode child = node.ChildNode[i];
                        Transition transition = new Transition();
                        transition.FromNodeID = node.Node.ID;
                        transition.FromNode = node.Node;
                        transition.ToNodeID = child.Node.ID;
                        transition.ToNode = child.Node;
                        child.Node.ParentNode = node.Node;
                        node.Node.Transitions.Add(transition);
                        FreshTransition(child);
                    }
                }
            }
        }

        //复制节点
        public static CopyNode CopyNodeAndChilds(NodeDesigner node)
        {
            CopyNode copyNode = new CopyNode();
            copyNode.Node = node;

            if (node.Transitions.Count > 0)
            {
                for (int i = 0; i < node.Transitions.Count; i++)
                {
                    Transition transition = node.Transitions[i];
                    CopyNode tempNode = CopyNodeAndChilds(transition.ToNode);
                    copyNode.ChildNode.Add(tempNode);
                }
            }

            return copyNode;
        }

        //辅助Agent添加节点（粘贴添加）
        public static void AddNode(AgentDesigner agent, NodeDesigner node)
        {
            node.ID = agent.GenNodeID();

            if (node.StartNode)
                node.StartNode = agent.ExistStartNode() ? false : true;

            agent.AddNode(node);

            if (node.Transitions.Count > 0)
            {
                for (int i = 0; i < node.Transitions.Count; i++)
                {
                    Transition transition = node.Transitions[i];
                    transition.FromNode = node;
                    transition.FromNodeID = node.ID;
                    NodeDesigner childNode = transition.ToNode;
                    AddNode(agent, childNode);
                    transition.ToNodeID = childNode.ID;
                }
                node.Sort();
            }
        }

        //设置节点偏移
        public static void SetNodePositoin(NodeDesigner node, Vec2 offset)
        {
            node.Rect.x = node.Rect.x + offset.x;
            node.Rect.y = node.Rect.y + offset.y;

            if (node.Transitions.Count > 0)
            {
                for (int i = 0; i < node.Transitions.Count; i++)
                {
                    Transition transition = node.Transitions[i];
                    NodeDesigner childNode = transition.ToNode;
                    SetNodePositoin(childNode, offset);
                }
            }
        }
    }
}

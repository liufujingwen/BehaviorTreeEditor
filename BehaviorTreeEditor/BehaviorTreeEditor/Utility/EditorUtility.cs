using BehaviorTreeEditor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BehaviorTreeEditor
{
    public static class EditorUtility
    {
        public static Dictionary<NodeDesigner, NodeDefine> NodeDefineDic = new Dictionary<NodeDesigner, NodeDefine>();

        static EditorUtility()
        {
            NameStringFormat.LineAlignment = StringAlignment.Center;
            NameStringFormat.Alignment = StringAlignment.Center;
            //框选范围用虚线
            SelectionModePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        #region ==================Background===================
        //画布中心点
        public static Vec2 Center = new Vec2(5000, 5000);
        //视图缩放最小值
        public static float ZoomScaleMin = 0.1f;
        //视图缩放最大值
        public static float ZoomScaleMax = 2.0f;
        //普通格子线 画笔
        public static Pen LineNormalPen = new Pen(Color.FromArgb(255, 30, 30, 30), 1);
        //粗格子线 画笔
        public static Pen LineBoldPen = new Pen(Color.FromArgb(255, 40, 40, 40), 2);

        #endregion

        #region ==================Transition===================

        //节点普通连线 画笔
        public static Pen TransitionNormalPen = new Pen(Color.White, 2);
        //节点普通连线 画笔
        public static Pen TransitionSelectedPen = new Pen(Color.Orange, 3);
        //普通箭头 笔刷
        public static Brush ArrowNormalBrush = new SolidBrush(Color.White);
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
        public static Font NodeTitleFont = new Font("宋体", 12, FontStyle.Regular);
        public static Font NodeContentFont = new Font("宋体", 12, FontStyle.Regular);
        public static Brush NodeTitleFontBrush = new SolidBrush(Color.White);
        public static Brush NodeContentFontBrush = new SolidBrush(Color.Black);
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

        #region Debug

        //调试移动连线
        public static Pen Debug_TransitionPen = new Pen(Color.Orange, 3);
        public static Brush Debug_TransitionBrush = new SolidBrush(Color.Orange);
        //错误连线
        public static Pen Debug_Error_TransitionPen = new Pen(Color.Red, 3);
        public static Brush Debug_Error_TransitionBrush = new SolidBrush(Color.Red);
        //运行连线
        public static Pen Debug_Running_TransitionPen = new Pen(Color.Orange, 3);
        public static Brush Debug_Running_TransitionBrush = new SolidBrush(Color.Orange);
        //失败连线
        public static Pen Debug_Failed_TransitionPen = new Pen(Color.Gray, 3);
        public static Brush Debug_Failed_TransitionBrush = new SolidBrush(Color.Gray);
        //成功连线
        public static Pen Debug_Successed_TransitionPen = new Pen(Color.Yellow, 3);
        public static Brush Debug_Successed_TransitionBrush = new SolidBrush(Color.Yellow);

        //调试节点出错 画笔
        public static Pen Debug_Error_NodePen = new Pen(Color.Red, 4);
        //节点运行 画笔
        public static Pen Debug_Running_NodePen = new Pen(Color.Orange, 4);
        //节点失败 画笔
        public static Pen Debug_Failed_NodePen = new Pen(Color.Gray, 4);
        //节点成功 画笔
        public static Pen Debug_Successed_NodePen = new Pen(Color.Yellow, 4);
        //RunningAlphaSpeed
        public static int RunningAlphaSpeed = 5;

        #endregion

        //节点标题Rect
        public static Rect GetRect(NodeDesigner node)
        {
            return new Rect(node.Rect.x, node.Rect.y, node.Rect.width, node.Rect.height);
        }

        //节点标题Rect
        public static Rect GetTitleRect(NodeDesigner node)
        {
            return new Rect(node.Rect.x, node.Rect.y, node.Rect.width, EditorUtility.TitleNodeHeight);
        }

        //节点内容Rect
        public static Rect GetContentRect(NodeDesigner node)
        {
            return new Rect(node.Rect.x, node.Rect.y + EditorUtility.TitleNodeHeight, node.Rect.width, node.Rect.height - EditorUtility.TitleNodeHeight);
        }

        //左边连接点
        public static Vec2 GetLeftLinkPoint(NodeDesigner node)
        {
            return new Vec2(node.Rect.x, node.Rect.y + EditorUtility.TitleNodeHeight / 2.0f);
        }

        //右边连接点
        public static Vec2 GetRightLinkPoint(NodeDesigner node)
        {
            return new Vec2(node.Rect.x + node.Rect.width, node.Rect.y + EditorUtility.TitleNodeHeight / 2.0f);
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

            offset = -offset;

            for (float i = offset.x - offset.x % gridSize + gridSize; i < offset.x + rect.width; i = i + gridSize)
            {
                DrawLine(graphics, pen, new Vec2(i, rect.y), new Vec2(i, offset.y + rect.height));
            }

            for (float i = offset.y - offset.y % gridSize + gridSize; i < offset.y + rect.height; i = i + gridSize)
            {
                DrawLine(graphics, pen, new Vec2(offset.x, i), new Vec2(offset.x + rect.width, i));
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
        public static void DrawNodeLinkPoint(Graphics graphics, NodeDesigner node)
        {
            Vec2 leftPoint = EditorUtility.GetLeftLinkPoint(node);
            Vec2 rightPoint = EditorUtility.GetRightLinkPoint(node);

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
        public static void DrawStartNode(Graphics graphics, NodeDesigner node)
        {
            Rect rect = EditorUtility.GetTitleRect(node);

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
        public static void Draw(NodeDesigner node, Graphics graphics, bool on)
        {
            Rect titleRect = GetTitleRect(node);
            Rect contentRect = GetContentRect(node);

            //绘制开始节点
            if (node.StartNode)
                DrawStartNode(graphics, node);
            //画连接点
            DrawNodeLinkPoint(graphics, node);
            //画标题底框
            //graphics.DrawImage(Resources.NodeBackground_Dark, titleRect);
            graphics.FillRectangle(NodeTitleBrush, titleRect);
            //标题
            graphics.DrawString(node.Title, NodeTitleFont, NodeTitleFontBrush, titleRect.x + titleRect.width / 2.0f, titleRect.y + titleRect.height / 2.0f, EditorUtility.NameStringFormat);
            //画内容底框
            graphics.FillRectangle(NodeContentBrush, contentRect);

            //渲染内容
            graphics.DrawString(node.ShowContent(), NodeContentFont, NodeContentFontBrush, contentRect.x + contentRect.width / 2.0f, contentRect.y + contentRect.height / 2.0f, EditorUtility.NameStringFormat);

            //选中边框
            if (on && !DebugManager.Instance.Debugging)
            {
                graphics.DrawRectangle(EditorUtility.NodeSelectedPen, node.Rect);
            }

            //处理错误
            bool hasError = false;
            int errorCount = 0;

            if (node.StartNode && node.ParentNode != null)
            {
                hasError = true;
                errorCount++;
                graphics.DrawString("开始节点不能没有父节点", EditorUtility.NodeTitleFont, EditorUtility.NodeErrorBrush, contentRect.x, contentRect.yMax + (errorCount * 20));
            }

            if (!node.StartNode && node.ParentNode == null)
            {
                hasError = true;
                errorCount++;
                graphics.DrawString("没有父节点", EditorUtility.NodeTitleFont, EditorUtility.NodeErrorBrush, contentRect.x, contentRect.yMax + (errorCount * 20));
            }

            if ((node.NodeType == NodeType.Composite || node.NodeType == NodeType.Decorator) && node.Transitions.Count == 0)
            {
                hasError = true;
                errorCount++;
                graphics.DrawString("没有子节点", EditorUtility.NodeTitleFont, EditorUtility.NodeErrorBrush, contentRect.x, contentRect.yMax + (errorCount * 20));
            }

            if (node.NodeType == NodeType.Decorator && node.Transitions.Count > 1)
            {
                hasError = true;
                errorCount++;
                graphics.DrawString("装饰节点只能有一个子节点", EditorUtility.NodeTitleFont, EditorUtility.NodeErrorBrush, contentRect.x, contentRect.yMax + (errorCount * 20));
            }

            if (hasError)
            {
                graphics.DrawImage(Resources.conflict, new PointF(contentRect.center.x - Resources.conflict.Width / 2.0f, contentRect.center.y - Resources.conflict.Height / 2.0f));
            }
        }

        /// <summary>
        /// 绘制节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="graphics">graphics</param>
        /// <param name="offset">偏移</param>
        /// <param name="on">是否选中</param>
        public static void Draw_Debug(DebugNode node, Graphics graphics, float deltatime)
        {
            Rect rect = GetRect(node.Node);

            switch (node.Status)
            {
                case DebugNodeStatus.Transition:
                    if (node.TransitionElapsedTime >= DebugManager.TransitionTime)
                        graphics.DrawRectangle(Debug_Running_NodePen, rect);
                    break;
                case DebugNodeStatus.Error:
                    graphics.DrawRectangle(Debug_Error_NodePen, rect);
                    break;
                case DebugNodeStatus.Failed:
                    graphics.DrawRectangle(Debug_Failed_NodePen, rect);
                    break;
                case DebugNodeStatus.Success:
                    if (node.SuccessAlpha <= 0.9f)
                    {
                        Pen successed_NodePen = Debug_Successed_NodePen.Clone() as Pen;
                        Color successColor = successed_NodePen.Color;
                        node.SuccessAlpha = Mathf.Lerp(node.SuccessAlpha, 1, RunningAlphaSpeed * deltatime);
                        node.SuccessAlpha = Mathf.Clamp01(node.SuccessAlpha);
                        successColor = Color.FromArgb((int)(node.SuccessAlpha * 255), successColor);
                        successed_NodePen.Color = successColor;
                        graphics.DrawRectangle(successed_NodePen, rect);
                    }
                    else
                    {
                        graphics.DrawRectangle(Debug_Successed_NodePen, rect);
                    }
                    break;
                case DebugNodeStatus.Running:

                    if (node.SuccessAlpha <= 0.9f)
                    {
                        Pen runningPen = Debug_Running_NodePen.Clone() as Pen;
                        Color color = runningPen.Color;
                        node.RunningAlpha = Mathf.Lerp(node.RunningAlpha, 1, RunningAlphaSpeed * deltatime);
                        node.RunningAlpha = Mathf.Clamp01(node.RunningAlpha);
                        color = Color.FromArgb((int)(node.RunningAlpha * 255), color);
                        runningPen.Color = color;
                        graphics.DrawRectangle(runningPen, rect);
                    }
                    else
                    {
                        graphics.DrawRectangle(Debug_Running_NodePen, rect);
                    }
                    break;
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
            field.Label = nodeField.Label;
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

        /// <summary>
        /// 获取根节点下所有的节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static void GetNodeAndChilds(NodeDesigner node, List<NodeDesigner> nodes)
        {
            if (node == null)
                return;

            nodes.Add(node);

            if (node.Transitions.Count > 0)
            {
                for (int i = 0; i < node.Transitions.Count; i++)
                {
                    Transition transition = node.Transitions[i];
                    GetNodeAndChilds(transition.ToNode, nodes);

                }
            }
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

        #region serialize bytes 

        public static BehaviorTreeData.TreeData CreateTreeData(TreeData treeData)
        {
            BehaviorTreeData.TreeData data = new BehaviorTreeData.TreeData();

            for (int i = 0; i < treeData.Agents.Count; i++)
            {
                AgentDesigner agent = treeData.Agents[i];
                if (agent == null)
                    continue;
                data.Agents.Add(CreateAgentData(agent));
            }

            return data;
        }

        public static BehaviorTreeData.AgentData CreateAgentData(AgentDesigner agentData)
        {
            BehaviorTreeData.AgentData data = new BehaviorTreeData.AgentData();
            data.ID = agentData.AgentID;

            for (int i = 0; i < agentData.Fields.Count; i++)
            {
                FieldDesigner field = agentData.Fields[i];
                if (field == null)
                    continue;
                data.Fields.Add(CreateField(field));
            }

            NodeDesigner startNode = agentData.GetStartNode();
            if (startNode != null)
                data.StartNode = CreateNode(agentData, startNode);

            return data;
        }

        public static BehaviorTreeData.BaseField CreateField(FieldDesigner fieldData)
        {
            BehaviorTreeData.BaseField data = null;

            switch (fieldData.FieldType)
            {
                case FieldType.IntField:
                    BehaviorTreeData.IntField intField = new BehaviorTreeData.IntField();
                    IntFieldDesigner intFieldDesigner = fieldData.Field as IntFieldDesigner;
                    intField.FieldName = fieldData.FieldName;
                    intField.Value = intFieldDesigner.Value;
                    data = intField;
                    break;
                case FieldType.LongField:
                    BehaviorTreeData.LongField longField = new BehaviorTreeData.LongField();
                    LongFieldDesigner longFieldDesigner = fieldData.Field as LongFieldDesigner;
                    longField.FieldName = fieldData.FieldName;
                    longField.Value = longFieldDesigner.Value;
                    data = longField;
                    break;
                case FieldType.FloatField:
                    BehaviorTreeData.FloatField floatField = new BehaviorTreeData.FloatField();
                    FloatFieldDesigner floatFieldDesigner = fieldData.Field as FloatFieldDesigner;
                    floatField.FieldName = fieldData.FieldName;
                    floatField.Value = floatFieldDesigner.Value;
                    data = floatField;
                    break;
                case FieldType.DoubleField:
                    BehaviorTreeData.DoubleField doubleField = new BehaviorTreeData.DoubleField();
                    DoubleFieldDesigner doubleFieldDesigner = fieldData.Field as DoubleFieldDesigner;
                    doubleField.FieldName = fieldData.FieldName;
                    doubleField.Value = doubleFieldDesigner.Value;
                    data = doubleField;
                    break;
                case FieldType.StringField:
                    BehaviorTreeData.StringField stringField = new BehaviorTreeData.StringField();
                    StringFieldDesigner stringFieldDesigner = fieldData.Field as StringFieldDesigner;
                    stringField.FieldName = fieldData.FieldName;
                    stringField.Value = stringFieldDesigner.Value;
                    data = stringField;
                    break;
                case FieldType.ColorField:
                    BehaviorTreeData.ColorField colorField = new BehaviorTreeData.ColorField();
                    ColorFieldDesigner colorFieldDesigner = fieldData.Field as ColorFieldDesigner;
                    colorField.FieldName = fieldData.FieldName;
                    colorField.Value |= colorFieldDesigner.R << 24;
                    colorField.Value |= colorFieldDesigner.G << 16;
                    colorField.Value |= colorFieldDesigner.B << 8;
                    colorField.Value |= colorFieldDesigner.A;
                    data = colorField;
                    break;
                case FieldType.Vector2:
                    BehaviorTreeData.Vector2Field vector2Field = new BehaviorTreeData.Vector2Field();
                    Vector2FieldDesigner vector2FieldDesigner = fieldData.Field as Vector2FieldDesigner;
                    vector2Field.FieldName = fieldData.FieldName;
                    vector2Field.X = vector2FieldDesigner.X;
                    vector2Field.Y = vector2FieldDesigner.Y;
                    data = vector2Field;
                    break;
                case FieldType.Vector3:
                    BehaviorTreeData.Vector3Field vector3Field = new BehaviorTreeData.Vector3Field();
                    Vector3FieldDesigner vector3FieldDesigner = fieldData.Field as Vector3FieldDesigner;
                    vector3Field.FieldName = fieldData.FieldName;
                    vector3Field.X = vector3FieldDesigner.X;
                    vector3Field.Y = vector3FieldDesigner.Y;
                    vector3Field.Z = vector3FieldDesigner.Z;
                    data = vector3Field;
                    break;
                case FieldType.EnumField:
                    BehaviorTreeData.EnumField enumField = new BehaviorTreeData.EnumField();
                    EnumFieldDesigner enumFieldDesigner = fieldData.Field as EnumFieldDesigner;
                    enumField.FieldName = fieldData.FieldName;
                    enumField.Value = enumFieldDesigner.ValueInt;
                    data = enumField;
                    break;
                case FieldType.BooleanField:
                    BehaviorTreeData.BooleanField booleanField = new BehaviorTreeData.BooleanField();
                    BooleanFieldDesigner booleanFieldDesigner = fieldData.Field as BooleanFieldDesigner;
                    booleanField.FieldName = fieldData.FieldName;
                    booleanField.Value = booleanFieldDesigner.Value;
                    data = booleanField;
                    break;
                case FieldType.RepeatIntField:
                    BehaviorTreeData.RepeatIntField repeatIntField = new BehaviorTreeData.RepeatIntField();
                    RepeatIntFieldDesigner repeatIntFieldDesigner = fieldData.Field as RepeatIntFieldDesigner;
                    repeatIntField.FieldName = fieldData.FieldName;
                    if (repeatIntFieldDesigner.Value.Count > 0)
                    {
                        repeatIntField.Value = new List<int>();
                        for (int i = 0; i < repeatIntFieldDesigner.Value.Count; i++)
                            repeatIntField.Value.Add(repeatIntFieldDesigner.Value[i]);
                    }
                    data = repeatIntField;
                    break;
                case FieldType.RepeatLongField:
                    BehaviorTreeData.RepeatLongField repeatLongField = new BehaviorTreeData.RepeatLongField();
                    RepeatLongFieldDesigner repeatLongFieldDesigner = fieldData.Field as RepeatLongFieldDesigner;
                    repeatLongField.FieldName = fieldData.FieldName;
                    if (repeatLongFieldDesigner.Value.Count > 0)
                    {
                        repeatLongField.Value = new List<long>();
                        for (int i = 0; i < repeatLongFieldDesigner.Value.Count; i++)
                            repeatLongField.Value.Add(repeatLongFieldDesigner.Value[i]);
                    }

                    data = repeatLongField;
                    break;
                case FieldType.RepeatFloatField:
                    BehaviorTreeData.RepeatFloatField repeatFloatField = new BehaviorTreeData.RepeatFloatField();
                    RepeatFloatFieldDesigner repeatFloatFieldDesigner = fieldData.Field as RepeatFloatFieldDesigner;
                    repeatFloatField.FieldName = fieldData.FieldName;
                    if (repeatFloatFieldDesigner.Value.Count > 0)
                    {
                        repeatFloatField.Value = new List<float>();
                        for (int i = 0; i < repeatFloatFieldDesigner.Value.Count; i++)
                            repeatFloatField.Value.Add(repeatFloatFieldDesigner.Value[i]);
                    }
                    data = repeatFloatField;
                    break;
                case FieldType.RepeatVector2Field:
                    BehaviorTreeData.RepeatVector2Field repeatVector2Field = new BehaviorTreeData.RepeatVector2Field();
                    RepeatVector2FieldDesigner repeatVector2FieldDesigner = fieldData.Field as RepeatVector2FieldDesigner;
                    repeatVector2Field.FieldName = fieldData.FieldName;
                    if (repeatVector2FieldDesigner.Value.Count > 0)
                    {
                        for (int i = 0; i < repeatVector2FieldDesigner.Value.Count; i++)
                        {
                            Vector2 vector2 = repeatVector2FieldDesigner.Value[i];
                            BehaviorTreeData.Vector2 temp = new BehaviorTreeData.Vector2();
                            temp.X = vector2.X;
                            temp.Y = vector2.Y;
                            repeatVector2Field.Value.Add(temp);
                        }
                    }
                    data = repeatVector2Field;
                    break;
                case FieldType.RepeatVector3Field:
                    BehaviorTreeData.RepeatVector3Field repeatVector3Field = new BehaviorTreeData.RepeatVector3Field();
                    RepeatVector3FieldDesigner repeatVector3FieldDesigner = fieldData.Field as RepeatVector3FieldDesigner;
                    repeatVector3Field.FieldName = fieldData.FieldName;
                    for (int i = 0; i < repeatVector3FieldDesigner.Value.Count; i++)
                    {
                        Vector3 vector3 = repeatVector3FieldDesigner.Value[i];
                        BehaviorTreeData.Vector3 temp = new BehaviorTreeData.Vector3();
                        temp.X = vector3.X;
                        temp.Y = vector3.Y;
                        temp.Z = vector3.Z;
                        repeatVector3Field.Value.Add(temp);
                    }
                    data = repeatVector3Field;
                    break;
                case FieldType.RepeatStringField:
                    BehaviorTreeData.RepeatStringField repeatStringField = new BehaviorTreeData.RepeatStringField();
                    RepeatStringFieldDesigner repeatStringFieldDesigner = fieldData.Field as RepeatStringFieldDesigner;
                    repeatStringField.FieldName = fieldData.FieldName;
                    if (repeatStringFieldDesigner.Value.Count > 0)
                    {
                        repeatStringField.Value = new List<string>();
                        for (int i = 0; i < repeatStringFieldDesigner.Value.Count; i++)
                            repeatStringField.Value.Add(repeatStringFieldDesigner.Value[i]);
                    }
                    data = repeatStringField;
                    break;
            }

            return data;
        }

        public static BehaviorTreeData.NodeData CreateNode(AgentDesigner agent, NodeDesigner nodeData)
        {
            BehaviorTreeData.NodeData data = new BehaviorTreeData.NodeData();
            data.ID = nodeData.ID;
            data.ClassType = nodeData.ClassType;
            data.Label = nodeData.Label;
            data.X = (int)nodeData.Rect.x;
            data.Y = (int)nodeData.Rect.y;

            for (int i = 0; i < nodeData.Fields.Count; i++)
            {
                FieldDesigner field = nodeData.Fields[i];
                if (field == null)
                    continue;
                data.Fields.Add(CreateField(field));
            }

            if (nodeData.Transitions.Count > 0)
            {
                data.Childs = new List<BehaviorTreeData.NodeData>(nodeData.Transitions.Count);
                for (int i = 0; i < nodeData.Transitions.Count; i++)
                {
                    Transition transition = nodeData.Transitions[i];
                    NodeDesigner childNode = agent.FindByID(transition.ToNodeID);
                    BehaviorTreeData.NodeData childNodeData = CreateNode(agent, childNode);
                    data.Childs.Add(childNodeData);
                }
            }

            return data;
        }

        #endregion
    }
}

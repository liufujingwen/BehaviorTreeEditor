using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using BehaviorTreeViewer;
using Vector2 = UnityEngine.Vector2;

namespace BT.Editor
{
    public static class BehaviorTreeEditorStyles
    {
        public const float StateWidth = 150f;
        public const float StateHeight = 30f;
        public const float StateMachineWidth = 150f;
        public const float StateMachineHeight = 45f;

        public static GUIStyle canvasBackground;
        public static GUIStyle selectionRect;
        public static GUIStyle elementBackground;
        public static GUIStyle breadcrumbLeft;
        public static GUIStyle breadcrumbMiddle;
        public static GUIStyle wrappedLabel;
        public static GUIStyle wrappedLabelLeft;
        public static GUIStyle variableHeader;
        public static GUIStyle label;
        public static GUIStyle centeredLabel;
        public static GUIStyle inspectorTitle;
        public static GUIStyle inspectorTitleText;
        public static GUIStyle stateLabelGizmo;
        public static GUIStyle instructionLabel;
        public static GUIStyle shortcutLabel;
        public static GUIStyle browserPopup;
        public static GUIStyle preLabel;

        public static Texture2D popupIcon;
        public static Texture2D helpIcon;
        public static Texture2D errorIcon;
        public static Texture2D warnIcon;
        public static Texture2D infoIcon;
        public static Texture2D toolbarPlus;
        public static Texture2D toolbarMinus;
        public static Texture2D iCodeLogo;



        public static Color gridMinorColor;
        public static Color gridMajorColor;

        public static int fsmColor;
        public static int startNodeColor;
        public static int anyStateColor;
        public static int defaultNodeColor;


        static GUIStyle compositeNodeStyle;
        static GUIStyle decoratorNodeStyle;
        static GUIStyle conditionNodeStyle;
        static GUIStyle actionNodeStyle;

        static GUIStyle compositeNodeStyle_selected;
        static GUIStyle decoratorNodeStyle_selected;
        static GUIStyle conditionNodeStyle_selected;
        static GUIStyle actionNodeStyle_selected;





        static BehaviorTreeEditorStyles()
        {
            BehaviorTreeEditorStyles.nodeStyleCache = new Dictionary<string, GUIStyle>();
            BehaviorTreeEditorStyles.gridMinorColor = EditorGUIUtility.isProSkin ? new Color(0f, 0f, 0f, 0.18f) : new Color(0f, 0f, 0f, 0.1f);
            BehaviorTreeEditorStyles.gridMajorColor = EditorGUIUtility.isProSkin ? new Color(0f, 0f, 0f, 0.28f) : new Color(0f, 0f, 0f, 0.15f);

            BehaviorTreeEditorStyles.popupIcon = EditorGUIUtility.FindTexture("_popup");
            BehaviorTreeEditorStyles.helpIcon = EditorGUIUtility.FindTexture("_help");
            BehaviorTreeEditorStyles.errorIcon = EditorGUIUtility.FindTexture("d_console.erroricon.sml");
            BehaviorTreeEditorStyles.warnIcon = EditorGUIUtility.FindTexture("console.warnicon");
            BehaviorTreeEditorStyles.infoIcon = EditorGUIUtility.FindTexture("console.infoicon");
            BehaviorTreeEditorStyles.toolbarPlus = EditorGUIUtility.FindTexture("Toolbar Plus");
            BehaviorTreeEditorStyles.toolbarMinus = EditorGUIUtility.FindTexture("Toolbar Minus");

            BehaviorTreeEditorStyles.canvasBackground = "flow background";
            BehaviorTreeEditorStyles.selectionRect = "SelectionRect";
            BehaviorTreeEditorStyles.elementBackground = new GUIStyle("PopupCurveSwatchBackground")
            {
                padding = new RectOffset()
            };
            BehaviorTreeEditorStyles.breadcrumbLeft = "GUIEditor.BreadcrumbLeft";
            BehaviorTreeEditorStyles.breadcrumbMiddle = "GUIEditor.BreadcrumbMid";
            BehaviorTreeEditorStyles.wrappedLabel = new GUIStyle("label")
            {
                fixedHeight = 0,
                wordWrap = true
            };
            BehaviorTreeEditorStyles.wrappedLabelLeft = new GUIStyle("label")
            {
                fixedHeight = 0,
                wordWrap = true,
                alignment = TextAnchor.UpperLeft
            };
            BehaviorTreeEditorStyles.variableHeader = "flow overlay header lower left";
            BehaviorTreeEditorStyles.label = "label";
            BehaviorTreeEditorStyles.inspectorTitle = "IN Title";
            BehaviorTreeEditorStyles.inspectorTitleText = "IN TitleText";
            BehaviorTreeEditorStyles.iCodeLogo = (Texture2D)Resources.Load("ICodeLogo");
            BehaviorTreeEditorStyles.stateLabelGizmo = new GUIStyle("HelpBox")
            {
                alignment = TextAnchor.UpperCenter,
                fontSize = 21
            };
            BehaviorTreeEditorStyles.centeredLabel = new GUIStyle("Label")
            {
                alignment = TextAnchor.UpperCenter,
            };
            BehaviorTreeEditorStyles.instructionLabel = new GUIStyle("TL Selection H2")
            {
                padding = new RectOffset(3, 3, 3, 3),
                contentOffset = BehaviorTreeEditorStyles.wrappedLabel.contentOffset,
                alignment = TextAnchor.UpperLeft,
                fixedHeight = 0,
                wordWrap = true
            };
            BehaviorTreeEditorStyles.shortcutLabel = new GUIStyle("ObjectPickerLargeStatus")
            {
                padding = new RectOffset(3, 3, 3, 3),
                alignment = TextAnchor.UpperLeft
            };
            BehaviorTreeEditorStyles.browserPopup = new GUIStyle("label")
            {
                contentOffset = new Vector2(0, 2)
            };

            BehaviorTreeEditorStyles.fsmColor = (int)NodeColor.Blue;
            BehaviorTreeEditorStyles.startNodeColor = (int)NodeColor.Orange;
            BehaviorTreeEditorStyles.anyStateColor = (int)NodeColor.Aqua;
            BehaviorTreeEditorStyles.defaultNodeColor = (int)NodeColor.Grey;

            compositeNodeStyle = GetNodeStyle(2, false, false);
            decoratorNodeStyle = GetNodeStyle(2, false, false);
            conditionNodeStyle = GetNodeStyle(3, false, true);
            actionNodeStyle = GetNodeStyle(3, false, false);

            compositeNodeStyle_selected = GetNodeStyle(2, true, false);
            decoratorNodeStyle_selected = GetNodeStyle(2, true, false);
            conditionNodeStyle_selected = GetNodeStyle(3, true, true);
            actionNodeStyle_selected = GetNodeStyle(3, true, false);

            preLabel = new GUIStyle("WhiteLargeLabel");

        }

        private static Dictionary<string, GUIStyle> nodeStyleCache;

        private static string[] styleCache =
        {
            "flow node 0",
            "flow node 1",
            "flow node 2",
            "flow node 3",
            "flow node 4",
            "flow node 5",
            "flow node 6"
        };

        private static string[] styleCacheHex =
        {
            "flow node hex 0",
            "flow node hex 1",
            "flow node hex 2",
            "flow node hex 3",
            "flow node hex 4",
            "flow node hex 5",
            "flow node hex 6"
        };

        public static GUIStyle GetNodeStyle(int color, bool on, bool hex)
        {
            return GetNodeStyle(hex ? styleCacheHex[color] : styleCache[color], on, hex ? 8f : 2f);
        }

        public static GUIStyle GetNodeStyle(NodeDesigner node, bool on)
        {
            GUIStyle guiStyle = null;

            if (node.NodeType == NodeType.Decorator)
            {
                //if (BehaviorTreeUtility.CheckNodeError(node as BaseDecoratorNodeDesigner).Length > 0)
                //    guiStyle = GetNodeStyle(6, on, false) ;
                //else
                guiStyle = on ? decoratorNodeStyle_selected : decoratorNodeStyle;
            }
            else if (node.NodeType == NodeType.Composite)
            {
                //if (BehaviorTreeUtility.CheckNodeError(node as BaseCompositeNodeDesigner).Length > 0)
                //    guiStyle = GetNodeStyle(6, on, false) ;
                //else
                guiStyle = on ? decoratorNodeStyle_selected : decoratorNodeStyle;
            }
            else if (node.NodeType == NodeType.Condition)
            {
                //if (BehaviorTreeUtility.CheckNodeError(node as BaseCondictionNodeDesigner).Length > 0)
                //    guiStyle = GetNodeStyle(6, on, true);
                //else
                guiStyle = on ? conditionNodeStyle_selected : conditionNodeStyle;
            }
            else if (node.NodeType == NodeType.Action)
            {
                //if (BehaviorTreeUtility.CheckNodeError(node as BaseActionNodeDesigner).Length > 0)
                //    guiStyle = GetNodeStyle(6, on, false);
                //else
                guiStyle = on ? actionNodeStyle_selected : actionNodeStyle;
            }

            return guiStyle;
        }

        private static GUIStyle GetNodeStyle(string styleName, bool on, float offset)
        {
            string str = on ? string.Concat(styleName, " on") : styleName;
            if (!BehaviorTreeEditorStyles.nodeStyleCache.ContainsKey(str))
            {
                GUIStyle style = new GUIStyle(str);
                style.contentOffset = new Vector2(0, style.contentOffset.y - offset);
                if (on)
                {
                    style.fontStyle = FontStyle.Bold;
                }
                nodeStyleCache[str] = style;
            }
            return nodeStyleCache[str];
        }
    }

    public enum NodeColor
    {
        Grey = 0,
        Blue = 1,
        Aqua = 2,
        Green = 3,
        Yellow = 4,
        Orange = 5,
        Red = 6
    }
}
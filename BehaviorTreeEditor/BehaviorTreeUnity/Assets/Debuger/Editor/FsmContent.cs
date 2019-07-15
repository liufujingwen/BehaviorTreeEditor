using UnityEngine;
using System.Collections;

namespace BT.Debuger
{
    public static class FsmContent
    {
        public static string noticeStr = "提示";
        public static string deleteSkillConfirmStr = "确定删除该技能吗?";
        public static string actionNodeAddTips = "行为节点不能包含子节点";
        public static string decoratorNodeAddTips = "修饰节点最多只能包含一个子节点";

        public static GUIContent centerButtonStr;
        public static GUIContent deleteStr;
        public static GUIContent newStr;
        public static GUIContent saveStr;
        public static GUIContent copyAndPasteStr;
        public static GUIContent moveUpStr;
        public static GUIContent moveDownStr;
        public static GUIContent freshStr;


        public static GUIContent createState;

        public static GUIContent createSubFsm;

        public static GUIContent sequence;
        public static GUIContent makeTransition;
        public static GUIContent setAsDefault;
        public static GUIContent copy;
        public static GUIContent copyToDisc;
        public static GUIContent paste;
        public static GUIContent pasteFromDisc;
        public static GUIContent delete;
        public static GUIContent addToSelection;
        public static GUIContent saveAsAsset;
        public static GUIContent bindToGameObject;
        public static GUIContent moveToSubStateMachine;
        public static GUIContent moveToParentStateMachine;

        static FsmContent()
        {

            centerButtonStr = new GUIContent("居中");
            deleteStr = new GUIContent("删除");
            newStr = new GUIContent("新建");
            saveStr = new GUIContent("保存");
            copyAndPasteStr = new GUIContent("复制并粘贴");
            freshStr = new GUIContent("刷新");

            makeTransition = new GUIContent("连线");
            copy = new GUIContent("复制");
            copyToDisc = new GUIContent("复制到磁盘");
            paste = new GUIContent("粘贴");
            pasteFromDisc = new GUIContent("粘贴(磁盘)");
            delete = new GUIContent("删除");

            moveUpStr = new GUIContent("上移");
            moveDownStr = new GUIContent("下移");
        }
    }
}
using System;
using System.Collections.Generic;

namespace BehaviorTreeEditor
{
    public class BehaviorTreeDataDesigner
    {
        private GlobalVariableDesigner m_GlobalVariable = new GlobalVariableDesigner();
        private List<Group> m_Groups = new List<Group>();
        private List<BehaviorTreeDesigner> m_BehaviorTrees = new List<BehaviorTreeDesigner>();

        public GlobalVariableDesigner GlobalVariable
        {
            get { return m_GlobalVariable; }
            set { m_GlobalVariable = value; }
        }

        public List<Group> Groups
        {
            get { return m_Groups; }
            set { m_Groups = value; }
        }

        public List<BehaviorTreeDesigner> BehaviorTrees
        {
            get { return m_BehaviorTrees; }
            set { m_BehaviorTrees = value; }
        }

        /// <summary>
        /// 判断行为树是否存在
        /// </summary>
        /// <param name="behaviroTree">行为树</param>
        /// <returns>true:存在</returns>
        public bool ExistBehaviorTree(BehaviorTreeDesigner behaviroTree)
        {
            if (behaviroTree == null)
                throw new Exception("BehaviorTreeData.ExistBehaviorTree() error: behaviroTree = null");

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner tempBehaviorTree = m_BehaviorTrees[i];
                if (tempBehaviorTree == null)
                    continue;
                if (tempBehaviorTree == behaviroTree)
                    return true;
                if (tempBehaviorTree.ID == behaviroTree.ID)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判断行为树是否存在
        /// </summary>
        /// <param name="behaviorTreeID">behaviorTreeID</param>
        /// <returns>true:存在</returns>
        public bool ExistBehaviorTree(string behaviorTreeID)
        {
            if (string.IsNullOrEmpty(behaviorTreeID))
                throw new Exception("BehaviorTreeData.ExistBehaviorTree() error: behaviorTreeID = null");

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner tempBehaviorTree = m_BehaviorTrees[i];
                if (tempBehaviorTree == null)
                    continue;
                if (tempBehaviorTree.ID == behaviorTreeID)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 添加行为树
        /// </summary>
        /// <param name="behaviorTree">behaviorTree</param>
        /// <returns>true:添加成功</returns>
        public bool AddBehaviorTree(BehaviorTreeDesigner behaviorTree)
        {
            if (ExistBehaviorTree(behaviorTree))
                return false;

            m_BehaviorTrees.Add(behaviorTree);

            return true;
        }

        /// <summary>
        /// 删除行为树
        /// </summary>
        /// <param name="behaviroTree">behaviroTree</param>
        /// <returns>true:删除成功</returns>
        public bool RemoveBehaviorTree(BehaviorTreeDesigner behaviroTree)
        {
            if (behaviroTree == null)
                return false;

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner temp = m_BehaviorTrees[i];
                if (temp == null)
                    continue;
                if (temp.ID == behaviroTree.ID)
                {
                    m_BehaviorTrees.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 检验行为树ID
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyBehaviorTreeID()
        {
            //校验ID是否为空
            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (string.IsNullOrEmpty(behaviorTree.ID))
                {
                    return new VerifyInfo("行为树的ID为空");
                }
            }

            //检验行为树ID是否相同
            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree_i = m_BehaviorTrees[i];
                if (behaviorTree_i != null)
                {
                    for (int ii = i + 1; ii < m_BehaviorTrees.Count; ii++)
                    {
                        BehaviorTreeDesigner behaviorTree_ii = m_BehaviorTrees[ii];
                        if (behaviorTree_i.ID == behaviorTree_ii.ID)
                            return new VerifyInfo(string.Format("行为树存在相同ID:{0}", behaviorTree_i.ID));
                    }
                }
            }

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 校验行为树数据
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyBehaviorTree()
        {
            VerifyInfo verifyID = VerifyBehaviorTreeID();
            if (verifyID.HasError)
                return verifyID;

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (behaviorTree != null)
                {
                    VerifyInfo verifyBehaviorTree = behaviorTree.VerifyBehaviorTree();
                    if (verifyBehaviorTree.HasError)
                        return verifyBehaviorTree;
                }
            }
            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 移除未定义的节点
        /// </summary>
        public void RemoveUnDefineNode()
        {
            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (behaviorTree != null)
                    behaviorTree.RemoveUnDefineNode();
            }
        }


        /// <summary>
        /// 修正数据(和模板保持一致)
        /// </summary>
        public void AjustData()
        {
            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (behaviorTree != null)
                    behaviorTree.AjustData();
            }
        }

        /// <summary>
        /// 修改节点名字
        /// </summary>
        /// <param name="old">旧的classType</param>
        /// <param name="newType">新的classType</param>
        public void UpdateClassType(string old, string newType)
        {
            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (behaviorTree != null)
                    behaviorTree.UpdateClassType(old, newType);
            }
        }
    }
}

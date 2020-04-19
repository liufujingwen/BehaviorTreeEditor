using System;
using System.Collections.Generic;

namespace BehaviorTreeEditor
{
    public class BehaviorTreeDataDesigner
    {
        private VariableDesigner m_GlobalVariable = new VariableDesigner();
        private VariableDesigner m_ContextVariable = new VariableDesigner();
        private List<BehaviorGroupDesigner> m_Groups = new List<BehaviorGroupDesigner>();
        private List<BehaviorTreeDesigner> m_BehaviorTrees = new List<BehaviorTreeDesigner>();

        public VariableDesigner GlobalVariable
        {
            get { return m_GlobalVariable; }
            set { m_GlobalVariable = value; }
        }

        public VariableDesigner ContextVariable
        {
            get { return m_ContextVariable; }
            set { m_ContextVariable = value; }
        }

        public List<BehaviorGroupDesigner> Groups
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
                throw new ArgumentNullException("behaviroTree is null.");

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner tempBehaviorTree = m_BehaviorTrees[i];
                if (tempBehaviorTree == null)
                    continue;
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
            if (behaviorTree == null)
                throw new ArgumentNullException("behaviorTree is null."); ;

            if (ExistBehaviorTree(behaviorTree))
                return false;

            if (m_BehaviorTrees.Contains(behaviorTree))
                throw new Exception(string.Format("重复添加behaviorTree:{0}", behaviorTree.ID));

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
        /// 判断分组是否存在
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <returns>true:存在</returns>
        public bool ExistBehaviorGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
                throw new ArgumentException("BehaviorTreeDataDesigner.ExistBehaviorGroup() error: groupName = null");

            for (int i = 0; i < m_Groups.Count; i++)
            {
                BehaviorGroupDesigner behaviorGroup = m_Groups[i];
                if (behaviorGroup == null)
                    continue;
                if (behaviorGroup.GroupName == groupName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 添加行为树分组
        /// </summary>
        /// <param name="behaviorGroup"></param>
        public void AddGroup(BehaviorGroupDesigner behaviorGroup)
        {
            if (behaviorGroup == null)
                throw new ArgumentException("BehaviorTreeDataDesigner.AddGroup() error: behaviorGroup = null.");

            if (ExistBehaviorGroup(behaviorGroup.GroupName))
                return;

            if (m_Groups.Contains(behaviorGroup))
                throw new Exception(string.Format("BehaviorTreeDataDesigner.AddGroup() error: m_Groups aready contains behaviorGroup:{0}.", behaviorGroup.GroupName));

            m_Groups.Add(behaviorGroup);
        }

        /// <summary>
        /// 删除行为树分组
        /// </summary>
        /// <param name="behaviorGroup"></param>
        public bool RemoveGroup(BehaviorGroupDesigner behaviorGroup)
        {
            if (behaviorGroup == null)
                throw new ArgumentException("BehaviorTreeDataDesigner.RemoveGroup() error: behaviorGroup = null.");

            if (!ExistBehaviorGroup(behaviorGroup.GroupName))
                return false;

            for (int i = 0; i < m_Groups.Count; i++)
            {
                BehaviorGroupDesigner temp = m_Groups[i];
                if (temp == null)
                    continue;

                if (string.IsNullOrEmpty(temp.GroupName))
                    continue;

                if (temp.GroupName == behaviorGroup.GroupName)
                {
                    m_Groups.RemoveAt(i);
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

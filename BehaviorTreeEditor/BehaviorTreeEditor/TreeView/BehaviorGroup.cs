using System;
using System.Collections.Generic;

namespace BehaviorTreeEditor
{
    public class BehaviorGroup
    {
        private List<BehaviorTreeDesigner> m_BehaviorTrees = new List<BehaviorTreeDesigner>();


        public BehaviorGroup()
        {
        }

        public BehaviorGroup(string groupName)
        {
            GroupName = groupName;
        }

        public string GroupName = string.Empty;

        public List<BehaviorTreeDesigner> BehaviorTrees
        {
            get { return m_BehaviorTrees; }
            set { m_BehaviorTrees = value; }
        }

        /// <summary>
        /// 判断行为树是否存在
        /// </summary>
        /// <param name="behaviroTreeID">行为树ID</param>
        /// <returns>true:存在</returns>
        public bool ExistBehaviorTree(string behaviroTreeID)
        {
            if (string.IsNullOrEmpty(behaviroTreeID))
                throw new Exception("BehaviorGroup.ExistBehaviorTree() error: behaviroTreeID = null.");

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner tempBehaviorTree = m_BehaviorTrees[i];
                if (tempBehaviorTree == null)
                    continue;
                if (tempBehaviorTree.ID == behaviroTreeID)
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

            if (ExistBehaviorTree(behaviorTree.ID))
                return false;

            if (m_BehaviorTrees.Contains(behaviorTree))
                throw new Exception(string.Format("重复包含behaviorTree:{0}", behaviorTree.ID));

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

        public override string ToString()
        {
            return GroupName;
        }
    }
}

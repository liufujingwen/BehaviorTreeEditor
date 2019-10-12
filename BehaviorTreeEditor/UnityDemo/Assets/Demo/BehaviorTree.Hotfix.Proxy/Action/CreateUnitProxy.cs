using UnityEngine;
using BehaviorTreeData;
using Vector3 = UnityEngine.Vector3;

namespace R7BehaviorTree
{
    [ActionNode("CreateUnit")]
    public class CreateUnitProxy : BaseNodeProxy
    {
        private int m_ID;
        private EUnitType m_UnitType;
        private string m_Res;
        private Vector3 m_EulerAngles;
        private Vector3 m_Position;
        private int m_Hp;
        private int m_HpMax;
        private string m_Name;

        public override void OnAwake()
        {
            if (NodeData["ID"] == null)
            {
                Node.Status = ENodeStatus.Error;
                return;
            }

            if (NodeData["UnitType"] == null)
            {
                Node.Status = ENodeStatus.Error;
                return;
            }

            if (NodeData["Res"] == null)
            {
                Node.Status = ENodeStatus.Error;
                return;
            }

            if (NodeData["EulerAngles"] == null)
            {
                Node.Status = ENodeStatus.Error;
                return;
            }

            m_ID = NodeData["ID"];
            m_UnitType = (EUnitType)((EnumField)NodeData["UnitType"]).Value;
            m_Res = NodeData["Res"];
            Vector3Field eulerAngles = NodeData["EulerAngles"] as Vector3Field;
            m_EulerAngles = new Vector3(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
            Vector3Field position = NodeData["Position"] as Vector3Field;
            m_Position = new Vector3(position.X, position.Y, position.Z);
            m_Hp = NodeData["Hp"];
            m_HpMax = NodeData["HpMax"];
            m_Name = NodeData["Name"];
        }

        public override void OnStart()
        {
            Unit unit = UnitFactory.CreateUnit(m_Res, m_ID, m_UnitType);
            unit.gameObject.name = "Unit_" + m_Name;
            unit.SetName(m_Name);
            unit.EulerAngles = m_EulerAngles;
            unit.Position = m_Position;
            unit.SetAttr(AttrType.Hp, m_Hp);
            unit.SetAttr(AttrType.HpMax, m_HpMax);
            HudComponent hudComponent = unit.gameObject.AddComponent<HudComponent>();
            hudComponent.SetOwner(unit);
            Node.Status = ENodeStatus.Succeed;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviorTreeData;

namespace SerializeTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            TestIntField();
            TestFloatField();
            TestLongField();
            TestDoubleField();
            TestEnumField();
            TestNode();
            Console.ReadLine();
        }

        public static void TestIntField()
        {
            AgentData agent = new AgentData();
            agent.StartNode = new NodeData();

            IntField intField = new IntField();
            intField.FieldName = "IntField";
            intField.Value = 100;
            agent.StartNode.Fileds.Add(intField);

            RepeatIntField repeatIntField = new RepeatIntField();
            repeatIntField.FieldName = "RepeatIntField";
            repeatIntField.Value = new List<int>();
            repeatIntField.Value.Add(1);
            repeatIntField.Value.Add(100);
            repeatIntField.Value.Add(-1000000);
            repeatIntField.Value.Add(10000);
            agent.StartNode.Fileds.Add(repeatIntField);

            byte[] bytes = Serializer.Serialize(agent);
            AgentData deAgentData = Serializer.DeSerialize<AgentData>(bytes);
        }

        public static void TestLongField()
        {
            AgentData agent = new AgentData();
            agent.StartNode = new NodeData();

            LongField longField = new LongField();
            longField.FieldName = "LongField";
            longField.Value = 100;
            agent.StartNode.Fileds.Add(longField);

            RepeatLongField repeatLongField = new RepeatLongField();
            repeatLongField.FieldName = "RepeatLongField";
            repeatLongField.Value = new List<long>();
            repeatLongField.Value.Add(1);
            repeatLongField.Value.Add(100);
            repeatLongField.Value.Add(1000);
            repeatLongField.Value.Add(10000);
            agent.StartNode.Fileds.Add(repeatLongField);

            byte[] bytes = Serializer.Serialize(agent);
            AgentData deAgentData = Serializer.DeSerialize<AgentData>(bytes);
        }

        public static void TestFloatField()
        {
            AgentData agent = new AgentData();
            agent.StartNode = new NodeData();

            FloatField floatField = new FloatField();
            floatField.FieldName = "FloatField";
            floatField.Value = 100.001f;
            agent.StartNode.Fileds.Add(floatField);

            RepeatFloatField repeatFloatField = new RepeatFloatField();
            repeatFloatField.FieldName = "RepeatIntField";
            repeatFloatField.Value = new List<float>();
            repeatFloatField.Value.Add(1.1f);
            repeatFloatField.Value.Add(100.11f);
            repeatFloatField.Value.Add(1000.11f);
            repeatFloatField.Value.Add(10000.1119f);
            agent.StartNode.Fileds.Add(repeatFloatField);

            byte[] bytes = Serializer.Serialize(agent);
            AgentData deAgentData = Serializer.DeSerialize<AgentData>(bytes);
        }

        public static void TestDoubleField()
        {
            AgentData agent = new AgentData();
            agent.StartNode = new NodeData();

            DoubleField doubleField = new DoubleField();
            doubleField.FieldName = "DoubleField";
            doubleField.Value = 100.001d;
            agent.StartNode.Fileds.Add(doubleField);

            RepeatDoubleField repeatDoubleField = new RepeatDoubleField();
            repeatDoubleField.FieldName = "RepeatDoubleField";
            repeatDoubleField.Value = new List<double>();
            repeatDoubleField.Value.Add(1.1d);
            repeatDoubleField.Value.Add(100.11d);
            repeatDoubleField.Value.Add(1000.11d);
            repeatDoubleField.Value.Add(10000.1119d);
            agent.StartNode.Fileds.Add(repeatDoubleField);

            byte[] bytes = Serializer.Serialize(agent);
            AgentData deAgentData = Serializer.DeSerialize<AgentData>(bytes);
        }

        public static void TestEnumField()
        {
            AgentData agent = new AgentData();
            agent.StartNode = new NodeData();

            EnumField enumField = new EnumField();
            enumField.FieldName = "EnumField";
            enumField.EnumType = "TestEnum";
            enumField.Value = 100;
            agent.StartNode.Fileds.Add(enumField);

            byte[] bytes = Serializer.Serialize(agent);
            AgentData deAgentData = Serializer.DeSerialize<AgentData>(bytes);
        }

        public static void TestNode()
        {
            //创建AgentData
            AgentData agent = new AgentData();

            //添加开始节点
            agent.StartNode = new NodeData();

            //开始节点字段
            IntField intField = new IntField();
            intField.FieldName = "IntField";
            intField.Value = 100;
            agent.StartNode.Fileds.Add(intField);

            //创建开始节点的第一个子节点
            NodeData node1 = new NodeData();
            //子节点1的枚举字段
            EnumField enumField = new EnumField();
            enumField.FieldName = "EnumField";
            enumField.EnumType = "TestEnum";
            enumField.Value = 100;
            node1.Fileds.Add(enumField);

            //创建开始节点的第二个子节点
            NodeData node2 = new NodeData();
            //子节点2的Long字段
            LongField longField = new LongField();
            longField.FieldName = "LongField";
            longField.Value = 100;
            node2.Fileds.Add(longField);

            agent.StartNode.Childs = new List<NodeData>();
            agent.StartNode.Childs.Add(node1);
            agent.StartNode.Childs.Add(node2);

            byte[] bytes = Serializer.Serialize(agent);
            AgentData deAgentData = Serializer.DeSerialize<AgentData>(bytes);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTData;

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
            TestBooleanField();
            TestNode();
            Console.ReadLine();
        }

        public static void TestIntField()
        {
            BehaviorTreeElement behaviorTree = new BehaviorTreeElement();
            behaviorTree.StartNode = new NodeData();

            IntField intField = new IntField();
            intField.FieldName = "IntField";
            intField.Value = 100;
            behaviorTree.StartNode.Fields.Add(intField);

            RepeatIntField repeatIntField = new RepeatIntField();
            repeatIntField.FieldName = "RepeatIntField";
            repeatIntField.Value = new List<int>();
            repeatIntField.Value.Add(1);
            repeatIntField.Value.Add(100);
            repeatIntField.Value.Add(-1000000);
            repeatIntField.Value.Add(10000);
            behaviorTree.StartNode.Fields.Add(repeatIntField);

            byte[] bytes = Serializer.Serialize(behaviorTree);
            BehaviorTreeElement deBehaviorTreeData = Serializer.DeSerialize<BehaviorTreeElement>(bytes);
        }

        public static void TestLongField()
        {
            BehaviorTreeElement behaviorTree = new BehaviorTreeElement();
            behaviorTree.StartNode = new NodeData();

            LongField longField = new LongField();
            longField.FieldName = "LongField";
            longField.Value = 100;
            behaviorTree.StartNode.Fields.Add(longField);

            RepeatLongField repeatLongField = new RepeatLongField();
            repeatLongField.FieldName = "RepeatLongField";
            repeatLongField.Value = new List<long>();
            repeatLongField.Value.Add(1);
            repeatLongField.Value.Add(100);
            repeatLongField.Value.Add(1000);
            repeatLongField.Value.Add(10000);
            behaviorTree.StartNode.Fields.Add(repeatLongField);

            byte[] bytes = Serializer.Serialize(behaviorTree);
            BehaviorTreeElement deBehaviorTreeData = Serializer.DeSerialize<BehaviorTreeElement>(bytes);
        }

        public static void TestFloatField()
        {
            BehaviorTreeElement behaviorTree = new BehaviorTreeElement();
            behaviorTree.StartNode = new NodeData();

            FloatField floatField = new FloatField();
            floatField.FieldName = "FloatField";
            floatField.Value = 100.001f;
            behaviorTree.StartNode.Fields.Add(floatField);

            RepeatFloatField repeatFloatField = new RepeatFloatField();
            repeatFloatField.FieldName = "RepeatIntField";
            repeatFloatField.Value = new List<float>();
            repeatFloatField.Value.Add(1.1f);
            repeatFloatField.Value.Add(100.11f);
            repeatFloatField.Value.Add(1000.11f);
            repeatFloatField.Value.Add(10000.1119f);
            behaviorTree.StartNode.Fields.Add(repeatFloatField);

            byte[] bytes = Serializer.Serialize(behaviorTree);
            BehaviorTreeElement deBehaviorTreeData = Serializer.DeSerialize<BehaviorTreeElement>(bytes);
        }

        public static void TestDoubleField()
        {
            BehaviorTreeElement behaviorTree = new BehaviorTreeElement();
            behaviorTree.StartNode = new NodeData();

            DoubleField doubleField = new DoubleField();
            doubleField.FieldName = "DoubleField";
            doubleField.Value = 100.001d;
            behaviorTree.StartNode.Fields.Add(doubleField);

            RepeatDoubleField repeatDoubleField = new RepeatDoubleField();
            repeatDoubleField.FieldName = "RepeatDoubleField";
            repeatDoubleField.Value = new List<double>();
            repeatDoubleField.Value.Add(1.1d);
            repeatDoubleField.Value.Add(100.11d);
            repeatDoubleField.Value.Add(1000.11d);
            repeatDoubleField.Value.Add(10000.1119d);
            behaviorTree.StartNode.Fields.Add(repeatDoubleField);

            byte[] bytes = Serializer.Serialize(behaviorTree);
            BehaviorTreeElement deBehaviorTreeData = Serializer.DeSerialize<BehaviorTreeElement>(bytes);
        }

        public static void TestEnumField()
        {
            BehaviorTreeElement behaviorTree = new BehaviorTreeElement();
            behaviorTree.StartNode = new NodeData();

            EnumField enumField = new EnumField();
            enumField.FieldName = "EnumField";
            enumField.Value = 100;
            behaviorTree.StartNode.Fields.Add(enumField);

            byte[] bytes = Serializer.Serialize(behaviorTree);
            BehaviorTreeElement deBehaviorTreeData = Serializer.DeSerialize<BehaviorTreeElement>(bytes);
        }

        public static void TestBooleanField()
        {
            BehaviorTreeElement behaviorTree = new BehaviorTreeElement();
            behaviorTree.StartNode = new NodeData();

            BooleanField booleanField = new BooleanField();
            booleanField.FieldName = "BooleanField";
            booleanField.Value = true;
            behaviorTree.StartNode.Fields.Add(booleanField);

            byte[] bytes = Serializer.Serialize(behaviorTree);
            BehaviorTreeElement deBehaviorTreeData = Serializer.DeSerialize<BehaviorTreeElement>(bytes);
        }

        public static void TestNode()
        {
            BehaviorTreeElement behaviorTree = new BehaviorTreeElement();

            //添加开始节点
            behaviorTree.StartNode = new NodeData();

            //开始节点字段
            IntField intField = new IntField();
            intField.FieldName = "IntField";
            intField.Value = 100;
            behaviorTree.StartNode.Fields.Add(intField);

            //创建开始节点的第一个子节点
            NodeData node1 = new NodeData();
            //子节点1的枚举字段
            EnumField enumField = new EnumField();
            enumField.FieldName = "EnumField";
            enumField.Value = 100;
            node1.Fields.Add(enumField);

            //创建开始节点的第二个子节点
            NodeData node2 = new NodeData();
            //子节点2的Long字段
            LongField longField = new LongField();
            longField.FieldName = "LongField";
            longField.Value = 100;
            node2.Fields.Add(longField);

            behaviorTree.StartNode.Childs = new List<NodeData>();
            behaviorTree.StartNode.Childs.Add(node1);
            behaviorTree.StartNode.Childs.Add(node2);

            byte[] bytes = Serializer.Serialize(behaviorTree);
            BehaviorTreeElement deBehaviorTreeData = Serializer.DeSerialize<BehaviorTreeElement>(bytes);
        }
    }
}

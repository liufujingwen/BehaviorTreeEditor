using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public enum FieldType
    {
        None,
        IntField,
        LongField,
        FloatField,
        DoubleField,
        ColorField,
        EnumField,
        BooleanField,
        RepeatIntField,//int数组
        RepeatLongField,//Long数组
        RepeatFloatField,//浮点数组
        RepeatDoubleField,//双精度浮点数组
    }
}

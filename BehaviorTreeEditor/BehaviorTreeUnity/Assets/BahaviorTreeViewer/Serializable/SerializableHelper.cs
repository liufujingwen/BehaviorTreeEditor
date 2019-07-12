using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Serializable
{
    public static class SerializableHelper
    {
        static int msZone = 8;
        static string[] enumParameters = new string[1];
        static Type[] enumTypes = new Type[] { typeof(string) };

        static Transfer transfer = new Transfer();

        public static void AddType(Type type)
        {
            transfer.getDesc().AddClassType(type);
        }


        public static int zone
        {
            get { return msZone; }
            set { msZone = value; }
        }

        /// <summary>
        /// 序列化PO对象
        /// </summary>
        /// <typeparam name="T">BasePO</typeparam>
        /// <param name="instance">PO对象</param>
        /// <returns>二进制</returns>
        public static byte[] SerializeObject<T>(T instance) where T : BasePO
        {
            return instance.SerializeObject<T>(transfer);
        }

        /// <summary>
        /// 反序列化PO对象
        /// </summary>
        /// <typeparam name="T">BasePO</typeparam>
        /// <param name="buffer">二进制</param>
        /// <returns>PO对象</returns>
        public static T DeserializeObject<T>(byte[] buffer) where T : BasePO
        {
            T instance = Activator.CreateInstance<T>();
            object value = transfer.Decode(buffer);
            instance.SetData(value as PO);
            return instance;
        }

        //public static T DeserializeObject<T>(byte[] buffer) where T : BasePO
        //{
        //    T instance = Activator.CreateInstance<T>();
        //    object value = transfer.Decode(buffer);
        //    instance.SetData(value as PO);
        //    return instance;
        //}


        /// <summary>
        /// 反序列化PO对象
        /// </summary>
        /// <param name="type">BasePO</param>
        /// <param name="buffer">二进制</param>
        /// <returns>PO对象</returns>
        public static object DeserializeObject(Type type, byte[] buffer)
        {
            BasePO instance = Activator.CreateInstance(type) as BasePO;
            object value = transfer.Decode(buffer);
            instance.SetData(value as PO);
            return instance;
        }


        #region SetValue

        //===========================================================================================
        //-------------------------------------- SetPOValue -----------------------------------------
        //===========================================================================================

        /// <summary>
        /// 设置PO值（PO嵌套PO）
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="po">PO对象</param>
        /// <param name="key">PO.key</param>
        /// <param name="instance">BasePO对象</param>
        public static void SetPoValue<T>(PO po, string key, T instance) where T : BasePO
        {
            if (instance == null)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("PO存在重复的key:{0}", key));

            if (instance != null) po.Add(key, instance.GetPO());
        }

        /// <summary>
        /// 设置PO数组值（PO嵌套PO）
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="po">PO对象</param>
        /// <param name="key">PO.key</param>
        /// <param name="instances">BasePO对象列表</param>
        public static void SetPoValue__List<T>(PO po, string key, List<T> instances) where T : BasePO
        {
            if (instances == null)
                return;

            if (instances.Count == 0)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("PO存在重复的key:{0}", key));

            List<PO> pos = new List<PO>(instances.Count);
            for (int i = 0; i < instances.Count; i++)
            {
                T instance = instances[i];
                if (instance == null)
                    throw new Exception(string.Format("BasePO[]数组存在空值{0}", key));
                pos.Add(instance.GetPO());
            }

            po.Add(key, pos);
        }

        public static void SetPoValue__Dictionary<K, V>(PO po, string key, Dictionary<K, V> value) where V : BasePO
        {
            if (value == null)
                return;

            if (value.Count == 0)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("SetPoDicValue PO存在重复的key:{0}", key));

            if (typeof(K).IsSubclassOf(typeof(BasePO)))
                throw new Exception(string.Format("字典请不用使用BasePO对象作为key, key:{0}", key));

            Dictionary<K, PO> dic = new Dictionary<K, PO>();

            foreach (var kv in value)
            {
                if (kv.Value == null)
                    throw new Exception(string.Format("字典存在空值,key:{0}", kv.Key));

                K k_value = kv.Key;

                dic.Add(kv.Key, kv.Value.GetPO());
            }
            po.Add(key, dic);
        }

        public static void SetPoValue__Dictionary_List<K, V>(PO po, string key, Dictionary<K, List<V>> value) where V : BasePO
        {
            if (value == null)
                return;

            if (value.Count == 0)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("SetPoDicValue PO存在重复的key:{0}", key));

            if (typeof(K).IsSubclassOf(typeof(BasePO)))
                throw new Exception(string.Format("字典请不用使用BasePO对象作为key, key:{0}", key));

            Dictionary<K, List<PO>> dic = new Dictionary<K, List<PO>>(value.Count);

            foreach (var kv in value)
            {
                List<V> temps = kv.Value;
                if (temps.Count > 0)
                {
                    List<PO> pos = new List<PO>(temps.Count);
                    for (int i = 0; i < temps.Count; i++)
                    {
                        V basePo = temps[i];
                        if (basePo == null)
                            throw new Exception(string.Format("数组存在空值, key:{0}", key));
                        pos.Add(basePo.GetPO());
                    }

                    dic.Add(kv.Key, pos);
                }
            }

            po.Add(key, dic);
        }

        public static void SetPoValue__List_Dictionary_List<K, V>(PO po, string key, List<Dictionary<K, List<V>>> value) where V : BasePO
        {
            if (value == null)
                return;

            if (value.Count == 0)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("SetPoDicValue PO存在重复的key:{0}", key));

            if (typeof(K).IsSubclassOf(typeof(BasePO)))
                throw new Exception(string.Format("字典请不用使用BasePO对象作为key, key:{0}", key));

            List<Dictionary<K, List<PO>>> list = new List<Dictionary<K, List<PO>>>(value.Count);

            for (int i = 0; i < value.Count; i++)
            {
                Dictionary<K, List<V>> temp1 = (Dictionary<K, List<V>>)value[i];
                Dictionary<K, List<PO>> temp2 = new Dictionary<K, List<PO>>(value.Count);

                foreach (var kv in temp1)
                {
                    List<V> basePos = kv.Value;
                    if (basePos.Count > 0)
                    {
                        List<PO> pos = new List<PO>(basePos.Count);
                        for (int j = 0; j < 0; j++)
                        {
                            V basePo = basePos[j];
                            pos.Add(basePo.GetPO());
                        }
                        temp2.Add(kv.Key, pos);
                    }
                }
                list.Add(temp2);
            }

            po.Add(key, list);
        }

        //===========================================================================================
        //-------------------------------------- SetValue -------------------------------------------
        //===========================================================================================

        public static void SetValue__Dictionary<K, V>(PO po, string key, Dictionary<K, V> value)
        {
            if (value == null)
                return;

            if (value.Count == 0)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("PO存在重复的key:{0}", key));

            if (typeof(K).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            if (typeof(V).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            po.Add(key, value);
        }

        public static void SetValue__List_Dictionary<K, V>(PO po, string key, List<Dictionary<K, V>> value)
        {
            if (value == null)
                return;

            if (value.Count == 0)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("PO存在重复的key:{0}", key));

            if (typeof(K).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            if (typeof(V).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            po.Add(key, value);
        }

        public static void SetValue__List_Dictionary_List<K, V>(PO po, string key, List<Dictionary<K, List<V>>> value)
        {
            if (value == null)
                return;

            if (value.Count == 0)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("PO存在重复的key:{0}", key));

            if (typeof(K).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            if (typeof(V).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            po.Add(key, value);
        }

        public static void SetValue_Dictionary_Dictionary_List<K, K1, V>(PO po, string key, Dictionary<K, Dictionary<K1, List<V>>> value)
        {
            if (value == null)
                return;

            if (value.Count == 0)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("PO存在重复的key:{0}", key));

            if (typeof(K).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            if (typeof(K1).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            if (typeof(V).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            po.Add(key, value);
        }


        public static void SetValue__Dictionary_List<K, V>(PO po, string key, Dictionary<K, List<V>> value)
        {
            if (value == null)
                return;

            if (value.Count == 0)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("PO存在重复的key:{0}", key));

            if (typeof(K).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            if (typeof(V).IsSubclassOf(typeof(BasePO)))
                throw new Exception(value.GetType() + ",请使用SetPoValue");

            po.Add(key, value);
        }

        /// <summary>
        /// 设置值类型
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="po">PO对象</param>
        /// <param name="key">PO.key</param>
        /// <param name="value">PO.value</param>
        public static void SetValue<T>(PO po, string key, T value)
        {
            if (value == null)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("SetValue PO存在重复的key:{0}", key));

            if (typeof(T).IsSubclassOf(typeof(BasePO)))
                throw new Exception(string.Format("BasePO对象不能调用SetValue,key:{0}", key));

            po.Add(key, value);
        }

        public static void SetValue__List<T>(PO po, string key, List<T> value)
        {
            if (value == null)
                return;

            if (po.dic.ContainsKey(key))
                throw new Exception(string.Format("SetValue PO存在重复的key:{0}", key));

            if (typeof(T).IsSubclassOf(typeof(BasePO)))
                throw new Exception(string.Format("BasePO对象不能调用SetValue,key:{0}", key));

            po.Add(key, value);
        }

        #endregion

        #region GetValue

        //===========================================================================================
        //-------------------------------------- GetPoValue -----------------------------------------
        //===========================================================================================

        public static T GetPoValue<T>(PO po, string key, T instance) where T : BasePO
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    PO tempPo = (PO)obj;
                    if (instance == null)
                        instance = Activator.CreateInstance(tempPo.Type) as T;
                    instance.SetData(tempPo);
                }
            }
            return instance;
        }

        public static Dictionary<K, List<V>> GetPoValue__Dictionary_List<K, V>(PO po, string key, Dictionary<K, List<V>> defaultValue) where V : BasePO
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    var dic = (Dictionary<object, object>)obj;
                    if (dic != null && dic.Count > 0)
                    {
                        defaultValue = new Dictionary<K, List<V>>(dic.Count);
                        int i = 0;
                        foreach (var kv in dic)
                        {
                            //Type keyType = typeof(K);

                            //object k_value = kv.Key;

                            //if (IsEnum(keyType))
                            //    k_value = GetEnum<K>((string)kv.Key);

                            IList tempArrayList = (IList)kv.Value;

                            if (tempArrayList != null && tempArrayList.Count > 0)
                            {
                                List<V> basePos = new List<V>(tempArrayList.Count);
                                for (int j = 0; j < tempArrayList.Count; j++)
                                {
                                    PO o = (PO)tempArrayList[j];
                                    V tempBasePo = Activator.CreateInstance(o.Type) as V;
                                    tempBasePo.SetData(o);
                                    basePos.Add(tempBasePo);
                                }

                                defaultValue.Add((K)kv.Key, basePos);
                            }

                            i++;
                        }
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// PO数组
        /// </summary>
        /// <typeparam name="T">BasePO</typeparam>
        /// <param name="poDic"></param>
        /// <param name="key"></param>
        /// <param name="instances"></param>
        /// <returns></returns>
        public static List<T> GetPoValue__List<T>(PO po, string key, List<T> instances) where T : BasePO
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    IList objs = (IList)obj;
                    if (objs.Count > 0)
                    {
                        instances = new List<T>(objs.Count);
                        for (int i = 0; i < objs.Count; i++)
                        {
                            PO tempPo = (PO)objs[i];
                            T tempInstance = Activator.CreateInstance(tempPo.Type) as T;
                            instances.Add(tempInstance);
                            tempInstance.SetData(tempPo);
                        }
                    }
                }
            }
            return instances;
        }

        public static Dictionary<K, V> GetPoValue__Dictionary<K, V>(PO po, string key, Dictionary<K, V> defaultValue) where V : BasePO
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    var dic = (Dictionary<object, object>)obj;
                    if (dic != null && dic.Count > 0)
                    {
                        defaultValue = new Dictionary<K, V>();
                        foreach (var kv in dic)
                        {
                            //Type keyType = typeof(K);

                            //object k_value = (K)kv.Key;

                            //if (IsEnum(keyType))
                            //    k_value = GetEnum<K>((string)kv.Key);

                            PO tempPo = (PO)kv.Value;
                            V tempInstance = Activator.CreateInstance(tempPo.Type) as V;
                            tempInstance.SetData(tempPo);
                            defaultValue.Add((K)kv.Key, tempInstance);
                        }
                    }
                }
            }
            return defaultValue;
        }

        public static List<Dictionary<K, V>> GetPoValue__List_Dictionary<K, V>(PO po, string key, List<Dictionary<K, V>> defaultValue) where V : BasePO
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    IList list1 = (IList)obj;
                    if (list1 != null && list1.Count > 0)
                    {
                        defaultValue = new List<Dictionary<K, V>>(list1.Count);
                        for (int i = 0; i < list1.Count; i++)
                        {
                            var dic = (Dictionary<object, object>)list1[i];
                            if (dic != null && dic.Count > 0)
                            {
                                Dictionary<K, V> dic1 = new Dictionary<K, V>();
                                foreach (var kv in dic)
                                {
                                    V instance = Activator.CreateInstance<V>();
                                    instance.SetData((PO)kv.Value);

                                    //object k_value = kv.Key;

                                    //if (IsEnum(typeof(K)))
                                    //    k_value = GetEnum<K>((string)kv.Key);

                                    dic1.Add((K)kv.Key, instance);
                                }
                                defaultValue.Add(dic1);
                            }
                        }
                    }
                }
            }
            return defaultValue;
        }

        public static List<Dictionary<K, List<V>>> GetPoValue__List_Dictionary_List<K, V>(PO po, string key, List<Dictionary<K, List<V>>> defaultValue) where V : BasePO
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    IList list1 = (IList)obj;
                    if (list1 != null && list1.Count > 0)
                    {
                        defaultValue = new List<Dictionary<K, List<V>>>(list1.Count);
                        for (int i = 0; i < list1.Count; i++)
                        {
                            var dic = (Dictionary<object, object>)list1[i];
                            if (dic != null && dic.Count > 0)
                            {
                                Dictionary<K, List<V>> dic1 = new Dictionary<K, List<V>>(dic.Count);
                                foreach (var kv in dic)
                                {
                                    IList array2 = (IList)kv.Value;
                                    if (array2.Count > 0)
                                    {
                                        List<V> basePos = new List<V>(array2.Count);
                                        for (int j = 0; j < array2.Count; j++)
                                        {
                                            PO tempPo = (PO)array2[j];
                                            V instance = Activator.CreateInstance<V>();
                                            instance.SetData(tempPo);
                                            basePos.Add(instance);
                                        }

                                        //K k_value = (K)kv.Key;

                                        //if (IsEnum(typeof(K)))
                                        //    k_value = GetEnum<K>((string)kv.Key);

                                        dic1.Add((K)kv.Key, basePos);
                                    }
                                }
                                defaultValue.Add(dic1);
                            }
                        }
                    }
                }
            }
            return defaultValue;
        }

        //===========================================================================================
        //-------------------------------------- GetValue -----------------------------------------
        //===========================================================================================

        public static T GetValue<T>(PO po, string key, T defaultValue)
        {
            if (typeof(T).IsSubclassOf(typeof(BasePO)))
                throw new Exception(string.Format("key:{0},请使用SetPoValue", key));

            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                Type type = obj.GetType();
                if (obj != null)
                {
                    //if (IsEnum(typeof(T)))
                    //    return GetEnum<T>((string)obj);
                    //else
                    return (T)obj;
                }
            }
            return defaultValue;
        }

        public static List<T> GetValue__List<T>(PO po, string key, List<T> defaultValue)
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    IList array = (IList)obj;
                    if (array != null && array.Count > 0)
                    {
                        defaultValue = new List<T>(array.Count);
                        for (int i = 0; i < array.Count; i++)
                        {
                            //object tempObj = array[i];
                            //if (IsEnum(typeof(T)))
                            //    tempObj = GetEnum<T>((string)obj);
                            defaultValue.Add((T)array[i]);
                        }
                    }
                }
            }
            return defaultValue;
        }

        public static Dictionary<K, V> GetValue__Dictionary<K, V>(PO po, string key, Dictionary<K, V> defaultValue)
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    var dic = (Dictionary<object, object>)obj;
                    if (dic != null && dic.Count > 0)
                    {
                        defaultValue = new Dictionary<K, V>(dic.Count);
                        foreach (var kv in dic)
                        {
                            //Type keyType = typeof(K);
                            //Type valueType = typeof(V);

                            //object k_value = kv.Key;
                            //object v_value = kv.Value;

                            //if (IsEnum(keyType))
                            //    k_value = GetEnum<K>((string)kv.Key);

                            //if (IsEnum(valueType))
                            //    v_value = GetEnum<V>((string)kv.Value);

                            defaultValue.Add((K)kv.Key, (V)kv.Value);
                        }
                    }
                }
            }
            return defaultValue;
        }

        public static Dictionary<K, List<V>> GetValue__Dictionary_List<K, V>(PO po, string key, Dictionary<K, List<V>> defaultValue)
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    var dic = (Dictionary<object, object>)obj;
                    if (dic != null && dic.Count > 0)
                    {
                        defaultValue = new Dictionary<K, List<V>>(dic.Count);
                        foreach (var kv in dic)
                        {
                            //Type keyType = typeof(K);
                            //Type valueType = typeof(V);

                            //object k_value = kv.Key;
                            //if (IsEnum(keyType))
                            //    k_value = GetEnum<K>((string)kv.Key);

                            IList tempList = (IList)kv.Value;
                            if (tempList.Count > 0)
                            {
                                List<V> list = new List<V>(tempList.Count);
                                for (int i = 0; i < tempList.Count; i++)
                                {
                                    //object v_value = tempList[i];
                                    //if (IsEnum(typeof(V)))
                                    //    v_value = GetEnum<V>((string)kv.Key);
                                    list.Add((V)tempList[i]);
                                }

                                defaultValue.Add((K)kv.Key, list);
                            }
                        }
                    }
                }
            }
            return defaultValue;
        }

        public static List<Dictionary<K, V>> GetValue__List_Dictionary<K, V>(PO po, string key, List<Dictionary<K, V>> defaultValue)
        {
            if (po != null && po.dic.ContainsKey(key))
            {
                object obj = po[key];
                if (obj != null)
                {
                    var list = (IList)obj;

                    if (list.Count > 0)
                    {
                        defaultValue = new List<Dictionary<K, V>>(list.Count);

                        for (int i = 0; i < list.Count; i++)
                        {
                            var temp_1 = (Dictionary<object, object>)list[i];

                            if (temp_1.Count > 0)
                            {
                                Dictionary<K, V> temp2 = new Dictionary<K, V>();

                                foreach (var kv in temp_1)
                                {
                                    //Type keyType = typeof(K);
                                    //Type valueType = typeof(V);

                                    //object k_value = kv.Key;
                                    //object v_value = kv.Value;

                                    //if (IsEnum(keyType))
                                    //    k_value = GetEnum<K>((string)kv.Key);

                                    //if (IsEnum(valueType))
                                    //    v_value = GetEnum<V>((string)kv.Value);

                                    temp2.Add((K)kv.Key, (V)kv.Value);
                                }

                                defaultValue.Add(temp2);
                            }
                        }
                    }
                }
            }
            return defaultValue;
        }

        #endregion

        //通过反射取对应的枚举
        //static T GetEnum<T>(string enumKey)
        //{
        //    MethodInfo methodInfo = typeof(T).GetMethod("Get", enumTypes);
        //    enumParameters[0] = enumKey;
        //    object obj = methodInfo.Invoke(null, enumParameters);
        //    //if (obj == null)
        //    //    Logger.LogErrorFormat("枚举:{0},不存在枚举项:{1}", typeof(T).Name, enumKey);
        //    return (T)obj;
        //}

        //static bool IsEnum(Type type)
        //{
        //    return type != null ? type.BaseType == typeof(BaseEnum) : false;
        //}
    }
}

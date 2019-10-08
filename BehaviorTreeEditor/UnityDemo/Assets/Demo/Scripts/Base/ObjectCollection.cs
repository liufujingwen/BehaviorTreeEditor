using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ObjectCollection : MonoBehaviour
{
    [System.Serializable]
    public class ObjectItem
    {
        public string Key;
        public Object Source;
        public string TypeName;
    }

    public ObjectItem[] Items;
    private Dictionary<string, Object> m_ItemDic;
    private bool m_Initalize;

    public Object Get(string key)
    {
        Init();
        Object obj = null;
        if (m_ItemDic == null || !m_ItemDic.TryGetValue(key, out obj))
        {
            throw new System.Exception(string.Format("ObjectCollection not found key  {0}", key));
        }
        return obj;
    }

    public T GetT<T>(string key) where T : Object
    {
        Init();
        Object obj = null;
        if (m_ItemDic == null || !m_ItemDic.TryGetValue(key, out obj))
        {
            throw new System.Exception(string.Format("ObjectCollection not found key  {0}", key));
        }
        return (T)obj;
    }

    void OnDestroy()
    {
        m_ItemDic = null;
        Items = null;
    }

    void Init()
    {
        if (m_Initalize)
            return;

        m_Initalize = true;
        if (Items != null && Items.Length > 0)
        {
            m_ItemDic = new Dictionary<string, Object>();
            for (int i = 0; i < Items.Length; i++)
            {
                Object obj = Items[i].Source;
                if (obj != null)
                    m_ItemDic[Items[i].Key] = obj;
            }
        }
    }
}
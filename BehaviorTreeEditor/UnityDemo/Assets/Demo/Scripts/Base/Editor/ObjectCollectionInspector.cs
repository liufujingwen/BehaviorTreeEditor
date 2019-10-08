using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using Object = UnityEngine.Object;

[CustomEditor(typeof(ObjectCollection))]
public class ObjectCollectionInspector : Editor
{
    private ObjectCollection m_ObjectCollection;

    private void OnEnable()
    {
        m_ObjectCollection = target as ObjectCollection;
    }

    public override void OnInspectorGUI() 
    {
        EditorGUILayout.Space();

        List<ObjectCollection.ObjectItem> itemlist = new List<ObjectCollection.ObjectItem>();

        if (m_ObjectCollection.Items != null)
            itemlist.AddRange(m_ObjectCollection.Items);
        else
            m_ObjectCollection.Items = new ObjectCollection.ObjectItem[0];

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("+", GUILayout.Width(30)))
        {
            ObjectCollection.ObjectItem tmpItem = new ObjectCollection.ObjectItem();
            itemlist.Add(tmpItem);
        }

        if (GUILayout.Button("自动设置key", GUILayout.Width(100)))
        {
            for (int i = 0; i < itemlist.Count; i++)
            {
                ObjectCollection.ObjectItem item = itemlist[i];
                if (item != null && item.Source != null)
                {
                    item.Key = item.Source.name;
                }
            }
        }

        if (GUILayout.Button("检查重名元素", GUILayout.Width(100)))
        {
            ValidateItems(m_ObjectCollection);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        bool typeChange = false;

        for (int i = 0; i < m_ObjectCollection.Items.Length; i++)
        {
            ObjectCollection.ObjectItem item = m_ObjectCollection.Items[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Key:", GUILayout.Width(30));
            item.Key = EditorGUILayout.TextField(item.Key, GUILayout.Width(120));

            Object t1 = item.Source;
            item.Source = EditorGUILayout.ObjectField(item.Source, typeof(Object), true);

            if (t1 == null && item.Source != null)
            {
                item.Key = item.Source.name;
            }
            if (t1 != item.Source)
            {
                typeChange = true;
            }
            List<string> names = new List<string>();
            List<System.Type> types = new List<System.Type>();
            if (item.TypeName == null)
                item.TypeName = typeof(Object).FullName;
            if (item.Source != null)
            {
                GameObject gameObj = item.Source as GameObject;
                if (gameObj == null)
                {
                    Component tObj = item.Source as Component;
                    if (tObj != null)
                    {
                        gameObj = tObj.gameObject;
                    }
                }
                names.Add(typeof(Object).ToString());
                types.Add(typeof(Object));
                if (gameObj != null)
                {
                    MonoBehaviour[] behaviours = gameObj.GetComponents<MonoBehaviour>();

                    for (int i2 = 0; i2 < behaviours.Length; i2++)
                    {
                        System.Type type = behaviours[i2].GetType();
                        if (!names.Contains(type.ToString()))
                        {
                            names.Add(type.ToString());
                            types.Add(type);
                        }
                    }
                }
            }
            else
            {
                names.Add(typeof(Object).ToString());
                types.Add(typeof(Object));
            }
            System.Type sourceType = typeof(Object);

            if (item.TypeName != null && item.Source != null)
            {
                sourceType = GetTypeForName(item.TypeName);
            }

            if (sourceType == typeof(GameObject))
                sourceType = typeof(Object);

            int oldSelectInx = types.IndexOf(sourceType);
            if (oldSelectInx == -1)
            {
                sourceType = typeof(Object);
                oldSelectInx = types.IndexOf(sourceType);
                item.TypeName = sourceType.FullName;
                typeChange = true;
            }

            GUILayout.FlexibleSpace();

            int selectInx = EditorGUILayout.Popup(oldSelectInx, names.ToArray(),GUILayout.Width(120));
            if (selectInx != oldSelectInx)
            {
                item.TypeName = types[selectInx].FullName;
                typeChange = true;
            }

            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                itemlist.Remove(item);
                i--;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        if (typeChange)
            CheckAllType(m_ObjectCollection);

        m_ObjectCollection.Items = itemlist.ToArray();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(m_ObjectCollection);
        }
    }

    public System.Type GetTypeForName(string name)
    {
        if (string.IsNullOrEmpty((name)) || name == typeof(Object).FullName)
        {
            return typeof(Object);
        }

        Assembly[] assemblys = System.AppDomain.CurrentDomain.GetAssemblies();
        foreach (var item in assemblys)
        {
            System.Type t = item.GetType(name);
            if (t != null)
                return t;
        }
        return null;
    }

    private void CheckAllType(ObjectCollection target)
    {
        foreach (var item in target.Items)
        {
            if (item.Source == null)
            {
                item.TypeName = typeof(Object).FullName;
            }
            else
            {
                if (item.Source.GetType() != GetTypeForName(item.TypeName))
                {
                    GameObject gameObj = item.Source as GameObject;
                    if (gameObj == null)
                    {
                        Component comp = item.Source as Component;
                        if (comp == null)
                            continue;
                        gameObj = comp.gameObject;
                    }
                    if (GetTypeForName(item.TypeName) == typeof(Object))
                    {
                        item.Source = gameObj;
                    }
                    else
                    {
                        Component c = gameObj.GetComponent(GetTypeForName(item.TypeName));
                        item.Source = c;
                    }
                }
            }
        }
    }

    void ValidateItems(ObjectCollection target)
    {
        ObjectCollection.ObjectItem[] items = target.Items;
        if (items == null)
            return;

        HashSet<Object> dics = new HashSet<Object>();
        List<ObjectCollection.ObjectItem> list = new List<ObjectCollection.ObjectItem>();
        for (int i = 0; i < items.Length; i++)
        {
            Object obj = items[i].Source;
            if (obj != null && !dics.Contains(obj))
            {
                dics.Add(obj);
                list.Add(items[i]);
            }
            else
            {
                Debug.LogWarning("重复元素 " + i.ToString() + "  " + obj);
            }
        }
        ObjectCollection.ObjectItem[] tmpItems = list.ToArray();

        HashSet<string> tmpDic = new HashSet<string>();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < tmpItems.Length; i++)
        {
            if (tmpDic.Contains(tmpItems[i].Key))
            {
                sb.AppendLine("Element: " + i.ToString() + "\nKey: " + tmpItems[i].Key + "\nSource: " + tmpItems[i].Source);
                sb.AppendLine();
            }
            tmpDic.Add(items[i].Key);
        }
        if (sb.Length > 0)
        {
            EditorUtility.DisplayDialog("", "key重复\n" + sb.ToString(), "OK");
            Debug.LogError("key重复\n" + sb.ToString());
        }
        else
        {
            EditorUtility.DisplayDialog("", "done!", "OK");
        }
    }
}
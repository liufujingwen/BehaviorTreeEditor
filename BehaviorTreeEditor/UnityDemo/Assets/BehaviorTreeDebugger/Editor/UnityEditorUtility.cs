using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Reflection;
using System;

namespace BT
{
    /// <summary>
    /// Editor helper class.
    /// </summary>
    public static class UnityEditorUtility
    {
        private readonly static Dictionary<Type, object[]> typeAttributeCache;
        private readonly static Dictionary<FieldInfo, object[]> fieldAttributeCache;

        static UnityEditorUtility()
        {
            typeAttributeCache = new Dictionary<Type, object[]>();
            fieldAttributeCache = new Dictionary<FieldInfo, object[]>();
        }

        /// <summary>
        /// Gets custom attributes from type.
        /// </summary>
        /// <returns>The custom attributes.</returns>
        /// <param name="type">Type.</param>
        public static object[] GetCustomAttributes(Type type)
        {
            object[] customAttributes;
            if (!UnityEditorUtility.typeAttributeCache.TryGetValue(type, out customAttributes))
            {
                customAttributes = type.GetCustomAttributes(true);
                UnityEditorUtility.typeAttributeCache.Add(type, customAttributes);
            }
            return customAttributes;
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <returns>The custom attributes.</returns>
        /// <param name="field">Field.</param>
        public static object[] GetCustomAttributes(FieldInfo field)
        {
            object[] customAttributes;
            if (!UnityEditorUtility.fieldAttributeCache.TryGetValue(field, out customAttributes))
            {
                customAttributes = field.GetCustomAttributes(true);
                UnityEditorUtility.fieldAttributeCache.Add(field, customAttributes);
            }
            return customAttributes;
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <returns>The attribute.</returns>
        /// <param name="field">Field.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetAttribute<T>(this FieldInfo field)
        {
            object[] objArray = UnityEditorUtility.GetCustomAttributes(field);
            for (int i = 0; i < (int)objArray.Length; i++)
            {
                if (objArray[i].GetType() == typeof(T) || objArray[i].GetType().IsSubclassOf(typeof(T)))
                {
                    return (T)objArray[i];
                }
            }
            return default(T);
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <returns>The attribute.</returns>
        /// <param name="type">Type.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetAttribute<T>(this Type type)
        {
            object[] objArray = UnityEditorUtility.GetCustomAttributes(type);
            for (int i = 0; i < (int)objArray.Length; i++)
            {
                if (objArray[i].GetType() == typeof(T) || objArray[i].GetType().IsSubclassOf(typeof(T)))
                {
                    return (T)objArray[i];
                }
            }
            return default(T);
        }

        /// <summary>
        /// Search field gui.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="search">Search.</param>
        /// <param name="options">Options.</param>
        public static string SearchField(string search, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            string before = search;
            string after = EditorGUILayout.TextField("", before, "SearchTextField", options);

            if (GUILayout.Button("", "SearchCancelButton", GUILayout.Width(18f)))
            {
                after = "Search...";
                GUIUtility.keyboardControl = 0;
            }
            GUILayout.EndHorizontal();
            return after;
        }

        /// <summary>
        /// Gets the type of the assets of.
        /// </summary>
        /// <returns>The assets of type.</returns>
        /// <param name="fileExtension">File extension.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T[] GetAssetsOfType<T>(string fileExtension)
        {
            List<T> tempObjects = new List<T>();
            DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
            FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);

            int i = 0; int goFileInfoLength = goFileInfo.Length;
            FileInfo tempGoFileInfo; string tempFilePath;
            UnityEngine.Object tempGO;
            for (; i < goFileInfoLength; i++)
            {
                tempGoFileInfo = goFileInfo[i];
                if (tempGoFileInfo == null)
                    continue;

                tempFilePath = tempGoFileInfo.FullName;
                tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");

                tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(UnityEngine.Object));
                if (tempGO == null)
                {
                    continue;
                }
                else if (tempGO.GetType() != typeof(T))
                {
                    continue;
                }

                tempObjects.Add((T)(object)tempGO);
            }

            return tempObjects.ToArray();
        }

        /// <summary>
        /// Finds components the in scene.
        /// </summary>
        /// <returns>The in scene.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> FindInScene<T>() where T : Component
        {
            T[] comps = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

            List<T> list = new List<T>();

            foreach (T comp in comps)
            {
                if (comp.gameObject.hideFlags == 0)
                {
                    string path = AssetDatabase.GetAssetPath(comp.gameObject);
                    if (string.IsNullOrEmpty(path)) list.Add(comp);
                }
            }
            return list;
        }

        public static void DrawProperties(SerializedObject obj, params string[] properties)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                SerializedProperty property = obj.FindProperty(properties[i]);
                if (property != null)
                {
                    EditorGUILayout.PropertyField(obj.FindProperty(properties[i]));
                }
            }
        }
    }
}
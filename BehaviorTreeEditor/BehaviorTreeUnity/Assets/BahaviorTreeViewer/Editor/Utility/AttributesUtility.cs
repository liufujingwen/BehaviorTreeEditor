using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace BT.Editor
{
    public static class AttributeUtility
    {
        private readonly static Dictionary<Type, object[]> typeAttributeCache;
        private readonly static Dictionary<FieldInfo, object[]> fieldAttributeCache;
        private readonly static Dictionary<FieldInfo, GUIContent> inspectorContentCache;

        static AttributeUtility()
        {
            AttributeUtility.typeAttributeCache = new Dictionary<Type, object[]>();
            AttributeUtility.fieldAttributeCache = new Dictionary<FieldInfo, object[]>();
            AttributeUtility.inspectorContentCache = new Dictionary<FieldInfo, GUIContent>();
        }

        public static object[] GetCustomAttributes(Type type)
        {
            object[] customAttributes;
            if (!AttributeUtility.typeAttributeCache.TryGetValue(type, out customAttributes))
            {
                customAttributes = type.GetCustomAttributes(true);
                AttributeUtility.typeAttributeCache.Add(type, customAttributes);
            }
            return customAttributes;
        }

        public static object[] GetCustomAttributes(FieldInfo field)
        {
            object[] customAttributes;
            if (!AttributeUtility.fieldAttributeCache.TryGetValue(field, out customAttributes))
            {
                customAttributes = field.GetCustomAttributes(true);
                AttributeUtility.fieldAttributeCache.Add(field, customAttributes);
            }
            return customAttributes;
        }


        //public static Type GetCustomDrawerAttribute(Type type)
        //{
        //    return AttributeUtility.GetCustomDrawerAttribute(AttributeUtility.GetCustomAttributes(type));
        //}

        public static Type GetPropertyAttribute(FieldInfo field)
        {
            return AttributeUtility.GetPropertyAttribute(AttributeUtility.GetCustomAttributes(field)) ?? field.FieldType;
        }

        public static Type GetPropertyAttribute(object[] attributes)
        {
            object[] objArray = attributes;
            for (int i = 0; i < (int)objArray.Length; i++)
            {
                PropertyAttribute propertyAttribute = objArray[i] as PropertyAttribute;
                if (propertyAttribute != null)
                {
                    return propertyAttribute.GetType();
                }
            }
            return null;
        }

        public static string GetCategory(this object obj)
        {
            return GetCategory(obj.GetType());
        }

        public static string GetTooltip(this object obj)
        {
            return AttributeUtility.GetTooltip(obj.GetType());
        }

        public static string GetTooltip(this Type type)
        {
            return AttributeUtility.GetTooltip(AttributeUtility.GetCustomAttributes(type));
        }

        public static string GetTooltip(this FieldInfo field)
        {
            return AttributeUtility.GetTooltip(AttributeUtility.GetCustomAttributes(field));
        }

        public static string GetHelpUrl(this object obj)
        {
            return GetHelpUrl(obj.GetType());
        }

        public static string GetHelpUrl(this Type type)
        {
            object[] objArray = AttributeUtility.GetCustomAttributes(type);
            //for (int i = 0; i < (int)objArray.Length; i++)
            //{
            //    HelpUrlAttribute infoAttribute = objArray[i] as HelpUrlAttribute;
            //    if (infoAttribute != null)
            //    {
            //        return infoAttribute.Url;
            //    }
            //}
            return string.Empty;
        }
    }
}
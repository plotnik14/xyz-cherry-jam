﻿#if  UNITY_EDITOR
using System;
using UnityEditor;

namespace CherryJam.Utils.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static bool TryGetEnum<TEnumType>(this SerializedProperty property, out TEnumType retValue) 
            where TEnumType : Enum
        {
            retValue = default;
            var names = property.enumNames;
            if (names == null || names.Length == 0)
            {
                return false;
            }

            var enumName = names[property.enumValueIndex];
            retValue = (TEnumType) Enum.Parse(typeof(TEnumType), enumName);
            return true;
        }
    }
}
#endif
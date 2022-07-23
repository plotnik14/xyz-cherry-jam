using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Model.Definition.Editor
{
    [CustomPropertyDrawer(typeof(InventoryIdAttribute))]
    public class InventoryIdAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defs = DefsFacade.I.Items.ItemsForEditor;
            var ids = new List<string>();
            foreach (var itemDef in defs)
            {
                ids.Add(itemDef.Id);
            }

            var index = ids.IndexOf(property.stringValue);
            index = EditorGUI.Popup(position, property.displayName, index, ids.ToArray());
            property.stringValue = ids[index];
        }
    }
}

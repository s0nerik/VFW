using System;
using System.Reflection;
using UnityEditorInternal;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Drawers
{
	public class SortingLayerDrawer : AttributeDrawer<string, SortingLayerAttribute>
	{
        private string[] _variables;
        private int _current;

        protected override void Initialize()
        {
            if (memberValue == null)
                memberValue = "Default";

            FetchVariables();
        }

        private string[] GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[]) sortingLayersProperty.GetValue(null, new object[0]);
        }

        private void FetchVariables()
        {
            _variables = GetSortingLayerNames();

            if (_variables.IsEmpty())
                _variables = new[] { "Default" };
            else
            {
                _current = _variables.IndexOfZeroIfNotFound(memberValue);
            }
        }

        public override void OnGUI()
        {
            if (_variables.IsNullOrEmpty())
                FetchVariables();

            var selection = gui.Popup(displayText, _current, _variables);
            {
                if (_current != selection || memberValue != _variables[selection])
                    memberValue = _variables[_current = selection];
            }
        }
    }
}
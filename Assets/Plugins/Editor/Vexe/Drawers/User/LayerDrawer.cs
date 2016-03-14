using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Drawers
{
	public class LayerDrawer : AttributeDrawer<string, LayerAttribute>
	{
        private string[] _variables;
        private int _current;

        private string _lastMemberValue;

        protected override void Initialize()
        {
            if (memberValue == null)
                memberValue = "Default";

            FetchVariables();
        }

        private string[] GetLayerNames()
        {
            var names = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                var name = LayerMask.LayerToName(i);
                if (!name.IsNullOrEmpty())
                    names.Add(LayerMask.LayerToName(i));
            }
            return names.ToArray();
        }

        private void FetchVariables()
        {
            _variables = GetLayerNames();

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

            if (memberValue != _lastMemberValue)
                _current = _variables.IndexOf(memberValue);

            var selection = gui.Popup(displayText, _current, _variables);
            {
                if (_current != selection || memberValue != _variables[selection])
                    memberValue = _variables[_current = selection];
            }

            _lastMemberValue = memberValue;
        }
    }
}
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Drawers
{
	public class AnimStateDrawer : AttributeDrawer<string, AnimStateAttribute>
	{
		private string[] _variables;
		private int _current;

        private string _lastMemberValue;

        private Animator _animator;
		private Animator animator
		{
			get
			{
				if (_animator == null)
				{
					string getterMethod = attribute.GetAnimatorMethod;
					if (getterMethod.IsNullOrEmpty())
						_animator = gameObject.GetComponent<Animator>();
					else
						_animator = targetType.GetMethod(getterMethod, Flags.InstanceAnyVisibility)
											  .Invoke(rawTarget, null) as Animator;
				}
				return _animator;
			}
		}

		protected override void Initialize()
		{
			if (memberValue == null)
				memberValue = "";

			if (animator != null && animator.runtimeAnimatorController != null)
				FetchVariables();
		}

		private void FetchVariables()
		{
            string assetPath = AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);

            AnimatorController ctrl;
            var overrideCtrl = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(assetPath);
            if (overrideCtrl == null)
                ctrl = AssetDatabase.LoadAssetAtPath<AnimatorController>(assetPath);
            else
            {
                assetPath = AssetDatabase.GetAssetPath(overrideCtrl.runtimeAnimatorController);
                ctrl = AssetDatabase.LoadAssetAtPath<AnimatorController>(assetPath);
            }

		    if (attribute.Layer < ctrl.layers.Length)
		    {
		        var states = ctrl.layers[attribute.Layer].stateMachine.states;
		        _variables = states.Select(s => s.state.name).ToArray();
		    }
		    else
		    {
                _variables = new string[] {};
            }

			if (_variables.IsEmpty())
				_variables = new[] { "N/A" };
			else
			{
                _current = _variables.IndexOfZeroIfNotFound(memberValue);
			}
		}

		public override void OnGUI()
		{
			if (animator == null || animator.runtimeAnimatorController == null)
			{
				memberValue = gui.Text(displayText, memberValue);
			}
			else
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
}
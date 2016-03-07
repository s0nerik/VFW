using System;

namespace Vexe.Runtime.Types
{
	/// <summary>
	/// Apply this to a string to get a popup of all the available variables in the Animator component that's attached to the owner's gameObject
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
	public class AnimStateAttribute : DrawnAttribute
	{
		/// <summary>
		/// A method to get the animator in case it wasn't attached to the target's gameObject
		/// If this was left out, the drawer will use the target's gameObject to get the animator from
		/// The method should return an Animator and take no parameters
		/// </summary>
		public string GetAnimatorMethod { get; set; }

		/// <summary>
		/// Layer from which the states should be gathered.
		/// </summary>
		public int Layer { get; set; }
	}
}
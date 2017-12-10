using System;
using System.Reflection;
using UnityEngine;

namespace LocalizationModule.Supports {
	/// <summary>
	/// Translation property support.
	/// </summary>
	[Serializable]
	public class TranslationProperty {
		#region public accessors		
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get { return name; } }

		/// <summary>
		/// Gets a value indicating whether the name should be kept in the translation data.
		/// </summary>
		/// <value>
		///   <c>true</c> if [keep name in data]; otherwise, <c>false</c>.
		/// </value>
		public bool KeepName { get { return keepName; } }

		/// <summary>
		/// Gets the alias.
		/// </summary>
		/// <value>
		/// The alias.
		/// </value>
		public string[] Aliases { get { return aliases; } }

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public string Value { get { return FieldInfo.GetValue(Component).ToString(); } }

		/// <summary>
		/// The component.
		/// </summary>
		[NonSerialized] public UnityEngine.Component Component;

		/// <summary>
		/// The field information.
		/// </summary>
		public FieldInfo FieldInfo;
		#endregion

		#region private properties
		/// <summary>
		/// The name of the property.
		/// </summary>
		[SerializeField]
		private string name;

		/// <summary>
		/// Indicates if the name should be kept in the translation data.
		/// </summary>
		[SerializeField]
		private bool keepName = true;

		/// <summary>
		/// The alias of the property.
		/// </summary>
		[SerializeField]
		private string[] aliases;
		#endregion
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LocalizationModule.Supports {
	/// <summary>
	/// Translation attribute support.
	/// </summary>
	[Serializable]
	public class TranslationAttribute {
		#region public accessors		
		/// <summary>
		/// Gets the events.
		/// </summary>
		/// <value>
		/// The events.
		/// </value>
		public List<TranslationEventBuilder> Events {
			get {
				var eventList = new List<TranslationEventBuilder>();
				var component = gameObject.GetComponent(componentName);
				if(component == null) return eventList;
				var containerType = component.GetType();
				eventList.AddRange(from eventName in events select containerType.GetEvent(eventName) into eventInfo where eventInfo != null select new TranslationEventBuilder(component, eventInfo));
				return eventList;
			}
		}
		#endregion

		#region public properties
		/// <summary>
		/// The game object containing the attribute.
		/// </summary>
		[SerializeField] private GameObject gameObject;

		/// <summary>
		/// The component name.
		/// </summary>
		[SerializeField] private string componentName;

		/// <summary>
		/// The component properties.
		/// </summary>
		[SerializeField] private TranslationProperty[] properties;

		/// <summary>
		/// The component events to subscribe.
		/// </summary>
		[SerializeField] private string[] events;
		#endregion

		#region public methods
		/// <summary>
		/// Gets the attributes data as dictionary.
		/// </summary>
		/// <returns>A dictionary containing the attributes data</returns>
		public Dictionary<string, TranslationProperty> ToDictionary() {
			var result = new Dictionary<string, TranslationProperty>();
			var component = gameObject.GetComponent(componentName);
			if(component == null) return result;
			var containerType = component.GetType();
			foreach(var property in properties) {
				var key = property.Name;
				var fieldInfo = containerType.GetField(key);
				if (fieldInfo == null) continue;
				property.FieldInfo = fieldInfo;
				property.Component = component;
				result[key] = property;
			}
			return result;
		}
		#endregion
	}
}
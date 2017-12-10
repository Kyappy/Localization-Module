using System.Collections.Generic;
using System.Text.RegularExpressions;
using LocalizationModule.Supports;
using UnityEngine;
using UnityEngine.UI;

namespace LocalizationModule.Component.UI {
	/// <inheritdoc />
	/// <summary>
	/// Localized UI text.
	/// </summary>
	/// <seealso cref="T:UnityEngine.MonoBehaviour" />
	[RequireComponent(typeof(Text))]
	public class LocalizedText: MonoBehaviour {
		#region public properties
		/// <summary>
		/// The attributes.
		/// </summary>
		public TranslationAttribute[] Attributes; 
		#endregion

		#region private fields				
		/// <summary>
		/// The original content.
		/// </summary>
		private string originalContent;

		/// <summary>
		/// The text component.
		/// </summary>
		private Text textComponent;

		/// <summary>
		/// The trans regular expression.
		/// </summary>
		private readonly Regex transRgx = new Regex(@"@trans\(([^|\)]*)(?:\|(.*))?\)");

		/// <summary>
		/// The translation data.
		/// </summary>
		private readonly Dictionary<string, TranslationProperty> data = new Dictionary<string, TranslationProperty>();

		/// <summary>
		/// The events.
		/// </summary>
		private readonly List<TranslationEventBuilder> translationEventBuilders = new List<TranslationEventBuilder>();
        #endregion

        #region public methods
        /// <summary>
        /// Refreshes the text.
        /// </summary>
        public void Refresh()
        {
            textComponent.text = transRgx.Replace(originalContent,
                match => {
                    int count;
                    var translationKey = match.Groups[1].Value;
                    var countKey = match.Groups[2].Value;
                    if (translationKey.Contains(":")) foreach (var index in data.Keys) translationKey = translationKey.Replace(":" + index, data[index].Value.ToLower());
                    return LocalizationManager.LocalizationService.Translate(translationKey, GetParameters(data), !string.IsNullOrEmpty(countKey) && data.ContainsKey(countKey) && int.TryParse(data[countKey].Value.Split('.')[0], out count) ? (int?)count : null);
                });
        }
        #endregion

        #region unity methods
        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake() {
			textComponent = GetComponent<Text>();
			originalContent = textComponent.text;
			foreach(var attribute in Attributes) {
				foreach(var translationEventBuilder in attribute.Events) translationEventBuilders.Add(translationEventBuilder);
				foreach(var attributeField in attribute.ToDictionary()) data[attributeField.Key] = attributeField.Value;
			}
		}

		/// <summary>
		/// Called when component enabled.
		/// </summary>
		private void OnEnable() {
			foreach(var translationEventBuilder in translationEventBuilders) translationEventBuilder.Subscribe(Refresh);
		}

		/// <summary>
		/// Called when component disabled.
		/// </summary>
		private void OnDisable() {
			foreach(var translationEventBuilder in translationEventBuilders) translationEventBuilder.Unsubscribe(Refresh);
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		private void Start() {
			Refresh();
		}
		#endregion

		#region private fields		
		/// <summary>
		/// Gets the parameters as string dictionary.
		/// </summary>
		/// <param name="properties">The properties source.</param>
		/// <returns>The parameter data ready to be used by the translation system.</returns>
		private static Dictionary<string, string> GetParameters(Dictionary<string, TranslationProperty> properties) {
			var result = new Dictionary<string, string>();
			foreach(var translationProperty in properties) {
				var value = translationProperty.Value.Value;
				if(translationProperty.Value.KeepName) result[translationProperty.Key] = value;
				foreach(var valueAlias in translationProperty.Value.Aliases) result[valueAlias] = value;
			}
			return result;
		}
		#endregion
	}
}
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LocalizationModule.Exceptions;
using UnityEngine;
using UtilityModule.SimpleJSON.Support;

namespace LocalizationModule.Services {
	/// <summary>
	/// Localization service.
	/// </summary>
	public class LocalizationService {
		#region accessors		
		/// <summary>
		/// Gets the localization path.
		/// </summary>
		/// <value>
		/// The localization path.
		/// </value>
		public string LocalizationPath { get; private set; }

		/// <summary>
		/// Gets the default local.
		/// </summary>
		/// <value>
		/// The default local.
		/// </value>
		public string DefaultLocal { get; private set; }

		/// <summary>
		/// Gets the forced local.
		/// </summary>
		/// <value>
		/// The forced local.
		/// </value>
		public string ForcedLocal { get; private set; }

		/// <summary>
		/// Gets the local.
		/// </summary>
		/// <value>
		/// The local.
		/// </value>
		public string Local { get { return selectedCultureInfo.Name; } }
		#endregion

		#region private fields
		/// <summary>
		/// The path to load.
		/// </summary>
		private string pathToLoad;

		/// <summary>
		/// The translations.
		/// </summary>
		private readonly JsonObject translations = new JsonObject();

		/// <summary>
		/// The selected culture information.
		/// </summary>
		private CultureInfo selectedCultureInfo;

		/// <summary>
		/// The selected language folder.
		/// </summary>
		private string languageFolder;
		#endregion

		#region private accessors
		/// <summary>
		/// Gets the path to load.
		/// </summary>
		/// <value>
		/// The path to load.
		/// </value>
		private string PathToLoad { get { return pathToLoad ?? SetPathToLoad(string.IsNullOrEmpty(ForcedLocal) ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(ForcedLocal)); } }
		#endregion

		#region constructors		

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizationService"/> class.
		/// </summary>
		/// <param name="localizationPath">The localization path.</param>
		/// <param name="defaultLocal">The default local.</param>
		/// <param name="forcedLocal">The forced local.</param>
		/// <param name="loadTranslations">if set to <c>true</c> [load translations].</param>
		/// <param name="loadingKey">The loading key.</param>
		/// <exception cref="LocalizationFolderException"></exception>
		public LocalizationService(string localizationPath, string defaultLocal = "en-US", string forcedLocal = null, bool loadTranslations = true, string loadingKey = null) {
			if (!Directory.Exists(localizationPath)) throw new LocalizationFolderException(localizationPath);
			LocalizationPath = localizationPath;
			DefaultLocal = defaultLocal;
			ForcedLocal = forcedLocal;
			if (loadTranslations) Load(loadingKey);
		}
		#endregion

		#region public methods
		/// <summary>
		/// Translates the specified key.
		/// </summary>
		/// <param name="key">The translation key.</param>
		/// <param name="parameters">The parameters.</param>
		/// /// <param name="count">The pluralization count.</param>
		/// <returns>The translated content matching the translation key.</returns>
		public string Translate(string key, Dictionary<string, string> parameters = null, int? count = null) {
			JsonNode result = translations;
			foreach (var part in key.Split('.')) result = JsonNode.Cast(result, part);

			if (result == null) return key;

			var translation = result.ToString().Trim(' ', '\t', '\n', '\v', '\f', '\r', '"');

			if (count != null) {
				var pluralization = translation.Split('|');
				if (count <= 1 && translation.Length > 0 && pluralization.Length > 0) translation = pluralization[0];
				else if (count > 1 && translation.Length > 1 && pluralization.Length > 1) translation = pluralization[1];

				var inferiorRgx = new Regex(@"^\[\*,(\d+)\]", RegexOptions.IgnoreCase);
				var superiorRgx = new Regex(@"^\[(\d+),\*\]", RegexOptions.IgnoreCase);
				var equalRgx = new Regex(@"^{(\d+)}", RegexOptions.IgnoreCase);
				var betweenRgx = new Regex(@"^\[(\d+),(\d+)\]", RegexOptions.IgnoreCase);

				foreach (var testString in pluralization) {
					if (inferiorRgx.IsMatch(testString) && count <= int.Parse(inferiorRgx.Matches(testString)[0].Groups[1].ToString())) translation = inferiorRgx.Replace(testString, "");
					if (superiorRgx.IsMatch(testString) && count >= int.Parse(superiorRgx.Matches(testString)[0].Groups[1].ToString())) translation = superiorRgx.Replace(testString, "");
					if (equalRgx.IsMatch(testString) && count == int.Parse(equalRgx.Matches(testString)[0].Groups[1].ToString())) translation = equalRgx.Replace(testString, "");
					if (!betweenRgx.IsMatch(testString)) continue;
					var between = betweenRgx.Matches(testString)[0];
					if (count >= int.Parse(between.Groups[1].ToString()) && count <= int.Parse(between.Groups[2].ToString())) translation = betweenRgx.Replace(testString, "");
				}
			}

			if (parameters == null) return translation;

			foreach (var index in parameters.Keys) {
				translation = translation.Replace(":" + index.ToLower(), parameters[index].ToLower());
				translation = translation.Replace(":" + char.ToUpper(index[0]) + index.Substring(1).ToLower(), char.ToUpper(parameters[index][0]) + parameters[index].Substring(1).ToLower());
				translation = translation.Replace(":" + index.ToUpper(), parameters[index].ToUpper());
			}

			Debug.Log(translation);

			return translation;
		}

		/// <summary>
		/// Changes the local.
		/// </summary>
		/// <param name="cultureInfo">The culture information.</param>
		/// <param name="loadTranslations">if set to <c>true</c> [load translations].</param>
		/// <param name="loadingKey">The loading key.</param>
		public void SetLocale(CultureInfo cultureInfo, bool loadTranslations = false, string loadingKey = "") {
			SetPathToLoad(cultureInfo);
			if (loadTranslations) Load(loadingKey);
			else TranslateLoadedContent();
		}


		/// <summary>
		/// Loads the translations.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="currentNode">The current node.</param>
		public void Load(string path = "", JsonObject currentNode = null) {
			var parentNode = translations;
			if (currentNode == null) currentNode = translations;
			if (path != "") {
				foreach (var name in path.Split('.')) {
					parentNode = currentNode;
					currentNode = (currentNode[name] = new JsonObject()).AsObject;
				}
			}

			var directoryFullPath = Path.Combine(PathToLoad, path.Replace('.', Path.DirectorySeparatorChar));
			var fileFullPath = directoryFullPath + ".json";

			if (Directory.Exists(directoryFullPath)) {
				foreach (var filePath in Directory.GetFiles(directoryFullPath, "*.json")) currentNode[Path.GetFileNameWithoutExtension(filePath)] = Json.Parse(File.ReadAllText(filePath));
				foreach (var filePath in Directory.GetDirectories(directoryFullPath)) Load(Path.GetFileName(filePath), currentNode);
			}
			else if (File.Exists(fileFullPath)) parentNode[Path.GetFileNameWithoutExtension(fileFullPath)] = Json.Parse(File.ReadAllText(fileFullPath));
		}

		/// <summary>
		/// Unloads the translation at the specified path.
		/// </summary>
		/// <param name="path">The path of the translations node to remove.</param>
		public void Unload(string path = "") {
			JsonObject lastNode = null;
			var currentNode = translations;
			var pathArray = path.Split('.');
			if (path == "") {
				for (var i = 0; i < translations.Children.Count(); i++) { translations.Remove(i); }
				return;
			}
			foreach (var name in pathArray) {
				if (currentNode == null) return;
				lastNode = currentNode;
				currentNode = currentNode[name] as JsonObject;
			}
			if (lastNode != null) lastNode.Remove(pathArray.Last());
		}
		#endregion

		#region private methods
		/// <summary>
		/// Sets the path to load.
		/// </summary>
		/// <param name="cultureInfo">The culture information.</param>
		/// <returns>The language folder path.</returns>
		/// <exception cref="LanguageFolderException"></exception>
		private string SetPathToLoad(CultureInfo cultureInfo) {
			if (Directory.Exists(Path.Combine(LocalizationPath, cultureInfo.Name))) pathToLoad = SelectLanguageFolder(cultureInfo, true);
			else if (Directory.Exists(Path.Combine(LocalizationPath, cultureInfo.TwoLetterISOLanguageName))) pathToLoad = SelectLanguageFolder(cultureInfo, false);
			else if (Directory.Exists(Path.Combine(LocalizationPath, CultureInfo.GetCultureInfo(DefaultLocal).Name))) pathToLoad = SelectLanguageFolder(CultureInfo.GetCultureInfo(DefaultLocal), true);
			else if (Directory.Exists(Path.Combine(LocalizationPath, CultureInfo.GetCultureInfo(DefaultLocal).TwoLetterISOLanguageName))) pathToLoad = SelectLanguageFolder(CultureInfo.GetCultureInfo(DefaultLocal), false);
			else throw new LanguageFolderException(CultureInfo.CurrentCulture, CultureInfo.GetCultureInfo(DefaultLocal));
			return pathToLoad;
		}

		/// <summary>
		/// Selects the language folder.
		/// </summary>
		/// <param name="cultureInfo">The culture information.</param>
		/// <param name="useRegion">if set to <c>true</c> [use region].</param>
		/// <returns>The path of the selected language folder.</returns>
		private string SelectLanguageFolder(CultureInfo cultureInfo, bool useRegion) {
			selectedCultureInfo = cultureInfo;
			languageFolder = useRegion ? selectedCultureInfo.Name : selectedCultureInfo.TwoLetterISOLanguageName;
			return Path.Combine(LocalizationPath, languageFolder);
		}

		/// <summary>
		/// Translates the content of the loaded.
		/// </summary>
		/// <param name="currentNode">The current node.</param>
		/// <param name="path">The path.</param>
		private void TranslateLoadedContent(JsonObject currentNode = null, string path = "") {
			if (currentNode == null) currentNode = translations;
			foreach (var key in currentNode.Keys) {
				var relativePath = Path.Combine(path, key);
				var fullPath = Path.Combine(PathToLoad, relativePath);
				if (!File.Exists(fullPath + ".json")) TranslateLoadedContent(currentNode[key] as JsonObject, relativePath);
				else SwitchTranslations(Json.Parse(File.ReadAllText(fullPath + ".json")), currentNode[key] as JsonObject);
			}
		}

		/// <summary>
		/// Switches the translations.
		/// </summary>
		/// <param name="newTranslation">The new translation.</param>
		/// <param name="loadedTranslation">The loaded translation.</param>
		private void SwitchTranslations(JsonNode newTranslation, JsonObject loadedTranslation = null) {
			if (loadedTranslation == null) loadedTranslation = translations;
			for (var i = 0; i < loadedTranslation.Keys.Count; i++) {
				var key = loadedTranslation.Keys.ElementAt(i);
				var oldTranslation = loadedTranslation[key];
				if (oldTranslation.IsArray || oldTranslation.IsObject) SwitchTranslations(newTranslation[key], loadedTranslation[key] as JsonObject);
				else loadedTranslation[key] = newTranslation[key];
			}
		}
		#endregion
	}
}
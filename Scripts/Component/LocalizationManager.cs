using System.Globalization;
using System.IO;
using LocalizationModule.Services;
using UnityEngine;
using UtilityModule.Manager.Contracts;

namespace LocalizationModule.Component {
	/// <inheritdoc />
	/// <summary>
	/// Localization manager.
	/// </summary>
	public class LocalizationManager: ManagerBase {
		#region public properties		
		/// <summary>
		/// Gets the localization service.
		/// </summary>
		/// <value>
		/// The localization service.
		/// </value>
		public static LocalizationService LocalizationService { get; private set; }
		#endregion

		#region private fields
		/// <summary>
		/// The localization path.
		/// </summary>
		[SerializeField] private string localizationPath = "Data/Localization";

		/// <summary>
		/// The default local.
		/// </summary>
		[SerializeField] private string defaultLocal = "en-US";

		/// <summary>
		/// The forced local.
		/// </summary>
		[SerializeField] private string forceLocal;

		/// <summary>
		/// Loads the translation when the localization manager awakes if true.
		/// </summary>
		[SerializeField] private bool loadTranslations = true;

		/// <summary>
		/// The initial loading key.
		/// </summary>
		[SerializeField] private string loadingKey;
		#endregion

		#region unity methods
		/// <summary>
		/// Awakes this instance.
		/// </summary>
		private void Awake() {
			// todo: loading state
			if (LocalizationService == null) LocalizationService = new LocalizationService(Path.Combine(Application.dataPath, localizationPath), defaultLocal, forceLocal, loadTranslations, loadingKey);
			Ready = true;
		}
		#endregion
	}
}
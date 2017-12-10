using System;
using System.Globalization;

namespace LocalizationModule.Exceptions {
	/// <summary>
	/// Language folder exception.
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class LanguageFolderException: Exception {
		#region public accessors
		/// <summary>
		/// Gets the default culture.
		/// </summary>
		/// <value>
		/// The default culture.
		/// </value>
		public CultureInfo DefaultCulture { get; private set; }

		/// <summary>
		/// Gets the system culture.
		/// </summary>
		/// <value>
		/// The system culture.
		/// </value>
		public CultureInfo SystemCulture { get; private set; }

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public override string Message { get { return message; } }
		#endregion

		#region private fields
		/// <summary>
		/// The message.
		/// </summary>
		private readonly string message;
		#endregion

		#region constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="LanguageFolderException"/> class.
		/// </summary>
		/// <param name="defaultCulture">The default culture.</param>
		/// <param name="systemCulture">The system culture.</param>
		/// <param name="message"></param>
		public LanguageFolderException(CultureInfo defaultCulture, CultureInfo systemCulture, string message = null) {
			this.message = message ?? "The localization folder does not contains folder for the default language: \"" + defaultCulture + "\" neither for the system language: \"" + systemCulture + "\".";
			DefaultCulture = defaultCulture;
			SystemCulture = systemCulture;
		}
		#endregion
	}
}
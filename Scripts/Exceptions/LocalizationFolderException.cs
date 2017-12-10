using System;

namespace LocalizationModule.Exceptions {
	/// <summary>
	/// Localization folder exception.
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class LocalizationFolderException: Exception {
		#region public accessors
		/// <summary>
		/// Gets the localization path.
		/// </summary>
		/// <value>
		/// The localization path.
		/// </value>
		public string LocalizationPath { get; private set; }

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
		/// Initializes a new instance of the <see cref="LocalizationFolderException"/> class.
		/// </summary>
		/// <param name="localizationPath"></param>
		/// <param name="message"></param>
		public LocalizationFolderException(string localizationPath, string message = null) {
			this.message = message ?? "The localization folder can not be found using the path: \"" + localizationPath + "\" from the project assets root.";
			LocalizationPath = localizationPath;
		}
		#endregion
	}
}
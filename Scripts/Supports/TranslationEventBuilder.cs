using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LocalizationModule.Supports {
	/// <summary>
	/// Translation event builder.
	/// </summary>
	public class TranslationEventBuilder {
		#region private fields		
		/// <summary>
		/// The component.
		/// </summary>
		private readonly UnityEngine.Component component;

		/// <summary>
		/// The event information.
		/// </summary>
		private readonly EventInfo eventInfo;
		#endregion

		#region public constructors		
		/// <summary>
		/// Initializes a new instance of the <see cref="TranslationEventBuilder"/> class.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <param name="eventInfo">The event information.</param>
		public TranslationEventBuilder(UnityEngine.Component component, EventInfo eventInfo) {
			this.component = component;
			this.eventInfo = eventInfo;
		}
		#endregion

		#region public methods		
		/// <summary>
		/// Subscribes the specified action.
		/// </summary>
		/// <param name="action">The action to subscribe.</param>
		public void Subscribe(Action action) {
			eventInfo.AddEventHandler(component, Expression.Lambda(eventInfo.EventHandlerType, Expression.Call(Expression.Constant(action), "Invoke", Type.EmptyTypes)).Compile());
		}

		/// <summary>
		/// Unsubscribes the specified action.
		/// </summary>
		/// <param name="action">The action to unsubscribe.</param>
		public void Unsubscribe(Action action) {
			eventInfo.RemoveEventHandler(component, Expression.Lambda(eventInfo.EventHandlerType, Expression.Call(Expression.Constant(action), "Invoke", Type.EmptyTypes)).Compile());
		}
		#endregion
	}
}
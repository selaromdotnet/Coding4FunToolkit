﻿using System;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml;
#elif WINDOWS_PHONE
using System.Windows;
#endif

namespace Coding4Fun.Toolkit.Controls
{
    public abstract class UserPrompt : ActionPopUp<string, PopUpResult>
    {
        readonly RoundButton _cancelButton;
        protected internal Action MessageChanged;

        protected UserPrompt()
        {
            var okButton = new RoundButton();
	        _cancelButton = new RoundButton();

            okButton.Click += OkClick;
            _cancelButton.Click += CancelledClick;
			
            ActionPopUpButtons.Add(okButton);
            ActionPopUpButtons.Add(_cancelButton);

            SetCancelButtonVisibility(IsCancelVisible);
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
		{
			_cancelButton.Content = ButtonBaseHelper.CreateXamlCancel(_cancelButton);

			base.OnApplyTemplate();
		}

		#region Control Events
		private void OkClick(object sender, RoutedEventArgs e)
		{
			OnCompleted(new PopUpEventArgs<string, PopUpResult> { Result = Value, PopUpResult = PopUpResult.Ok });
		}

		private void CancelledClick(object sender, RoutedEventArgs e)
		{
			OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.Cancelled });
		}
		#endregion

		#region helper methods
		private void SetCancelButtonVisibility(bool value)
		{
			_cancelButton.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
		}
		#endregion

		#region Dependency Property Callbacks
		private static void OnMesageContentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = ((UserPrompt)o);

			if (sender != null && e.NewValue != e.OldValue && sender.MessageChanged != null)
				sender.MessageChanged();
		}

		private static void OnCancelButtonVisibilityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = ((UserPrompt)o);

			if (sender != null && e.NewValue != e.OldValue)
				sender.SetCancelButtonVisibility((bool)e.NewValue);
		}
		#endregion

		#region Dependency Properties / Properties
		public bool IsCancelVisible
		{
			get { return (bool)GetValue(IsCancelVisibileProperty); }
			set { SetValue(IsCancelVisibileProperty, value); }
		}

		// Using a DependencyProperty as the backing store for IsCancelVisible.  This enables animation, styling, binding, etc...
		public readonly DependencyProperty IsCancelVisibileProperty =
			DependencyProperty.Register("IsCancelVisible", typeof(bool), typeof(UserPrompt), new PropertyMetadata(false, OnCancelButtonVisibilityChanged));

		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(string), typeof(UserPrompt), new PropertyMetadata(""));

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(UserPrompt), new PropertyMetadata(""));

		public string Message
		{
			get { return (string)GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MessageProperty =
			DependencyProperty.Register("Message", typeof(string), typeof(UserPrompt), new PropertyMetadata("", OnMesageContentChanged));

		#endregion
	}
}

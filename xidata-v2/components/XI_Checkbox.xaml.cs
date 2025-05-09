using System;
using System.Windows;
using System.Windows.Controls;

namespace xidata_v2.components
{
    /// <summary>
    /// Interaction logic for XI_Checkbox.xaml
    /// </summary>
    public partial class XI_Checkbox : UserControl
    {
		/// <summary>
		/// Dependency: The list of selectable items to populate into the combo box
		/// </summary>
		public static readonly DependencyProperty PropText = DependencyProperty.Register(
			"DepText",
			typeof(string),
			typeof(XI_Checkbox),
			new FrameworkPropertyMetadata(null)
		);

		public static readonly DependencyProperty PropSubtext = DependencyProperty.Register(
			"DepSubtext",
			typeof(string),
			typeof(XI_Checkbox),
			new FrameworkPropertyMetadata(null)
		);

		public static readonly DependencyProperty PropChecked = DependencyProperty.Register(
			"DepChecked",
			typeof(string),
			typeof(XI_Checkbox),
			new FrameworkPropertyMetadata(null)
		);

		public event RoutedEventHandler Click;

		public string DepText
		{
			get { return (string)GetValue(PropText); }
			set { SetValue(PropText, value); }
		}

		public string DepSubtext
		{
			get { return (string)GetValue(PropSubtext); }
			set { SetValue(PropSubtext, value); }
		}

		public string DepChecked
		{
			get { return (string)GetValue(PropChecked); }
			set { SetValue(PropChecked, value); }
		}

		public XI_Checkbox()
		{
			InitializeComponent();

			Loaded += XI_Checkbox_Loaded;
		}

		public bool IsChecked()
		{
			return Convert.ToBoolean(InputCheckBox.IsChecked);
		}

		// SetCheckedManual basically prvents the "Checkbox Changed" event from firing back up to the parent UserControl
		// since we may want to set the default state in code AFTER rendering.
		private bool SetCheckedManual = false;
		public void SetChecked(bool state)
		{
			SetCheckedManual = true;
			InputCheckBox.IsChecked = state;
			DepChecked = state.ToString();
			SetCheckedManual = false;
		}

		public CheckBox Get()
		{
			return InputCheckBox;
		}

		private void XI_Checkbox_Loaded(object sender, RoutedEventArgs e)
		{
			InputCheckBox.Content = DepText;
			InputCheckBox.IsChecked = DepChecked == "True";
			InputSubtext.Text = DepSubtext;

			// Check and Uncheck!
			InputCheckBox.Checked += InputCheckBox_Changed;
			InputCheckBox.Unchecked += InputCheckBox_Changed;
		}

		private void InputCheckBox_Changed(object sender, RoutedEventArgs e)
		{
			if (SetCheckedManual)
			{
				return;
			}

			Click?.Invoke(this, e);
		}

		public void SetEnabled(bool enabled)
		{
			InputCheckBox.IsEnabled = enabled;
			InputCheckBox.Opacity = enabled ? 1 : 0.3;
		}
	}
}

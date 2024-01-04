using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace xidata_v2.components
{
	/// <summary>
	/// Interaction logic for XI_Input.xaml
	/// </summary>
	public partial class XI_Input : UserControl
	{
		public event RoutedEventHandler TextChanged;

		public static readonly DependencyProperty PropText = DependencyProperty.Register(
			"DepText",
			typeof(string),
			typeof(XI_Input),
			new FrameworkPropertyMetadata(null)
		);

		public static readonly DependencyProperty PropFontSize = DependencyProperty.Register(
			"DepFontSize",
			typeof(int),
			typeof(XI_Input),
			new FrameworkPropertyMetadata(null)
		);

		public string DepText
		{
			get { return (string)GetValue(PropText); }
			set { SetValue(PropText, value); }
		}

		public int DepFontSize
		{
			get { return (int)GetValue(PropFontSize); }
			set { SetValue(PropFontSize, value); }
		}

		public XI_Input()
		{
			InitializeComponent();

			Loaded += XI_TextBox_Loaded;
			InputTextBox.TextChanged += InputTextBox_TextChanged;
		}

		private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (SetTextManual)
			{
				return;
			}

			TextChanged?.Invoke(this, e);
		}

		public TextBox Get()
		{
			return InputTextBox;
		}

		public int GetNumber()
		{
			string numbers = Regex.Replace(InputTextBox.Text, "[^0-9]", "");
			_ = int.TryParse(numbers, out int result);

			return result;
		}

		private void XI_TextBox_Loaded(object sender, RoutedEventArgs e)
		{
			InputTextBox.Text = DepText;
			InputTextBox.FontSize = DepFontSize > 5 ? DepFontSize : 18;
		}

		/// <summary>
		/// Set the selected input text
		/// </summary>
		/// <param name="text"></param>
		private bool SetTextManual = false;
		public void SetText(string text)
		{
			SetTextManual = true;
			DepText = text;
			InputTextBox.Text = text;
			SetTextManual = false;
		}

		public void Clear()
		{
			InputTextBox.Clear();
		}

		public void Disable()
		{
			InputTextBox.IsEnabled = false;
			InputTextBox.Opacity = 0.5;
		}

		public void Enable()
		{
			InputTextBox.IsEnabled = true;
			InputTextBox.Opacity = 1;
		}
	}
}

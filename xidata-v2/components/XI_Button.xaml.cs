using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace xidata_v2.components
{
    /// <summary>
    /// Interaction logic for XI_Button.xaml
    /// </summary>
    public partial class XI_Button : UserControl
    {
		#region props

		public static readonly DependencyProperty PropContent = DependencyProperty.Register(
			"DepContent",
			typeof(object),
			typeof(XI_Button),
			new FrameworkPropertyMetadata(null)
		);

		public static readonly DependencyProperty PropFontSize = DependencyProperty.Register(
			"DepFontSize",
			typeof(string),
			typeof(XI_Button),
			new FrameworkPropertyMetadata(null)
		);

		public static readonly DependencyProperty PropType = DependencyProperty.Register(
			"DepType",
			typeof(string),
			typeof(XI_Button),
			new FrameworkPropertyMetadata(null)
		);

		public object DepContent
		{
			get { return (object)GetValue(PropContent); }
			set { SetValue(PropContent, value); }
		}

		public string DepFontSize
		{
			get { return (string)GetValue(PropFontSize); }
			set { SetValue(PropFontSize, value); }
		}

		#endregion

		public event RoutedEventHandler Click;

		public XI_Button()
		{
			InitializeComponent();

			Loaded += XI_Button_Loaded;
		}

		public void SetRenderingDisplay()
		{
			SetValue(TextOptions.TextFormattingModeProperty, TextFormattingMode.Display);
		}

		public void Disable()
		{
			Button.IsEnabled = false;
			Button.Opacity = 0.5;
		}

		public void Enable()
		{
			Button.IsEnabled = true;
			Button.Opacity = 1;
		}

		private void XI_Button_Loaded(object sender, RoutedEventArgs e)
		{
			int fontSize = Convert.ToInt32(DepFontSize);

			Button.Click += Button_Click;

			// text
			Button.FontSize = fontSize > 2 ? fontSize : 15;
			Button.Content = DepContent.ToString();
		}

		private void Button_Click(object sender, RoutedEventArgs e) => Click?.Invoke(this, e);
	}
}

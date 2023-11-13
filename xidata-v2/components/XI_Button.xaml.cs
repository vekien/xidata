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

		public static readonly DependencyProperty PropIsBold = DependencyProperty.Register(
			"DepIsBold",
			typeof(bool),
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

		public string DepType
		{
			get { return (string)GetValue(PropType); }
			set { SetValue(PropType, value); }
		}

		public bool DepIsBold
		{
			get { return (bool)GetValue(PropIsBold); }
			set { SetValue(PropIsBold, value); }
		}

		#endregion

		private SolidColorBrush BackgroundDefaultColor;
		private SolidColorBrush BackgroundHoverColor;
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
			Button.Opacity = 0.8;
			DepType = "disabled";
			SetColour(DepType);
		}

		public void Enable(string NewDeptType)
		{
			Button.IsEnabled = true;
			Button.Opacity = 1;
			DepType = NewDeptType;
			SetColour(DepType);
		}

		private void XI_Button_Loaded(object sender, RoutedEventArgs e)
		{
			int fontSize = Convert.ToInt32(DepFontSize);

			Button.Click += Button_Click;

			// text
			ButtonText .FontSize = fontSize > 2 ? fontSize : 15;
			ButtonText.Text = DepContent.ToString();
			ButtonText.FontWeight = DepIsBold ? FontWeights.Bold : FontWeights.Normal;
			ButtonText.FontFamily = new FontFamily(DepIsBold ? "Open Sans" : "Open Sans Medium");

			SetColour(DepType);
		}

		private void SetColour(string buttonType)
		{
			string ColourDefault;
			string ColourHover;

			//
			// Setup Button Styles
			// 

			switch (buttonType)
			{
				default:
				case "basic":
					ColourDefault = "#30343f";
					ColourHover = "#1D2026";
					break;

				case "red":
					ColourDefault = "#e03616";
					ColourHover = "#B52C12";
					break;

				case "blue":
					ColourDefault = "#1288CC";
					ColourHover = "#0570AD";
					break;

				case "green":
					ColourDefault = "#178A49";
					ColourHover = "#0F753C";
					break;

				case "orange":
					ColourDefault = "#BD8400";
					ColourHover = "#A37200";
					break;
			}

			BackgroundDefaultColor = new((Color)ColorConverter.ConvertFromString(ColourDefault));
			BackgroundHoverColor = new((Color)ColorConverter.ConvertFromString(ColourHover));
			Button.Background = BackgroundDefaultColor;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Click?.Invoke(this, e);
		}

		private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			Button.Background = BackgroundHoverColor;
		}

		private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			Button.Background = BackgroundDefaultColor;
		}
	}
}

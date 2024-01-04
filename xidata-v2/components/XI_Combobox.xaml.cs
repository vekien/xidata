using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace xidata_v2.components
{
    /// <summary>
    /// Interaction logic for XI_Combobox.xaml
    /// </summary>
    public partial class XI_Combobox : UserControl
    {
		public event RoutedEventHandler SelectionChanged;

		/// <summary>
		/// Dependency: The list of selectable items to populate into the combo box
		/// </summary>
		public static readonly DependencyProperty PropSelectItems = DependencyProperty.Register(
			"DepSelectItems",
			typeof(string),
			typeof(XI_Combobox),
			new FrameworkPropertyMetadata(null)
		);

		public static readonly DependencyProperty PropFontSize = DependencyProperty.Register(
			"DepFontSize",
			typeof(int),
			typeof(XI_Combobox),
			new FrameworkPropertyMetadata(null)
		);

		public int DepFontSize
		{
			get { return (int)GetValue(PropFontSize); }
			set { SetValue(PropFontSize, value); }
		}

		public string DepSelectItems
		{
			get { return (string)GetValue(PropSelectItems); }
			set { SetValue(PropSelectItems, value); }
		}


		public XI_Combobox()
        {
            InitializeComponent();

			Loaded += XI_Combobox_Loaded;
        }

		public ComboBox Get()
		{
			return InputComboBox;
		}

		private void XI_Combobox_Loaded(object sender, RoutedEventArgs e)
		{
			InputComboBox.ItemsSource = DepSelectItems.Split(',');
			InputComboBox.SelectionChanged += InputComboBox_SelectionChanged;
			InputComboBox.FontSize = DepFontSize > 5 ? DepFontSize : 18;
		}

		public void SetItems(string[] items)
		{
			SetOptionManual = true;
			DepSelectItems = string.Join(',', items);
			InputComboBox.ItemsSource = items;
			SetOptionManual = false;
		}
		
		private void InputComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            if (SetOptionManual)
            {
				return;
            }

            SelectionChanged?.Invoke(this, e);
		}

		private bool SetOptionManual = false;
		public void SetSelectedOption(int index)
		{
			SetOptionManual = true;
			InputComboBox.SelectedIndex = index;
			SetOptionManual = false;
		}

		public void SetSelectedOption(string value)
		{
			SetOptionManual = true;
			object selectedItem = InputComboBox.Items.Cast<object>().FirstOrDefault(item => item.ToString() == value);

			InputComboBox.SelectedItem = selectedItem;
			SetOptionManual = false;
		}
	}
}

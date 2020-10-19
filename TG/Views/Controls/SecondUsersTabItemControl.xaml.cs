using System.Windows.Controls;
using System.Windows.Data;
using TG.Models.Company;

namespace TG.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для SecondUsersTabItemControl.xaml
    /// </summary>
    public partial class SecondUsersTabItemControl : UserControl
    {
        public SecondUsersTabItemControl()
        {
            InitializeComponent();
        }

        private void DepartmentCollection_OnFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Department department)) return;
            if (department.Name is null) return;

            var filterText = DepartmentNameFilterText.Text;
            if (string.IsNullOrEmpty(filterText)) return;

            if (department.Name.Contains(filterText, System.StringComparison.OrdinalIgnoreCase)) return;
            if (department.Description != null && department.Description.Contains(filterText, System.StringComparison.OrdinalIgnoreCase)) return;

            e.Accepted = false;
        }

        private void OnDepartmentFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var collection = (CollectionViewSource)textBox.FindResource("DepartmentsCollection");
            collection.View.Refresh();
        }
    }
}

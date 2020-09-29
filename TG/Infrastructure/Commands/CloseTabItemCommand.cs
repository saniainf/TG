using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TG.Infrastructure.Commands.Base;

namespace TG.Infrastructure.Commands
{
    class CloseTabItemCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            TabControl panel = (TabControl)parameter;
            panel.Items.Remove(panel.SelectedItem);
        }
    }
}

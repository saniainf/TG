using System.Windows;
using TG.Infrastructure.Commands.Base;

namespace TG.Infrastructure.Commands
{
    internal class CloseAplicationCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}

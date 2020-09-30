using System;
using System.Windows;
using System.Windows.Input;
using TG.Infrastructure.Commands;
using TG.ViewModels.Base;

namespace TG.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Свойства

        #region Title : Заголовок окна

        private string title = "Работа Типографии";

        /// <summary> Заголовок окна </summary>
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        #endregion

        #region Status : Статус программы

        private string status = "Все готово";

        /// <summary> Статус программы </summary>
        public string Status
        {
            get => status;
            set => Set(ref status, value);
        }

        #endregion

        #region Progress : Прогресс выполнения

        private double progress;

        /// <summary> Прогресс выполнения </summary>
        public double Progress
        {
            get => progress;
            set => Set(ref progress, value);
        }

        #endregion

        #region SelectedPageIndex : Номер выбранной вкладки

        private double selectedPageIndex = 0;

        /// <summary> Номер выбранной вкладки </summary>
        public double SelectedPageIndex
        {
            get => selectedPageIndex;
            set => Set(ref selectedPageIndex, value);
        }

        #endregion

        #endregion

        #region Команды

        #region CloseAplicationCommand : Закрытие программы

        public ICommand CloseAplicationCommand { get; }

        private bool CanCloseAplicationCommandExecute(object p) => true;

        private void OnCloseAplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown(0);
        }

        #endregion

        #region ChangeTabIndexCommand : Переключение вкладок

        public ICommand ChangeTabIndexCommand { get; }

        private bool CanChangeTabIndexCommandExecute(object p) => selectedPageIndex >= 0;

        private void OnChangeTabIndexCommandExecuted(object p)
        {
            if (p is null) return;
            SelectedPageIndex += Convert.ToInt32(p);
        }

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            CloseAplicationCommand = new LambdaCommand(OnCloseAplicationCommandExecuted, CanCloseAplicationCommandExecute);

            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommandExecuted, CanChangeTabIndexCommandExecute);

            #endregion
        }
    }
}

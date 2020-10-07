using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using TG.Infrastructure.Commands;
using TG.Models.Company;
using TG.ViewModels.Base;

namespace TG.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {

        #region Свойства

        public object[] CompositeCollection { get; }

        public ObservableCollection<Department> Departments { get; }

        #region SelectedCompositeValue : Выбранное значение из CompositeCollection

        private object selectValue;

        /// <summary> Выбранный отдел </summary>
        public object SelectedCompositeValue
        {
            get => selectValue;
            set => Set(ref selectValue, value);
        }

        #endregion

        #region SelectedDepartment : Выбранный отдел

        private Department department;

        /// <summary> Выбранный отдел </summary>
        public Department SelectedDepartment
        {
            get => department;
            set => Set(ref department, value);
        }

        #endregion

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

            #region Тестовые данные

            var idx = 0;

            var users = Enumerable.Range(1, 15).Select(i => new User()
            {
                Name = "John",
                Patronymic = $"Johnovich",
                Surname = $"Smith_{idx++:d2}",
                Birthday = DateTime.Now,
                Rating = 1,
                Description = "Пользователь Ok"
            });
            var departments = Enumerable.Range(1, 10).Select(i => new Department()
            {
                Users = users.ToArray(),
                Name = $"Отдел №{i:d2}",
                Description = "Отдел Ok"
            });

            Departments = new ObservableCollection<Department>(departments);

            var dataList = new List<object>();

            dataList.Add("string literal");
            dataList.Add(42);
            dataList.Add(departments.First());
            dataList.Add(departments.First().Users.First());
            CompositeCollection = dataList.ToArray();

            #endregion
        }
    }
}

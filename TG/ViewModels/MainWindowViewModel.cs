using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Data;
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

        public DirectoryViewModel DiskRootDir { get; } = new DirectoryViewModel("c:\\");

        #region SelectedDirectory : Выбранная директория
        /// <summary> Выбранная директория </summary>
        private DirectoryViewModel _SelectedDirectory;
        /// <summary> Выбранная директория </summary>
        public DirectoryViewModel SelectedDirectory
        {
            get => _SelectedDirectory;
            set => Set(ref _SelectedDirectory, value);
        }
        #endregion

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
            set
            {
                if (!Set(ref department, value)) return;
                selectedDepartmentUsers.Source = value?.Users;
                OnPropertyChanged(nameof(SelectedDepartmentUsers));
            }
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

        #region MutableProperty : изменяемое свойство

        private bool mutableProperty;

        /// <summary> Номер выбранной вкладки </summary>
        public bool MutableProperty
        {
            get => mutableProperty;
            set => Set(ref mutableProperty, value);
        }

        #endregion

        #region GetUsers : большое количество пользователей
        public IEnumerable<User> GetUsers =>
            Enumerable.Range(1, App.IsDesignMode ? 10 : 100000)
            .Select(i => new User()
            {
                Name = $"Имя {i}",
                Surname = $"Фамилия {i}"
            });
        #endregion

        #region UserFilterText : текст фильтра пользователей
        /// <summary> текст фильтра пользователей </summary>
        private string _UserFilterText;
        /// <summary> текст фильтра пользователей </summary>
        public string UserFilterText
        {
            get => _UserFilterText;
            set
            {
                if (!Set(ref _UserFilterText, value)) return;
                selectedDepartmentUsers.View.Refresh();
            }
        }
        #endregion

        #region SelectedDepartmentUsers : фильтрация пользователей через модель представления

        private CollectionViewSource selectedDepartmentUsers = new CollectionViewSource();

        public ICollectionView SelectedDepartmentUsers => selectedDepartmentUsers?.View;

        private void OnUsersFiltred(object sender, FilterEventArgs e)
        {
            if (!(e.Item is User user))
            {
                e.Accepted = false;
                return;
            }

            var filterText = UserFilterText;
            if (string.IsNullOrWhiteSpace(filterText)) return;

            if (user.Name is null || user.Patronymic is null || user.Surname is null)
            {
                e.Accepted = false;
                return;
            }

            if (user.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase)) return;
            if (user.Surname.Contains(filterText, StringComparison.OrdinalIgnoreCase)) return;
            if (user.Patronymic.Contains(filterText, StringComparison.OrdinalIgnoreCase)) return;

            e.Accepted = false;
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

        #region CreateDepartmentCommand : Создать отдел

        public ICommand CreateDepartmentCommand { get; }

        private bool CanCreateDepartmentCommandExecute(object p) => true;

        private void OnCreateDepartmentCommandExecuted(object p)
        {
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

            var departmentIdx = Departments.Count + 1;
            var newDepartment = new Department
            {
                Users = users.ToArray(),
                Name = $"Отдел №{departmentIdx:d2}",
                Description = "Отдел Ok"
            };

            Departments.Add(newDepartment);
            SelectedDepartment = newDepartment;
        }

        #endregion

        #region DeleteDepartmentCommand : Удалить отдел

        public ICommand DeleteDepartmentCommand { get; }

        private bool CanDeleteDepartmentCommandExecute(object p) => p is Department department && Departments.Contains(department);

        private void OnDeleteDepartmentCommandExecuted(object p)
        {
            if (!(p is Department department)) return;
            var departmentIdx = Departments.IndexOf(department);
            Departments.Remove(department);
            if (departmentIdx < Departments.Count)
                SelectedDepartment = Departments[departmentIdx];
            else if (Departments.Count > 0)
                SelectedDepartment = Departments.Last();
        }

        #endregion

        #region ChangeMutableProperty : Изменение пробного свойства
        /// <summary> Изменение пробного свойства </summary>
        public ICommand ChangeMutablePropertyCommand { get; }
        private bool CanChangeMutablePropertyCommandExecute(object p) => true;
        private void OnChangeMutablePropertyCommandExecuted(object p)
        {
            MutableProperty = !MutableProperty;
        }
        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            CloseAplicationCommand = new LambdaCommand(OnCloseAplicationCommandExecuted, CanCloseAplicationCommandExecute);

            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommandExecuted, CanChangeTabIndexCommandExecute);

            CreateDepartmentCommand = new LambdaCommand(OnCreateDepartmentCommandExecuted, CanCreateDepartmentCommandExecute);

            DeleteDepartmentCommand = new LambdaCommand(OnDeleteDepartmentCommandExecuted, CanDeleteDepartmentCommandExecute);

            ChangeMutablePropertyCommand = new LambdaCommand(OnChangeMutablePropertyCommandExecuted, CanChangeMutablePropertyCommandExecute);

            #endregion

            #region Тестовые данные

            var idx = 0;

            var users = Enumerable.Range(1, App.IsDesignMode ? 15 : 100).Select(i => new User()
            {
                Name = "John",
                Patronymic = $"Johnovich",
                Surname = $"Smith_{idx++:d2}",
                Birthday = DateTime.Now,
                Rating = 1,
                Description = "Пользователь Ok"
            });
            var departments = Enumerable.Range(1, App.IsDesignMode ? 10 : 1000).Select(i => new Department()
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

            selectedDepartmentUsers.Filter += OnUsersFiltred;

            selectedDepartmentUsers.SortDescriptions.Add(new SortDescription("Surname", ListSortDirection.Descending));
        }
    }
}

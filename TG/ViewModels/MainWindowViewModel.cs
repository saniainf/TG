using TG.ViewModels.Base;

namespace TG.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Title : Заголовок окна

        private string _Title = "Работа Типографии";

        /// <summary> Заголовок окна </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region Status : Статус программы

        private string _Status = "Все готово";

        /// <summary> Статус программы </summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

        #endregion

        #region Progress : Прогресс выполнения

        private double _Progress;

        /// <summary> Прогресс выполнения </summary>
        public double Progress
        {
            get => _Progress;
            set => Set(ref _Progress, value);
        }

        #endregion
    }
}

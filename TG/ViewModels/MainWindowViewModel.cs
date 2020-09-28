using TG.ViewModels.Base;

namespace TG.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region заголовок окна

        private string _Title = "Работа Типографии";
        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion
    }
}

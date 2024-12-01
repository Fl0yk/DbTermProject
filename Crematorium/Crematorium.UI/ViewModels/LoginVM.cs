using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crematorium.Application.Abstractions;
using Crematorium.UI.Fabrics;
using Crematorium.UI.Pages;

namespace Crematorium.UI.ViewModels
{
    public partial class LoginVM : ObservableValidator
    {
        private IUserService _userService;
        public LoginVM(IUserService userService) 
        {
            _userService = userService;
        }

        [ObservableProperty]
        private string inputName = "";

        [ObservableProperty]
        private string inputNumPassport = "";

        //[RelayCommand]
        public bool LoginUser()
        {
            if (string.IsNullOrEmpty(InputName) ||
                string.IsNullOrEmpty(InputNumPassport))
            {
                var er = ServicesFabric.GetErrorPage("Что-то не заполнили");
                er.ShowDialog();
                return false;
            }

            bool validedUser = _userService.IsExist(InputName, InputNumPassport).Result;
            if (validedUser)
            {
                ServicesFabric.CurrentUser = _userService
                    .GetUserByNameAndPassport(InputName, InputNumPassport)
                    .Result;
                return true;
            }
            else
            {
                var er = ServicesFabric.GetErrorPage("Такого пользователя нет");
                er.ShowDialog();
                ServicesFabric.CurrentUser = null;
                return false;
            }
        }

        public void ClearFields()
        {
            InputName = "";
            InputNumPassport = "";
        }

        [RelayCommand]
        public void RegistrationUser()
        {
            var userChange = (ChangeUserPage)ServicesFabric.GetPage(typeof(ChangeUserPage));
            userChange.InitializeUser(-1, UserChangeOperation.UserRegistration);
            userChange.OpBtnName.Text = "Registration";
            userChange.ShowDialog();
        }
    }
}

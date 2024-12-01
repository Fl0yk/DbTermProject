using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using Crematorium.UI.Pages;

namespace Crematorium.UI.ViewModels
{
    public partial class UserAccountVM : ObservableValidator
    {
        [ObservableProperty]
        private User curUser;

        public UserAccountVM()
        {
            CurUser = ServicesFabric.CurrentUser!;
            UpdateFields();
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string surname;

        [ObservableProperty]
        private string mailAdress;

        private void UpdateFields()
        {
            Name = CurUser.Name;
            Surname = CurUser.Surname;
            MailAdress = CurUser.MailAdress;
        }

        [RelayCommand]
        public void UpdateUser()
        {
            var userChange = (ChangeUserPage)ServicesFabric.GetPage(typeof(ChangeUserPage));
            userChange.InitializeUser( CurUser.Id, UserChangeOperation.UserUpdate);
            userChange.OpBtnName.Text = "Update";
            userChange.ShowDialog();
            UpdateFields();
        }
    }
}

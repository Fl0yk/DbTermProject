using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crematorium.Application.Abstractions.Services;
using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using Crematorium.UI.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Crematorium.UI.ViewModels
{
    public partial class UsersVM : ObservableValidator
    {
        private IUserService _userService;

        public UsersVM(IUserService userService)
        {
            _userService = userService;
            Users = new ObservableCollection<User>( _userService.GetAllAsync().Result);
            
        }

        [ObservableProperty]
        [MaxLength(20)]
        private string inputFindName;

        public ObservableCollection<User> Users { get; set; }

        [RelayCommand]
        public void FindUsers()
        {
            Users.Clear();
            if (string.IsNullOrEmpty(InputFindName) || string.IsNullOrWhiteSpace(InputFindName))
            {
                UpdateUsersCollection();
                return;
            }

            foreach (User user in _userService.FindByNameAsync(InputFindName).Result) 
            {
                Users.Add(user);
            }
        }

        [RelayCommand]
        public void AddUser()
        {
            var userChange = (ChangeUserPage)ServicesFabric.GetPage(typeof(ChangeUserPage));
            userChange.InitializeUser(-1, UserChangeOperation.AdminAdd);
            userChange.OpBtnName.Text = "Registration";
            userChange.ShowDialog();
            UpdateUsersCollection();
        }

        [ObservableProperty]
        private User selectedUser;

        [RelayCommand]
        public void UpdateUser()
        {
            if (SelectedUser is null)
                return;

            var userChange = (ChangeUserPage)ServicesFabric.GetPage(typeof(ChangeUserPage));
            userChange.InitializeUser(SelectedUser.Id, UserChangeOperation.AdminUpdate);
            userChange.OpBtnName.Text = "Update";
            userChange.ShowDialog();
            UpdateUsersCollection();
        }

        [RelayCommand]
        public void DeleteUser()
        {
            if (SelectedUser is null || SelectedUser.Name == "Admin")
                return;

            _userService.DeleteAsync(SelectedUser.Id);
            UpdateUsersCollection();
        }

        private void UpdateUsersCollection()
        {
            Users.Clear();
            foreach (User user in _userService.GetAllAsync().Result)
            {
                Users.Add(user);
            }
        }
    }
}

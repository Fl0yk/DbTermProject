using CommunityToolkit.Mvvm.ComponentModel;
using Crematorium.Application.Abstractions;
using Crematorium.Application.Validators;
using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Crematorium.UI.ViewModels
{
    public partial class UserChangeVM : ObservableValidator
    {
        private IUserService _userService;
        private bool _isNewUser;
        public UserChangeVM(IUserService userService)
        {
            _userService = userService;
            SelectedRole = Role.NoName;
        }
        [ObservableProperty]
        private UserChangeOperation operation;

        [ObservableProperty]
        private bool isRegistration;

        [ObservableProperty]
        private Role selectedRole;

        [ObservableProperty]
        [Required]
        private User user;

        public void SetUser(int userId, UserChangeOperation op)
        {
            //IsRegistration = isRegUser;
            Operation = op;
            User = _userService.GetByIdAsync(userId).Result;

            if (User is null)
            {
                User = new User();
                _isNewUser = true;
            }
            else
            {
                _isNewUser = false;
            }
            this.Name = User.Name;
            this.Surname = User.Surname;
            this.NumPassport = string.Empty;
            this.SelectedRole = User.UserRole;
            this.MailAdress = User.MailAdress;
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string surname;

        [ObservableProperty]
        private string numPassport;

        [ObservableProperty]
        private string repNumPassport;

        [ObservableProperty]
        private string mailAdress;

        public void ClearFields()
        {
            Name = string.Empty;
            Surname = string.Empty;
            NumPassport = string.Empty;
            MailAdress = string.Empty;
        }

        public async Task DoUserOperation()
        {
            if (User is null)
                throw new ArgumentNullException("User not initialized");

            UserValidator validations = new UserValidator();

            switch (Operation)
            {
                case UserChangeOperation.UserRegistration:
                    IsBusyNumber();
                    NumPassportCoincide();
                    User.NumPassport = this.NumPassport;
                    InitializeValue(true);
                    validations.ValidateAndThrow(User);
                    await _userService.AddAsync(User);
                    ServicesFabric.CurrentUser = this.User;
                    break;

                case UserChangeOperation.UserUpdate:
                    NumPassportMatch();
                    InitializeValue(true);
                    validations.ValidateAndThrow(User);
                    await _userService.UpdateAsync(User);
                    break;

                case UserChangeOperation.AdminAdd:
                    IsBusyNumber();
                    User.NumPassport = this.NumPassport;
                    InitializeValue(false);
                    validations.ValidateAndThrow(User);
                    await _userService.AddAsync(User);
                    break;

                case UserChangeOperation.AdminUpdate:
                    InitializeValue(false);
                    validations.ValidateAndThrow(User);
                    await _userService.UpdateAsync(User);
                    break;

                default:
                    throw new ArgumentException("Несуществующая операция");
            }
        }

        private void InitializeValue(bool isUserRegistration)
        {
            User.Name = this.Name;
            User.Surname = this.Surname;
            //User.NumPassport = this.NumPassport;
            User.MailAdress = this.MailAdress;
            if (isUserRegistration)
            {
                User.UserRole = Role.Customer;
            }
            else
            {
                User.UserRole = this.SelectedRole;
            }
        }

        /// <summary>
        /// Проверяет совпадение изначального номера паспорта с введеным для подтверждения изменений
        /// </summary>
        private bool NumPassportMatch()
        {
            if (ServicesFabric.CurrentUser!.NumPassport != NumPassport)
                throw new Exception("Введенный номер паспорта не совпадает с номером пользователя");

            return true;
        }

        /// <summary>
        /// Проверяет совпадение изначальный номер паспорта и повторный
        /// </summary>
        private bool NumPassportCoincide()
        {
            if (NumPassport != RepNumPassport)
                throw new Exception("Введенные номера паспорта не совпадают");

            return true;
        }

        private void IsBusyNumber()
        {
            bool isExist = _userService.IsExist(NumPassport).Result;

            if (isExist)
                throw new Exception("Данный номер паспорта уже зарегестрирован");

            return;
        }
    }

    public enum UserChangeOperation
    {
        UserRegistration,  //Вводятся все поля, валидация паспорта и почты, проверка на существование такого пользователя

        UserUpdate, //Заполнются полученными данными все поля, кроме паспорта.
                    //Валидация почты, проверка на совпадение паспорта, проверка на существование

        AdminAdd,   // Вводятся все данные, выбор роли, валидация почты и паспорта

        AdminUpdate // Не видно номера паспорта, валидация почты, выбор роли
    }

}

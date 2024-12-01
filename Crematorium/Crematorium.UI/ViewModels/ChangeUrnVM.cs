using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crematorium.Application.Abstractions;
using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using Microsoft.Win32;
using System;
using System.IO;

namespace Crematorium.UI.ViewModels
{
    public partial class ChangeUrnVM : ObservableValidator
    {
        private IRitualUrnService _urnService;

        private RitualUrn? changedUrn;
        private bool _isNewUrn = false;

        public ChangeUrnVM(IRitualUrnService userService)
        {
            _urnService = userService;
        }

        public void SetUrn(int id)
        {
            changedUrn = _urnService.GetByIdAsync(id).Result;

            if (changedUrn is null)
            {
                changedUrn = new RitualUrn();
                _isNewUrn = true;
            }
            else
            {
                _isNewUrn = false;
            }

            Name = changedUrn.Name;
            Price = changedUrn.Price;
            Image = changedUrn.Image;
        }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private decimal price = 0;

        [ObservableProperty]
        private string image = "";

        [RelayCommand]
        public void ChangeFoto()
        {
            if (changedUrn is null)
            {
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            if (openFileDialog.ShowDialog() == true)
            {

                //byte[] data;
                //using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                //{
                //    data = new byte[fs.Length];
                //    fs.Read(data, 0, data.Length);
                //}
                Image = openFileDialog.FileName;
                changedUrn.Image = openFileDialog.FileName;
            }
        }

        [RelayCommand]
        public void AddUrn()
        {
            if (changedUrn is null)
                throw new ArgumentNullException("Urn not initialized");

            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name) ||
                Image is null || Price == 0)
            {
                var er = ServicesFabric.GetErrorPage("Что-то не заполнили");
                er.ShowDialog();
                return;
            }

            changedUrn.Name = Name;
            changedUrn.Price = Price;
            changedUrn.Image = Image;

            if(_isNewUrn)
            {
                _urnService.CreateAsync(changedUrn);
            }
            else
            {
                _urnService.UpdateAsync(changedUrn);
            }
        }
    }
}

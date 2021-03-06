﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using Sibur.Models;
using Sibur.Views;
using Sibur.Requests;

namespace Sibur.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private bool isBusy;    // идет ли загрузка с сервера
        UserRequests db = new UserRequests();
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SaveUserCommand { get; set; }
        public ICommand BackCommand { protected set; get; }
        public INavigation Navigation { get; set; }
        public Registration View { get; set; }
        public RegistrationViewModel()
        {
            IsBusy = false;
            SaveUserCommand = new Command(SaveUser);
            BackCommand = new Command(Back);
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }
        public bool IsLoaded
        {
            get { return !isBusy; }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        private void Back()
        {
            Navigation.PopAsync();
        }
        private async void SaveUser(object userObject)
        {
            User usr = userObject as User;
            if (usr != null)
            {
                IsBusy = true;
                Globals.CurrentUser = await db.Add(usr);
                View.Success();
                Back();
            }
            else
            {
                View.Fail();
            }
            IsBusy = false;
        }
        //private async void SaveUser(object userObject)
        //{
        //    IsBusy = true;
        //    Globals.CurrentUser = await db.Entry("tt", "12345");
        //    View.Success();
        //    Back();
        //}
    }
}

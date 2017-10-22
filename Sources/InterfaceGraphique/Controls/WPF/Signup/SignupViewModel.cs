﻿using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF.Signup
{
    public class SignupViewModel : ViewModelBase
    {

        private SignupEntity signupEntity;
        private HubManager hubManager;
        private ChatHub chatHub;

        static HttpClient client = new HttpClient();

        public SignupViewModel(SignupEntity signupEntity, ChatHub chatHub)
        {
            this.chatHub = chatHub;
            this.signupEntity = signupEntity;
            this.hubManager = HubManager.Instance;
            this.inputsEnabled = true;
        }

        private ICommand signupCommand;
        public ICommand SignupCommand
        {
            get
            {
                if (signupCommand == null)
                {
                    signupCommand = new RelayCommandAsync(Signup);
                }
                return signupCommand;
            }
        }

        private async Task<Uri> Signup()
        {
            {
                try
                {
                    Loading();
                    ResetErrMsg();
                    if (!ValidateFields())
                    {
                        return null;
                    }
                    var response = await client.PostAsJsonAsync("http://localhost:63056/api/signup", signupEntity);
                    if (response.IsSuccessStatusCode)
                    {
                        hubManager.AddHubs();
                        await hubManager.InitializeHubs(signupEntity.Username);
                        await chatHub.InitializeChat();
                        Program.FormManager.CurrentForm = Program.MainMenu;
                    }
                    else
                    {
                        var res = response.Content.ReadAsAsync<string>().Result;
                        //response.EnsureSuccessStatusCode();
                        // return URI of the created resource.
                        UsernameErrMsg = res;
                        PasswordErrMsg = res;
                    }
                    return response.Headers.Location;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                    return null;
                }
                finally
                {
                    LoadingDone();
                }

            }

        }

        private bool ValidateFields()
        {
            bool valid = true;
            if (Username == "")
            {
                UsernameErrMsg = "Nom d'usager requis";
                valid = false;
            }
            else if (Username.Length < 2)
            {
                UsernameErrMsg = "Minimum 2 charactères requis";
                valid = false;
            }
            else
            {
                Regex rgx = new Regex(@"^[a-zA-Z0-9_.-]*$");
                if (!rgx.IsMatch(Username))
                {
                    UsernameErrMsg = "Seul les charactères alphanumériqus et les tirets sont acceptés";
                    valid = false;
                }
            }
            if(Password == "")
            {
                PasswordErrMsg = "Mot de passe requis";
                valid = false;
            }
            else if (Password.Length < 8)
            {
                PasswordErrMsg = "Minimum 8 charactères requis";
                valid = false;
            }
            else
            {
                Regex rgx = new Regex(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]");
                if (!rgx.IsMatch(Password))
                {
                    PasswordErrMsg = "Minimum une lettre minuscule, une lettre majuscule et un chiffre";
                    valid = false;
                }
            }
            if (ConfirmPassword == "")
            {
                ConfirmPasswordErrMsg = "Confirmation mot de passe requise";
                valid = false;
            }
            if (Password != ConfirmPassword)
            {
                ConfirmPasswordErrMsg = "Mots de passes pas identiques";
                valid = false;
            }
            return valid;
        }

        private void Loading()
        {
            InputsEnabled = false;
        }

        private void LoadingDone()
        {
            InputsEnabled = true;
            CommandManager.InvalidateRequerySuggested();
        }

        private void ResetErrMsg()
        {
            UsernameErrMsg = "";
            PasswordErrMsg = "";
        }

        public string Username
        {
            get => signupEntity.Username;
            set
            {
                if (signupEntity.Username != value && value != "")
                {
                    UsernameErrMsg = "";
                    signupEntity.Username = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    signupEntity.Username = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private string usernameErrMsg;
        public string UsernameErrMsg
        {
            get => usernameErrMsg;
            set
            {
                usernameErrMsg = value;
                this.OnPropertyChanged();
            }
        }

        public string Password
        {
            get => signupEntity.Password;
            set
            {
                if (signupEntity.Password != value && value != "")
                {
                    PasswordErrMsg = "";
                    signupEntity.Password = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    signupEntity.Password = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private string passwordErrMsg;
        public string PasswordErrMsg
        {
            get => passwordErrMsg;
            set
            {
                passwordErrMsg = value;
                this.OnPropertyChanged();
            }
        }

        public string ConfirmPassword
        {
            get => signupEntity.ConfirmPassword;
            set
            {
                if (signupEntity.ConfirmPassword != value && value != "")
                {
                    ConfirmPasswordErrMsg = "";
                    signupEntity.ConfirmPassword = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    signupEntity.ConfirmPassword = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private string confirmPasswordErrMsg;
        public string ConfirmPasswordErrMsg
        {
            get => confirmPasswordErrMsg;
            set
            {
                confirmPasswordErrMsg = value;
                this.OnPropertyChanged();
            }
        }

        private bool inputsEnabled;
        public bool InputsEnabled
        {
            get { return inputsEnabled; }

            set
            {
                if (inputsEnabled == value)
                {
                    return;
                }
                inputsEnabled = value;
                this.OnPropertyChanged();
            }
        }
    }
}

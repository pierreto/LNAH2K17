using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF.Signup
{
    public class SignupViewModel : ViewModelBase
    {

        private SignupEntity signupEntity;

        static HttpClient client = new HttpClient();

        public SignupViewModel(SignupEntity signupEntity)
        {
            this.signupEntity = signupEntity;
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
                    if (!ValidatePasswordsIdentical())
                    {
                        return null;
                    }
                    var response = await client.PostAsJsonAsync("http://localhost:63056/api/signup", signupEntity);
                    if (response.IsSuccessStatusCode)
                    {
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

        private bool ValidatePasswordsIdentical()
        {
            if(Password == ConfirmPassword)
            {
                return true;
            }
            ConfirmPasswordErrMsg = "Mots de passes pas identiques";
            return false;
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

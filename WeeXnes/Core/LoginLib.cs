using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using Newtonsoft.Json;
using WeeXnes.Views.ProfileView;
using WeeXnes.Views.Settings;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace WeeXnes.Core
{
    
    public class Auth
    {
        public BackgroundWorker _loginWorker { get; set; }
        public UpdateVar<dynamic> _currentUserCache { get; private set; } = new UpdateVar<dynamic>();
        private string _token { get; set; }
        private string _userDataUrl { get; set; }
        private string _loginUrl { get; set; }
        public string _email { get; set; }
        public string _password { private get; set; }
        public UpdateVar<Exception> ExceptionCache { get; private set; } = new UpdateVar<Exception>();

        public Auth(string loginUrl, string userDataUrl)
        {
            this._currentUserCache.Value = null;
            this._loginWorker = new BackgroundWorker();
            this._loginWorker.WorkerReportsProgress = true;
            this._loginWorker.WorkerSupportsCancellation = true;
            this._loginWorker.DoWork += (sender, args) =>
            {
                try
                {
                    Login(this._email, this._password);
                    GetUserData();
                }
                catch (Exception ex)
                {
                    Console.Error(ex.ToString());
                    this.ExceptionCache.Value = ex;
                }
            };
            this._loginWorker.RunWorkerCompleted += LoginWorkerOnRunWorkerCompleted;
            this._loginUrl = loginUrl;
            this._userDataUrl = userDataUrl;
        }

        private void LoginWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("LoginWorker complete");
        }

        public string Login(string email, string password)
        {
            if (String.IsNullOrEmpty(email))
                throw new NullReferenceException();
            if (String.IsNullOrEmpty(password))
                throw new NullReferenceException();
            
            using (HttpClient client = new HttpClient())
            {
                var postData = new Dictionary<string, string>
                {
                    { "email", email },
                    { "password", password }
                };

                var content = new FormUrlEncodedContent(postData);

                HttpResponseMessage response = client.PostAsync(_loginUrl, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);
                    string token = jsonData.token;
                    _token = token;
                    return token;
                    //Console.WriteLine($"Token: {token}");
                    
                }
                else
                {
                    Console.Error("Error: " + response.StatusCode);
                    LoginView.errorStringCache.Value = response.StatusCode.ToString();
                    return null;
                }
            }
        }

        public dynamic GetUserData(string token = null)
        { 
            if(String.IsNullOrEmpty(token))
                if (String.IsNullOrEmpty(_token))
                {
                    return null;
                }
                else
                {
                    token = _token;
                }
            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Add the token to the request headers
                client.DefaultRequestHeaders.Add("token", token);

                // Send the GET request
                HttpResponseMessage response = client.GetAsync(_userDataUrl).Result;

                // Check if the request was successful (status code 200)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    //Console.WriteLine(responseData);
                    // Parse the JSON data into a dynamic object
                    dynamic user = JsonConvert.DeserializeObject(responseData);
                    user = user.user;
                    
                    // Now you can access the user object properties dynamically
                    Console.WriteLine("authenticated user: " + user.name);
                    //Console.WriteLine($"Email: {user.email}");
                    // Access other properties as needed
                    _currentUserCache.Value = user;
                    LoginView.alreadyLoggedIn = true;
                    return user;
                }
                else
                {
                    // Handle the error, e.g., print the status code
                    _currentUserCache.Value = null;
                    
                    Console.Error("Error: " + response.StatusCode);
                    
                    LoginView.errorStringCache.Value = response.StatusCode.ToString();
                    return null;
                }
            }
        }
        
    }
}
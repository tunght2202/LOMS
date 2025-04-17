using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Services
{
    public static class ApiServiceProvider
    {
        private static ApiService _instance;
        private static string _token;

        public static ApiService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ApiService();
                }

                var currentToken = Android.App.Application.Context
                    .GetSharedPreferences("auth", FileCreationMode.Private)
                    .GetString("token", null);

                if (!string.IsNullOrEmpty(currentToken) && currentToken != _token)
                {
                    _token = currentToken;
                    _instance.SetToken(_token); 
                }

                return _instance;
            }
        }

        public static void SetToken(string token)
        {
            _token = token;
            Instance.SetToken(token);
        }

        public static string Token => _token;
    }

}

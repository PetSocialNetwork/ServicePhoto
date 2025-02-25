using ServicePhoto.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;

namespace ServicePhoto.WebHostEnviroment
{
    public class WebHostEnviroment : IWebHostEnv
    {
        private readonly IWebHostEnvironment _webHostEnvironment; // Исправлено: Изменено название поля

        public WebHostEnviroment(IWebHostEnvironment webHostEnvironment) // Исправлено: Имя параметра
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetWebRoothPath()
        {
            return _webHostEnvironment.WebRootPath;
        }
    }
}

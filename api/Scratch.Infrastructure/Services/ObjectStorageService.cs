using Microsoft.AspNetCore.Http;
using Scratch.Application.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch.Infrastructure.Services
{
    public class ObjectStorageService : IObjectStorageService
    {
        private readonly string _savePath = @"C:\\Users\\Admin\\Desktop\\_\\playground\\scratch-project\\test-storage";

        public async Task<string> Save(string name, IFormFile file)
        {
            string path = Path.Combine(_savePath, name);
            using FileStream fileStream = File.Create(path);
            await file.CopyToAsync(fileStream);
            return path;
        }
    }
}

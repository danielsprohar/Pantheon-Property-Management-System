﻿using System.Collections.Generic;

namespace Nubles.Core.Application.Wrappers
{
    public class ApiResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(string message)
        {
            Message = message;
            Succeeded = false;
        }

        public ApiResponse(string message, bool succeeded)
        {
            Message = message;
            Succeeded = succeeded;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reminder.Models
{
    public class BaseApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class ApiMessages
    {
        public static string SuccessfullyCreated => "Successfully created";
        public static string SuccessfullyUpdated => "Successfully updated";
        public static string SuccessfullyDeleted => "Successfully deleted";
        public static string SuccessfullyFetched => "Successfully fetched";
        public static string Success => "Success";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqClientAPI.Models
{
    public class APIResponseListModel<T>
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; }
    }


    public class APIResponseModel<T>
    {
        public T Data { get; set; } = default!;
    }
}

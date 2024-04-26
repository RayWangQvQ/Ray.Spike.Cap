using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ray.Spike.Cap.Common
{
    public class MyEvt
    {
        public bool Success { get; set; }

        public int Count { get; set; }

        public string Title { get; set; }

        public static MyEvt Create()
        {
            return new MyEvt()
            {
                Success = true,
                Count = 10,
                Title = "Test"
            };
        }
    }
}

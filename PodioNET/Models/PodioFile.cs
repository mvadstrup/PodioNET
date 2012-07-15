using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodioNET.Models
{
    public class PodioFile
    {
        public class FileResponse
        {
            public Int64 File_Id { get; set; }
            public string Name { get; set; }
            public string Link { get; set; }
        }

    }
}

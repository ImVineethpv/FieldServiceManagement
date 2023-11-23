using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSM.Model
{
    public class AttachmentModel
    {
        public bool IsDocument { get; set; }
        public bool IsImage {  get; set; }
        public string Id { get; set; }
        public string FileName { get; set; }
        public string Author { get; set; }        
        public string Size { get; set; }
        public string DateAdded { get; set; }

    }
}

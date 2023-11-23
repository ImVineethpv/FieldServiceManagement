using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSM.Model
{
    public class CommentModel
    {
        public string Id { get; set; }
        public string TextContent { get; set; }
        public ImageSource ImageSource {  get; set; }
        public string Author { get; set; }
        public string DateAdded { get; set; }
        public bool IsImage {  get; set; }
        public string AttachmentId { get; set; }
        public bool IsText { get; set; }
        public ContentType Type { get; set; }
    }
    public enum ContentType
    {
        Text,
        Image
    }
}

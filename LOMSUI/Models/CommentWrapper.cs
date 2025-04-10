using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class CommentWrapper : Java.Lang.Object
    {
        public CommentModel Comment { get; }

        public CommentWrapper(CommentModel comment)
        {
            Comment = comment;
        }
    }

}

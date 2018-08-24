using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{

    /// <summary>
    /// Represents exceptions raised in the target language.
    /// </summary>
    [Serializable]
    public class InterpreterException : Exception
    {
        // default features
        public InterpreterException() { }
        public InterpreterException(string message) : base(message) { }
        public InterpreterException(string message, Exception inner) : base(message, inner) { }
        protected InterpreterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        // my customized features
        public InterpreterException(string source, string description)
            : this(source, description, null) { }
        public InterpreterException(string source, string description, string relatedCode)
        {
            Source = source;
            Description = description;
            RelatedCode = relatedCode;
        }

        public new string Source { get; } = null;

        public string Description { get; } = null;

        public string RelatedCode { get; } = null;

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            base.GetObjectData(info, context);

            info.AddValue("new Source", Source, typeof(string));
            info.AddValue("Description", Description, typeof(string));
            info.AddValue("RelatedCode", RelatedCode, typeof(string));
        }
    }
}

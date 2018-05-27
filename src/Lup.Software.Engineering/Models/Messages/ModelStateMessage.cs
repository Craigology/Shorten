using System.Collections.Generic;

namespace Lup.Software.Engineering.Models.Messages
{
    public class ModelStateMessage
    {
        public ModelStateMessage()
        {
            this.Errors = new List<string>();
        }

        public string Key { get; set; }

        public string AttemptedValue { get; set; }

        public object RawValue { get; set; }

        public ICollection<string> Errors { get; set; }
    }
}
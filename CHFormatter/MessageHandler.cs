using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFormatter
{
    internal class MessageHandler
    {
        private Dictionary<string, Action<string>> m_messageDirectory;

        public MessageHandler()
        {
            m_messageDirectory = new Dictionary<string, Action<string>>();
        }
    }
}

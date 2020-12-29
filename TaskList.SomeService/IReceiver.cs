using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList.SomeService
{
    public interface IReceiver
    {
        void TakeMessage();
    }
}

using BmsStudio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Application.Interfaces
{
    public interface ICanListenerService
    {
        void Start();
        void Stop();

        event Action<CanMessage>? MessageReceived;
    }
}

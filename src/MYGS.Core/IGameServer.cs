using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYGS.Core;

public interface IGameServer
{
    public void StartServer();
    public void StopServer();
    public void CheckForServerUpdate();
    public void UpdateServer();
}

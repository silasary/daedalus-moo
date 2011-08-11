using System;
using Chiroptera.Base;
namespace Daedalus
{
    public interface IConnection : IChiConsole, INetwork
    {
        void AddWidgit(System.Windows.Forms.Control control);
        void AddWidgit(System.Windows.Forms.ToolStripItem toolstripitem);
        WorldForm Form { get; }
    }
}

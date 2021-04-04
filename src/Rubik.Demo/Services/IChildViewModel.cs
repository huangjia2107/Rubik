using System;

namespace Rubik.Demo.Services
{
    public interface IChildViewModel
    {
        bool IsOpened { get; set; }
        string ViewName { get; }
        Type ParentViewType { get; }
    }
}

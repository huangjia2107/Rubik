using System;

namespace Rubik.Service.ViewModels
{
    public interface IChildViewModel
    {
        bool IsOpened { get; set; }
        string ViewName { get; }

        Type ParentViewType { get; }
        string ParentViewName { get; }
    }
}

using System;
using System.Windows.Data;

namespace Rubik.Theme.Datas
{
    public interface IRelativeBinding
    {
        Type AncestorType { get; set; }
        uint AncestorLevel { get; set; }
    }

    public class RelativeBinding : Binding, IRelativeBinding
    {
        public RelativeBinding()
            : this(null)
        {
        }

        public RelativeBinding(string path)
            : base(path)
        {
            AncestorLevel = 1;
        }

        public Type AncestorType { get; set; }
        public uint AncestorLevel { get; set; }
    }

    public class RelativeMultiBinding : MultiBinding, IRelativeBinding
    {
        public RelativeMultiBinding()
            : base()
        {
            AncestorLevel = 1;
        }

        public Type AncestorType { get; set; }
        public uint AncestorLevel { get; set; }
    }
}

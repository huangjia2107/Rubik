using Rubik.Service.Models;

namespace Rubik.Service
{
    public interface IDemoModel
    {
        /// <summary>
        /// Type
        /// </summary>
        DemoType Type { get; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Icon Path
        /// </summary>
        string IconData { get; }

        /// <summary>
        /// Icon Width
        /// </summary>
        double IconWidth { get; }

        /// <summary>
        /// Icon Height
        /// </summary>
        double IconHeight { get; }

        /// <summary>
        /// Demo Page
        /// </summary>
        object Page { get; }
    }
}

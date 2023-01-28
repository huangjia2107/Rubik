using Rubik.Service.Models;

namespace Rubik.Service
{
    public interface IDemoModel
    {
        /// <summary>
        /// Type
        /// </summary>
        DemoType Type { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Icon Path
        /// </summary>
        string IconData { get; set; }

        /// <summary>
        /// Icon Width
        /// </summary>
        double IconWidth { get; set; }

        /// <summary>
        /// Icon Height
        /// </summary>
        double IconHeight { get; set; }

        /// <summary>
        /// Demo Page
        /// </summary>
        object Page { get; set; }
    }
}

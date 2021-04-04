using System;
using Prism.Events;

namespace Rubik.Demo.Events
{
    public class NavigationContentPayload
    {
        public bool IsChild { get; set; }
        public Type ViewType { get; set; }
        public string ViewName { get; set; }
    }

    public class NavigationContentEvent: PubSubEvent<NavigationContentPayload>
    {
    }
}

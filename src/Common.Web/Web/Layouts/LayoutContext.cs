using System;

namespace Common.Web.Layouts
{
    public class LayoutContext
    {
        public string Layout { get; set; }

        public static LayoutContext Current => LayoutContextService.Resolve().GetLayoutContext();
    }
}
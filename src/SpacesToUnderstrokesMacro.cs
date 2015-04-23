using System;
using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.Util;

namespace ResharperPlugins
{
    //https://chrisseroka.wordpress.com/2013/03/23/resharper-template-for-unit-test-method-replacing-spaces-in-the-test-name-with-underscore-sign/

    [Macro("chsreplacespacewithunderscore",
        LongDescription = "Replace spaces with '_' (i.e. \"should return nothing\" into \"should_return_nothing\")",
        ShortDescription = "Replace spaces with '_'")]
    public class SpacesToUnderstrokesMacro : IMacro
    {
        private static string Evaluate(string text)
        {
            return text.Replace(" ", "_");
        }

        public ParameterInfo[] Parameters
        {
            get { return EmptyArray<ParameterInfo>.Instance; }
        }

        public HotspotItems GetLookupItems(IHotspotContext context, IList<string> arguments)
        {
            return null;
        }

        public string GetPlaceholder(IDocument document)
        {
            return "sentence with spaces";
        }

        public string EvaluateQuickResult(IHotspotContext context, IList<string> arguments)
        {
            if (context.HotspotSession == null || context.HotspotSession.CurrentHotspot == null)
            {
                return null;
            }
            var currentValue = context.HotspotSession.CurrentHotspot.CurrentValue;
            return Evaluate(currentValue);
        }

        public bool HandleExpansion(IHotspotContext context, IList<string> arguments)
        {
            context.HotspotSession.HotspotUpdated += this.CurrentHotspotUpdated;

            return false;
        }

        private void CurrentHotspotUpdated(object sender, EventArgs e)
        {
            var hotspotSession = sender as IHotspotSession;
            if (hotspotSession != null)
            {
                hotspotSession.CurrentHotspot.QuickEvaluate();
            }
        }
    }
}

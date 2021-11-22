using BlazorApp.Client.Shared;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Xunit;

namespace PersonalSiteTests.Shared
{
    public class MarkdownViewTests
    {
        [Fact]
        public void TestLayout()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<MarkdownView>();

            cut.MarkupMatches(@"<div id=""markdown-component""></div>");
        }
    }
}

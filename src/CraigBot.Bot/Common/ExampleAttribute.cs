using System;

namespace CraigBot.Bot.Common
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class ExampleAttribute : Attribute
    {
        public string ExampleText { get; }

        public ExampleAttribute(string exampleText)
        {
            ExampleText = exampleText;
        }
    }
}
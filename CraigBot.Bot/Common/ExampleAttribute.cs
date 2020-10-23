using System;

namespace CraigBot.Bot.Common
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExampleAttribute : Attribute
    {
        public string ExampleText { get; }

        public ExampleAttribute(string exampleText)
        {
            ExampleText = exampleText;
        }
    }
}
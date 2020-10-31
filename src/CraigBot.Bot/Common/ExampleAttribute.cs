using System;

namespace CraigBot.Bot.Common
{
    // TODO: Allow the prefix to be passed in maybe?
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
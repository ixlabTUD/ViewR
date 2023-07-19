// The MIT License (MIT) - https://gist.github.com/bcatcho/1926794b7fd6491159e7
// Copyright (c) 2015 Brandon Catcho
// Modified by Florian Schier, 2021

using System;

namespace ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor
{
    
    /// <summary>
    /// Simply add a "[ExposeMethodInEditor]" attribute to a method and you can call it from play mode from within the editor!
    /// </summary>
    // Restrict to methods only
    [AttributeUsage(AttributeTargets.Method)]
    public class ExposeMethodInEditorAttribute : Attribute
    {
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public abstract class TailwindGenerator
{
    private const string JsonThemeKey = "theme";
    private const string JsonExtendKey = "extend";
    private const string DefaultValue = "default";
    private const string ColorsSection = "colors";

    private struct Property
    {
        public string Name { get; set; }
        public string ClassPrefix { get; set; }
        public string CssName { get; set; }
    }

    private struct ValueTranslation
    {
        public string RequiredSection { get; }

        public string Value { get; }

        public ValueTranslation(string value)
        {
            RequiredSection = string.Empty;
            Value = value;
        }

        public ValueTranslation(string section, string value)
        {
            RequiredSection = section;
            Value = value;
        }
    }

    private static readonly Property[] BaseMappings =
    {
        new() { Name = "alignItems", ClassPrefix = "items", CssName = "align-items" },
        new() { Name = "flex", ClassPrefix = "flex", CssName = "flex-direction" },
        new() { Name = "flexGrow", ClassPrefix = "grow", CssName = "flex-grow" },
        new() { Name = "flexWrap", ClassPrefix = "wrap", CssName = "flex-wrap" },
        new() { Name = "flexShrink", ClassPrefix = "shrink", CssName = "flex-shrink" },
        new() { Name = "justifyContent", ClassPrefix = "justify", CssName = "justify-content" },
        new() { Name = "fontSize", ClassPrefix = "text", CssName = "font-size" },
        new() { Name = "fontWeight", ClassPrefix = "font", CssName = "font-weight" },
        new() { Name = "fontStyle", ClassPrefix = "font", CssName = "-unity-font-style" },
        new() { Name = "lineHeight", ClassPrefix = "leading", CssName = "line-height" },
        new() { Name = "transformOrigin", ClassPrefix = "origin", CssName = "transform-origin" },
        new() { Name = "transitionDelay", ClassPrefix = "delay", CssName = "transition-delay" },
        new() { Name = "transitionDuration", ClassPrefix = "duration", CssName = "transition-duration" },
        new() { Name = "transitionTimingFunction", ClassPrefix = "ease", CssName = "transition-timing-function" },
        new() { Name = "transitionProperty", ClassPrefix = "transition", CssName = "transition-property" },
    };

    private static readonly Dictionary<string, Property[]> Permutations = new()
    {
        {
            ColorsSection, new[]
            {
                new Property { Name = "backgroundColor", ClassPrefix = "bg", CssName = "background-color" },
                new Property { Name = "borderColor", ClassPrefix = "border", CssName = "border-color" },
                new Property { Name = "color", ClassPrefix = "text", CssName = "color" },
            }
        },
        {
            "spacing", new[]
            {
                new Property { Name = "width", ClassPrefix = "w", CssName = "width" },
                new Property { Name = "height", ClassPrefix = "h", CssName = "height" },
                new Property { Name = "maxWidth", ClassPrefix = "max-w", CssName = "max-width" },
                new Property { Name = "maxHeight", ClassPrefix = "max-h", CssName = "max-height" },
                new Property { Name = "minWidth", ClassPrefix = "min-w", CssName = "min-width" },
                new Property { Name = "minHeight", ClassPrefix = "min-h", CssName = "min-height" },
                new Property { Name = "margin", ClassPrefix = "m", CssName = "margin" },
                new Property { Name = "margin", ClassPrefix = "m-t", CssName = "margin-top" },
                new Property { Name = "margin", ClassPrefix = "m-b", CssName = "margin-bottom" },
                new Property { Name = "margin", ClassPrefix = "m-l", CssName = "margin-left" },
                new Property { Name = "margin", ClassPrefix = "m-r", CssName = "margin-right" },
                new Property { Name = "padding", ClassPrefix = "p", CssName = "padding" },
                new Property { Name = "padding", ClassPrefix = "p-t", CssName = "padding-top" },
                new Property { Name = "padding", ClassPrefix = "p-b", CssName = "padding-bottom" },
                new Property { Name = "padding", ClassPrefix = "p-l", CssName = "padding-left" },
                new Property { Name = "padding", ClassPrefix = "p-r", CssName = "padding-right" },
            }
        },
        {
            "borderRadius", new[]
            {
                new Property { Name = "borderRadius", ClassPrefix = "rounded", CssName = "border-radius" },
                new Property { Name = "borderRadius", ClassPrefix = "rounded-tl", CssName = "border-top-left-radius" },
                new Property { Name = "borderRadius", ClassPrefix = "rounded-tr", CssName = "border-top-right-radius" },
                new Property { Name = "borderRadius", ClassPrefix = "rounded-bl", CssName = "border-bottom-left-radius" },
                new Property { Name = "borderRadius", ClassPrefix = "rounded-br", CssName = "border-bottom-right-radius" },
            }
        },
        {
            "borderWidth", new[]
            {
                new Property { Name = "borderWidth", ClassPrefix = "border", CssName = "border-width" },
                new Property { Name = "borderWidth", ClassPrefix = "border-t", CssName = "border-top-width" },
                new Property { Name = "borderWidth", ClassPrefix = "border-b", CssName = "border-bottom-width" },
                new Property { Name = "borderWidth", ClassPrefix = "border-l", CssName = "border-left-width" },
                new Property { Name = "borderWidth", ClassPrefix = "border-r", CssName = "border-right-width" },
            }
        }
    };

    /// <summary>
    /// Values to translate from standard CSS to Unity.
    /// </summary>
    private static readonly Dictionary<string, ValueTranslation> Translations = new()
    {
        { "transparent", new ValueTranslation("rgba(0,0,0,0)") },
        { "none", new ValueTranslation("borderRadius", "0px") },
        { "baseline", new ValueTranslation("alignItems", "initial") },
        { "0px", new ValueTranslation("0") },
        { "gray", new ValueTranslation("colors", "rgb(128,128,128)") }
    };

    private static readonly string[] DefaultColors =
    {
        "red", "blue", "green", "yellow", "purple", "pink", "indigo", "teal", "cyan", "white", "black", "gray"
    };

    private static readonly string[] Selectors =
    {
        null, "hover", "active", "inactive", "focus", "disabled"
    };

    private static readonly Dictionary<string, Property> PropertiesMapping;

    static TailwindGenerator()
    {
        PropertiesMapping = BaseMappings.ToDictionary(p => p.Name);
    }

    private static JObject ReadJsonTheme(string jsonFilePath)
    {
        var tailwindJson = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(jsonFilePath));
        return tailwindJson.GetValue(JsonThemeKey)!.Value<JObject>();
    }

    /// <summary>
    /// Returns the base class name for the given section and value key.
    /// </summary>
    private static string GetClassName(string themeSection, string valueKey)
    {
        var prefix = PropertiesMapping.TryGetValue(themeSection, out var property)
            ? property.ClassPrefix
            : themeSection;

        if (valueKey == DefaultValue)
            return prefix;

        if (string.IsNullOrEmpty(prefix))
        {
            return valueKey;
        }

        return prefix + "-" + valueKey;
    }

    /// <summary>
    /// If the given value string is a full CSS property, returns true.
    /// </summary>
    private static bool IsCssPropertyFull(string valueStr)
    {
        return valueStr.Contains(':');
    }

    private static string GetCssPropertyValue(Property property, string valueStr)
    {
        if (IsCssPropertyFull(valueStr))
        {
            return valueStr;
        }

        return property.CssName + ": " + valueStr + ";";
    }

    /// <summary>
    /// Returns the CSS property value for the given section and value string.
    /// </summary>
    private static string GetCssPropertyValue(string themeSection, string valueStr)
    {
        if (PropertiesMapping.TryGetValue(themeSection, out var property))
        {
            return GetCssPropertyValue(property, valueStr);
        }

        if (IsCssPropertyFull(valueStr))
        {
            return valueStr;
        }

        return themeSection + ": " + valueStr + ";";
    }

    private static string TranslateValue(string currentSection, string value)
    {
        while (true)
        {
            if (!Translations.TryGetValue(value, out var translation)) return value;

            if (!string.IsNullOrEmpty(translation.RequiredSection) &&
                translation.RequiredSection != currentSection) return value;

            var translatedValue = translation.Value;
            if (!Translations.ContainsKey(translatedValue)) return translatedValue;

            value = translatedValue;
        }
    }

    /// <summary>
    /// Extends a JObject with another JObject adding only the properties that are
    /// already present in the target.
    /// </summary>
    private static void ExtendJObject(JObject target, JObject extend)
    {
        foreach (var (propertyName, value) in extend)
        {
            if (value == null || !target.TryGetValue(propertyName, out var dstValue)) continue;

            foreach (var (keyToAdd, valueToAdd) in value.Value<JObject>())
            {
                dstValue[keyToAdd] = valueToAdd;
            }
        }
    }

    private static string SanitizeClassName(string className)
    {
        return className
            .Replace('.', '-')
            .Replace('/', '_');
    }

    private static void WriteCssClass(TextWriter writer, string className, string cssProperty, string selector)
    {
        writer.Write("." + className);
        if (selector != null)
        {
            writer.Write(":" + selector);
        }

        writer.Write(" { ");
        writer.Write(cssProperty);
        writer.WriteLine(" }");
    }

    private static void GenerateClass(
        TextWriter writer,
        string themeSection,
        string valueKey,
        JToken value,
        string selector,
        Property? overrideProp = null)
    {
        if (value.Type == JTokenType.Object)
        {
            foreach (var (k, v) in value.Value<JObject>())
            {
                GenerateClass(writer, themeSection, valueKey + "-" + k, v, selector, overrideProp);
            }
        }
        else
        {
            var valueStr = value.ToString().Trim();
            if (valueStr.EndsWith(';'))
            {
                valueStr = valueStr[..^1];
            }

            valueStr = TranslateValue(themeSection, valueStr);

            string className, cssProperty;

            if (overrideProp.HasValue)
            {
                className = overrideProp.Value.ClassPrefix;
                if (valueKey != DefaultValue)
                {
                    className = className + "-" + valueKey;
                }

                cssProperty = GetCssPropertyValue(overrideProp.Value, valueStr);
            }
            else
            {
                className = GetClassName(themeSection, valueKey);
                cssProperty = GetCssPropertyValue(themeSection, valueStr);
            }

            if (selector != null)
            {
                className = selector + '-' + className;
            }

            className = SanitizeClassName(className);

            WriteCssClass(writer, className, cssProperty, selector);
        }
    }

    private static JObject LoadMergedTheme(string jsonFilePath, string jsonExtFilePath)
    {
        var tailWindTheme = ReadJsonTheme(jsonFilePath);

        if (string.IsNullOrEmpty(jsonExtFilePath)) return tailWindTheme;

        var tailwindExtTheme = ReadJsonTheme(jsonExtFilePath);

        foreach (var (section, value) in tailwindExtTheme)
        {
            if (section == JsonExtendKey)
            {
                ExtendJObject(tailWindTheme, value!.Value<JObject>());
            }
            else if (tailWindTheme.ContainsKey(section))
            {
                tailWindTheme[section] = value;
            }
        }

        return tailWindTheme;
    }

    public static void Generate(string ussOutputFile, string jsonFilePath, string jsonExtFilePath)
    {
        var tailWindTheme = LoadMergedTheme(jsonFilePath, jsonExtFilePath);

        using var writer = new StreamWriter(ussOutputFile);

        foreach (var selector in Selectors)
        {
            // Write default colors
            var colorProperties = Permutations[ColorsSection];

            foreach (var color in DefaultColors)
            {
                foreach (var property in colorProperties)
                {
                    GenerateClass(writer, ColorsSection, color, JToken.FromObject(color), selector, property);
                }
            }

            foreach (var (themeSection, options) in tailWindTheme)
            {
                foreach (var (key, value) in options!.Value<JObject>())
                {
                    if (Permutations.TryGetValue(themeSection, out var permutations))
                    {
                        foreach (var p in permutations)
                        {
                            GenerateClass(writer, themeSection, key, value, selector, p);
                        }
                    }
                    else
                    {
                        GenerateClass(writer, themeSection, key, value, selector);
                    }
                }
            }
        }
    }
}
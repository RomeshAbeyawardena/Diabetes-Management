using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiabetesManagement.App.Shared.Services
{
    public class AssetService
    {
        private HtmlDocument GetDocumentFromHtml(string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            return htmlDocument;
        }

        private HtmlNodeCollection GetNodes(HtmlDocument htmlDocument, string xPath)
        {
            return htmlDocument.DocumentNode.SelectNodes(xPath);
        }

        public string GetAssetInBase64(string assetName, Func<string, Stream> getAsset)
        {
            var buffer = new List<byte>();
            using (var assetStream = getAsset(assetName))
            {
                int currentByte = 0;
                while((currentByte = assetStream.ReadByte()) != -1)
                {
                    buffer.Add(Convert.ToByte(currentByte));
                }
                
            }
            var base64String = Convert.ToBase64String(buffer.ToArray());
            buffer.Clear();
            buffer.Capacity = 0;
            GC.Collect(GC.GetGeneration(buffer), GCCollectionMode.Forced);
            return base64String;
        }

        public string GetAsset(string assetName, Func<string, Stream> getAsset)
        {
            using (var streamReader = new StreamReader(getAsset(assetName), true))
            {
                return streamReader.ReadToEnd();
            }
        }

        public IEnumerable<string> ParseRequiredResources(string html)
        {
            var resources = new List<string>();
            var htmlDocument = GetDocumentFromHtml(html);

            foreach (var styleNode in GetNodes(htmlDocument, "//head/link"))
            {
                resources.Add(styleNode.Attributes["href"].Value);
            }

            return resources;
        }

        public Dictionary<string, string> GetResources(IEnumerable<string> resources, Func<string, Stream> getAsset)
        {
            var assetDictionary = new Dictionary<string, string>();

            foreach (var resource in resources)
            {
                var asset = GetAsset($"Content{resource}", getAsset);

                if (assetDictionary.ContainsKey(resource))
                {
                    assetDictionary[resource] = asset;
                }
                else
                    assetDictionary.Add(resource, asset);

            }

            return assetDictionary;
        }

        public string ReplaceStyleResources(string style, Func<string, Stream> getAsset)
        {
            var parser = new ExCSS.StylesheetParser();
            var css = parser.Parse(style);
            var rules = css.FontfaceSetRules;
            foreach(var rule in rules)
            {
                var s = rule.Source;
                if (s.Contains("url"))
                {
                    var matches =  Regex.Matches(s, "(url)([(][\"])([#]|[?]|[0-9]|[a-z]|[.]|[/])+([\"][)])");
                    foreach(Match m in matches)
                    {
                        var value = string.Empty;

                        foreach(Capture capture in m.Groups[3].Captures)
                        {
                            value += capture.Value;
                        }

                        var assetPath = value.Split('?')[0].Replace("..", "Content");

                        var val = GetAssetInBase64(assetPath, getAsset);

                        var contentType = string.Empty;
                        if (assetPath.EndsWith("eot"))
                        {
                            contentType = "application/vnd.ms-fontobject";
                        }
                        else if (assetPath.EndsWith("ttf"))
                        {
                            contentType = "application/octet-stream";
                        }
                        else if (assetPath.EndsWith("woff"))
                        {
                            contentType = "application/font-woff";
                        }
                        else if (assetPath.EndsWith("svg"))
                        {
                            contentType = "image/svg+xml";
                        }

                        rule.Source = rule.Source.Replace(value, $"{contentType};base64,{val}");
                    }
                    
                }
            }
            using (var streamWriter = new StringWriter())
            {
                css.ToCss(streamWriter, ExCSS.CompressedStyleFormatter.Instance);
                return streamWriter.ToString();
            }
        }

        public string ReplaceResources(string html, Dictionary<string, string> resources,
            Func<string, Stream> getAsset)
        {
            var htmlDocument = GetDocumentFromHtml(html);

            foreach (var styleNode in GetNodes(htmlDocument, "//head/link"))
            {
                styleNode.Remove();
            }

            foreach (var resource in resources)
            {
                var headElement = htmlDocument.DocumentNode.SelectSingleNode("//head");
                var bodyElement = htmlDocument.DocumentNode.SelectSingleNode("//body");
                var headChildNodes = headElement.ChildNodes;
                var bodyChildNodes = bodyElement.ChildNodes;
                if (resource.Key.EndsWith("css", StringComparison.InvariantCultureIgnoreCase))
                {
                    var styleNode = HtmlNode
                        .CreateNode($"<style type=\"text/css\">{ReplaceStyleResources(resource.Value, getAsset)}</style>");
                    headChildNodes.Add(styleNode);
                }

                if (resource.Key.EndsWith("js", StringComparison.InvariantCultureIgnoreCase))
                {
                    var styleNode = HtmlNode
                        .CreateNode($"<script type=\"application/javascript\">{resource.Value}</script>");
                    bodyChildNodes.Add(styleNode);
                }
            }

            using (var stringWriter = new StringWriter())
            {
                htmlDocument.Save(stringWriter);
                return stringWriter.ToString();
            }
        }
    }
}

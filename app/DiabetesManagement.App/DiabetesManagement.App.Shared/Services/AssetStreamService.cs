using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiabetesManagement.App.Shared.Services
{
    public class AssetStreamService
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

        public string ReplaceResources(string html, Dictionary<string, string> resources)
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
                        .CreateNode($"<style type=\"text/css\">{resource.Value}</style>");
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

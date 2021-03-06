﻿namespace ServiceInsight.Explorer.EndpointExplorer
{
    using System.Drawing;
    using global::ServiceInsight.Properties;
    using ServiceInsight.Models;

    public class AuditEndpointExplorerItem : EndpointExplorerItem
    {
        public AuditEndpointExplorerItem(Endpoint endpoint, string hostNames = "")
            : base(endpoint)
        {
            HostNames = hostNames;
        }

        public string HostNames { get; set; }

        public override Bitmap Image => Resources.TreeAuditQueue;
    }
}
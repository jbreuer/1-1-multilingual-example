// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureDirectoryFactory.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   Defines the AzureDirectoryFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Umbraco.Extensions.ExamineAzure
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web;

    using Examine.Directory.AzureDirectory;
    using Examine.LuceneEngine.Providers;

    using Lucene.Net.Store;

    using Microsoft.WindowsAzure.Storage;

    using Umbraco.Core.Logging;
    using Umbraco.Extensions.Extensions;

    using Directory = Lucene.Net.Store.Directory;

    /// <summary>
    /// The azure umbraco content indexer.
    /// </summary>
    public class AzureDirectoryFactory : IDirectoryFactory
    {
        /// <summary>
        /// Return the AzureDirectory.
        /// It stores the master index in Blob storage.
        /// Only a master server can write to it.
        /// For each slave server, the blob storage index files are synced to the local machine.
        /// </summary>
        /// <param name="indexer">
        /// The indexer.
        /// </param>
        /// <param name="luceneIndexFolder">
        /// The lucene index folder.
        /// </param>
        /// <returns>
        /// The <see cref="Lucene.Net.Store.Directory"/>.
        /// </returns>
        public Directory CreateDirectory(LuceneIndexer indexer, string luceneIndexFolder)
        {
            return new AzureDirectory(
                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["azure:connectionString"]),
                ConfigurationManager.AppSettings["azure:umbracoExamineContainerName"], 
                GetLocalStorageDirectory(indexer.IndexSetName), 
                rootFolder: indexer.IndexSetName);
        }

        /// <summary>
        /// Return the folder where the Examine indexes will be stored local.
        /// </summary>
        /// <param name="name">
        /// The name
        /// </param>
        /// <returns>
        /// The <see cref="Lucene.Net.Store.Directory"/>.
        /// </returns>
        private static Directory GetLocalStorageDirectory(string name)
        {
            // Include the appdomain hash is just a safety check, for example if a website is moved from worker A to worker B and then back
            // to worker A again, in theory the %temp%  folder should already be empty but we really want to make sure that its not
            // utilizing an old index.
            var appDomainHash = HttpRuntime.AppDomainAppId.ToMd5();

            var cachePath = Path.Combine(
                Environment.ExpandEnvironmentVariables("%temp%"),
                "LuceneDir", 
                appDomainHash,
                "App_Data",
                "TEMP",
                "ExamineIndexes", 
                name);

            var azureDir = new DirectoryInfo(cachePath);
            if (!azureDir.Exists)
            {
                azureDir.Create();
            }

            LogHelper.Info<AzureDirectoryFactory>("Local Examine cache path: " + cachePath);

            return new SimpleFSDirectory(azureDir);
        }
    }
}
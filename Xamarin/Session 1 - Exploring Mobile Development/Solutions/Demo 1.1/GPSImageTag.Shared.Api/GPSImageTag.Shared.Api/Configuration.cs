using System;
using System.Collections.Generic;
using System.Text;

namespace GPSImageTag.Shared.Api
{
    public static class Configuration
    {
        /// <summary>
        /// Azure Storage Connection String. UseDevelopmentStorage=true points to the storage emulator.
        /// </summary>
        public const string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=Storage Account name goes here;AccountKey=Storage Account key goes here";
        public const string StorageContainerName = "photos";
    }
}

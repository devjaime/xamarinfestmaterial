using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using SetupAzureData.Models;

namespace SetupAzureData
{
    class Program
    {
        static string filepath = string.Empty;
        static string datapath = string.Empty;
        static List<Photo> photos;
        static List<Photo> savephotos;
        static CloudStorageAccount storageAccount;
        static CloudBlobClient blobClient;
        static CloudBlobContainer container;

        static void Main(string[] args)
        {
            photos = new List<Photo>();
            savephotos = new List<Photo>();

            InitAzureStorage();

            ReadJsonFile();

            ProgressHeader();

            var SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "photo.csv");
            foreach (var photo in photos)
            {
                UploadPhotoToAzureStorage(photo);
                CreateCSVFromGenericList<Photo>(savephotos, SavePath);
            }

            ProgressFooter();
        }


        public static void CreateCSVFromGenericList<T>(List<T> list, string csvCompletePath)
        {
            if (list == null || list.Count == 0) return;

            //get type from 0th member
            Type t = list[0].GetType();
            string newLine = Environment.NewLine;

            if (!Directory.Exists(Path.GetDirectoryName(csvCompletePath))) Directory.CreateDirectory(Path.GetDirectoryName(csvCompletePath));

            if (!File.Exists(csvCompletePath)) File.Create(csvCompletePath).Close();

            using (var sw = new StreamWriter(csvCompletePath))
            {
                //make a new instance of the class name we figured out to get its props
                object o = Activator.CreateInstance(t);
                //gets all properties
                PropertyInfo[] props = o.GetType().GetProperties();

                //foreach of the properties in class above, write out properties
                //this is the header row
                sw.Write(string.Join(",", props.Select(d => d.Name).ToArray()) + newLine);

                //this acts as datarow
                foreach (T item in list)
                {
                    //this acts as datacolumn
                    var row = string.Join(",", props.Select(d => item.GetType()
                                                                    .GetProperty(d.Name)
                                                                    .GetValue(item, null)
                                                                    .ToString())
                                                            .ToArray());
                    sw.Write(row + newLine);

                }
            }
        }


        private static void ProgressFooter()
        {
            Console.WriteLine("=================================================================");
            Console.WriteLine("               Azure sample data upload complete!");
            Console.ReadLine();
        }

        private static void ProgressHeader()
        {
            Console.WriteLine("File Name                                  Azure Storage Uri");
            Console.WriteLine("=================================================================");
        }

        private static void InitAzureStorage()
        {
            // Parse the connection string and return a reference to the storage account.
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client
            blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            container = blobClient.GetContainerReference("photos");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            //By default, the new container is private.  This set the container to be public
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            
         }

        private static void ReadJsonFile()
        {
            filepath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDi‌​rectory, "..\\..\\"));
            datapath = ConfigurationSettings.AppSettings.Get("SampleDataPath");
            photos = JsonConvert.DeserializeObject<List<Photo>>(File.ReadAllText($"{filepath}{datapath}"));
        }

        private static void UploadPhotoToAzureStorage(Photo photo)
        {

            // Retrieve reference to a blob by the image file name
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(photo.Name);

            using (var fileStream = System.IO.File.OpenRead($"{filepath}{photo.Thumbnail}"))
            {
                blockBlob.UploadFromStream(fileStream);
                photo.Uri = blockBlob.Uri.ToString();
                savephotos.Add(photo);
                Console.WriteLine("{0, -30:g} {1,-10:g} {2}", photo.Name, "Uploaded!", blockBlob.Uri);
            }

        }
      
    }
}

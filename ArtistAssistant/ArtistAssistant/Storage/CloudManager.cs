//-----------------------------------------------------------------------
// <copyright file="CloudManager.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Storage
{
    using System.Collections.Generic;
    using System.IO;
    using Amazon.S3;
    using Amazon.S3.Model;

    /// <summary>
    /// Manages saving/downloading <see cref="Drawing"/> data
    /// from an AWS bucket
    /// </summary>
    public class CloudManager
    {
        /// <summary>
        /// The AWS S3 user id
        /// </summary>
        private static string user = CloudManager.GetAWSAuthenticationUser();

        /// <summary>
        /// The AWS S3 bucket key
        /// </summary>
        private static string key = CloudManager.GetAwsAuthenticationKey();

        /// <summary>
        /// The AWS S3 bucket that contains the uploaded/downloaded files
        /// </summary>
        private static string bucket = "artistassistant";

        /// <summary>
        /// The client used to upload the file
        /// </summary>
        private static IAmazonS3 client = new AmazonS3Client(user, key, Amazon.RegionEndpoint.USWest2);

        /// <summary>
        /// Uploads an image file and a JSON file to the S3 bucket
        /// </summary>
        /// <param name="jsonFile">The JSON file being uploaded</param>
        /// <param name="imageFile">The image file being uploaded</param>
        /// <param name="key">The name that the files should have in the bucket</param>
        public static void Upload(string jsonFile, string imageFile, string key)
        {
            if (!File.Exists(jsonFile) || !File.Exists(imageFile))
            {
                return;
            }

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = CloudManager.bucket,
                FilePath = jsonFile,
                Key = $"JSON/{key}"
            };

            PutObjectResponse response = client.PutObject(request);

            request = new PutObjectRequest()
            {
                BucketName = CloudManager.bucket,
                FilePath = imageFile,
                Key = $"Image/{key}"
            };
            
            response = client.PutObject(request);
        }

        /// <summary>
        /// Get the AWS Authentication User from a file
        /// </summary>
        /// <returns>The AWS Authentication User string</returns>
        private static string GetAWSAuthenticationUser()
        {
            return "";
        }

        /// <summary>
        /// Get the AWS Authentication Key from a file
        /// </summary>
        /// <returns>The AWS Authentication Key string</returns>
        private static string GetAwsAuthenticationKey()
        {
            return "";
        }

        /// <summary>
        /// Gets a list of the names of the files in the bucket
        /// </summary>
        /// <returns>A list of the names of the files in the bucket</returns>
        public static List<string> ListFiles()
        {
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = "artistassistant",
                Prefix = "JSON/"
            };

            ListObjectsV2Response response;
            List<string> fileNames = new List<string>();
            response = client.ListObjectsV2(request);

            foreach (S3Object entry in response.S3Objects)
            {
                string[] splitKey = entry.Key.Split('/');
                if (splitKey.Length > 1 && splitKey[1] != string.Empty)
                {
                    fileNames.Add(splitKey[1]);
                }
            }

            return fileNames;
        }

        /// <summary>
        /// Download the JSON file and the image file that share the given key
        /// </summary>
        /// <param name="jsonFile">The path at which the JSON file should be stored</param>
        /// <param name="imageFile">The path at which the image file should be stored</param>
        /// <param name="key">The name of the files in the bucket that should be downloaded</param>
        public static void Download(string jsonFile, string imageFile, string key)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = CloudManager.bucket,
                Key = $"JSON/{key}",
            };

            using (GetObjectResponse response = client.GetObject(request))
            {
                if (File.Exists(jsonFile))
                {
                    File.Delete(jsonFile);
                }

                response.WriteResponseStreamToFile(jsonFile);
            }

            request = new GetObjectRequest()
            {
                BucketName = CloudManager.bucket,
                Key = $"Image/{key}",
            };

            using (GetObjectResponse response = client.GetObject(request))
            {
                if (File.Exists(imageFile))
                {
                    File.Delete(imageFile);
                }

                response.WriteResponseStreamToFile(imageFile);
            }
        }

        /// <summary>
        /// Deletes the keys in the given list from the bucket
        /// </summary>
        /// <param name="keys">The files being removed from the bucket</param>
        public static void Delete(List<string> keys)
        {
            foreach (string key in keys)
            {
                DeleteObjectRequest request = new DeleteObjectRequest()
                {
                    BucketName = CloudManager.bucket,
                    Key = $"JSON/{key}",
                };

                DeleteObjectResponse response = client.DeleteObject(request);

                request = new DeleteObjectRequest()
                {
                    BucketName = CloudManager.bucket,
                    Key = $"Image/{key}",
                };

                response = client.DeleteObject(request);
            }
        }
    }
}

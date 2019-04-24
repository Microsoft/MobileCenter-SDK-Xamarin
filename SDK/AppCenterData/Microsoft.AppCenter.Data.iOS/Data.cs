﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Foundation;
using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Data.iOS.Bindings;

namespace Microsoft.AppCenter.Data
{
    public partial class Data : AppCenterService
    {
        /// <summary>
        /// Internal SDK property not intended for public use.
        /// </summary>
        /// <value>
        /// The iOS SDK Crashes bindings type.
        /// </value>
        [Preserve]
        public static Type BindingType => typeof(MSDataStore);

        private static Task<bool> PlatformIsEnabledAsync()
        {
            return Task.FromResult(MSDataStore.IsEnabled());
        }

        private static Task PlatformSetEnabledAsync(bool enabled)
        {
            MSDataStore.SetEnabled(enabled);
            return Task.FromResult(default(object));
        }

        private static void PlatformSetTokenExchangeUrl(string apiUrl)
        {
            MSDataStore.SetTokenExchangeUrl(apiUrl);
        }

        private static Task<DocumentWrapper<T>> PlatformRead<T>(string partition, string documentId)
        {
            var taskCompletionSource = new TaskCompletionSource<DocumentWrapper<T>>();

            MSDataStore.Read(partition, documentId, (resultDoc) => 
            {
                DocumentWrapper<T> doc = new DocumentWrapper<T>
                {
                    Partition = resultDoc.Partition,
                    Id = resultDoc.DocumentId,
                    DeserializedValue = Document<T>.DeserializeString(resultDoc.DeserializedValue),
                    ETag = resultDoc.ETag,
                    LastUpdatedDate = (DateTime)resultDoc.LastUpdatedDate,
                    FromDeviceCache = resultDoc.FromDeviceCache,
                    Error = ConvertErrorToException(resultDoc.Error)
                };

                taskCompletionSource.TrySetResult(doc);
            });
            return taskCompletionSource.Task;
        }

        private static Task<DocumentWrapper<T>> PlatformRead<T>(string partition, string documentId, ReadOptions readOptions)
        {
            var taskCompletionSource = new TaskCompletionSource<DocumentWrapper<T>>();

            MSReadOptions msReadOptions = new MSReadOptions 
            {
                DeviceTimeToLive= readOptions.DeviceTimeToLive.Ticks
            };
            MSDataStore.Read(partition, documentId, msReadOptions, (resultDoc) =>
            {
                DocumentWrapper<T> doc = new DocumentWrapper<T>
                {
                    Partition = resultDoc.Partition,
                    Id = resultDoc.DocumentId,
                    DeserializedValue = Document<T>.DeserializeString(resultDoc.DeserializedValue),
                    ETag = resultDoc.ETag,
                    LastUpdatedDate = (DateTime)resultDoc.LastUpdatedDate,
                    FromDeviceCache = resultDoc.FromDeviceCache,
                    Error = ConvertErrorToException(resultDoc.Error)
                };

                taskCompletionSource.TrySetResult(doc);
            });
            return taskCompletionSource.Task;
        }

        private static Task<PaginatedDocuments<T>> PlatformList<T>(string partition)
        {
            var taskCompletionSource = new TaskCompletionSource<PaginatedDocuments<T>>();
            PaginatedDocuments<T> paginatedDocs = new PaginatedDocuments<T>();
            MSDataStore.List(partition, (resultPages) =>
            {
                MSPage currentPage = resultPages.CurrentPage();
                do
                {
                    foreach (var item in currentPage.Items)
                    {
                        DocumentWrapper<T> doc = new DocumentWrapper<T>
                        {
                            Partition = item.Partition,
                            Id = item.DocumentId,
                            DeserializedValue = Document<T>.DeserializeString(item.DeserializedValue),
                            ETag = item.ETag,
                            LastUpdatedDate = (DateTime)item.LastUpdatedDate,
                            FromDeviceCache = item.FromDeviceCache,
                            Error = ConvertErrorToException(item.Error)
                        };

                        paginatedDocs.Add(doc);
                    }

                    resultPages.NextPage((page) =>
                    {
                        currentPage = page as MSPage;
                    });
                }while (currentPage == null);

                taskCompletionSource.TrySetResult(paginatedDocs);
            });
            return taskCompletionSource.Task;
        }

        private static Task<DocumentWrapper<T>> PlatformCreate<T>(string partition, string documentId, T document)
        {
            var taskCompletionSource = new TaskCompletionSource<DocumentWrapper<T>>();

            MSDataStore.Create(partition, documentId, document.ToString(), (resultDoc) =>
            {
                DocumentWrapper<T> doc = new DocumentWrapper<T>
                {
                    Partition = resultDoc.Partition,
                    Id = resultDoc.DocumentId,
                    DeserializedValue = Document<T>.DeserializeString(resultDoc.DeserializedValue),
                    ETag = resultDoc.ETag,
                    LastUpdatedDate = (DateTime)resultDoc.LastUpdatedDate,
                    FromDeviceCache = resultDoc.FromDeviceCache,
                    Error = ConvertErrorToException(resultDoc.Error)
                };

                taskCompletionSource.TrySetResult(doc);
            });
            return taskCompletionSource.Task;
        }

        private static Task<DocumentWrapper<T>> PlatformCreate<T>(string partition, string documentId, T document, WriteOptions writeOptions)
        {
            var taskCompletionSource = new TaskCompletionSource<DocumentWrapper<T>>();

            MSWriteOptions msWriteOptions = new MSWriteOptions
            {
                DeviceTimeToLive = writeOptions.DeviceTimeToLive.Ticks
            };

            MSDataStore.Create(partition, documentId, document.ToString(), msWriteOptions, (resultDoc) =>
            {
                DocumentWrapper<T> doc = new DocumentWrapper<T>
                {
                    Partition = resultDoc.Partition,
                    Id = resultDoc.DocumentId,
                    DeserializedValue = Document<T>.DeserializeString(resultDoc.DeserializedValue),
                    ETag = resultDoc.ETag,
                    LastUpdatedDate = (DateTime)resultDoc.LastUpdatedDate,
                    FromDeviceCache = resultDoc.FromDeviceCache,
                    Error = ConvertErrorToException(resultDoc.Error)
                };

                taskCompletionSource.TrySetResult(doc);
            });
            return taskCompletionSource.Task;
        }

        private static Task<DocumentWrapper<T>> PlatformReplace<T>(string partition, string documentId, T document)
        {
            var taskCompletionSource = new TaskCompletionSource<DocumentWrapper<T>>();
            MSDataStore.Replace(partition, documentId, document.ToString(), (resultDoc) =>
            {
                DocumentWrapper<T> doc = new DocumentWrapper<T>
                {
                    Partition = resultDoc.Partition,
                    Id = resultDoc.DocumentId,
                    DeserializedValue = Document<T>.DeserializeString(resultDoc.DeserializedValue),
                    ETag = resultDoc.ETag,
                    LastUpdatedDate = (DateTime)resultDoc.LastUpdatedDate,
                    FromDeviceCache = resultDoc.FromDeviceCache,
                    Error = ConvertErrorToException(resultDoc.Error)
                };

                taskCompletionSource.TrySetResult(doc);
            });
            return taskCompletionSource.Task;
        }

        private static Task<DocumentWrapper<T>> PlatformReplace<T>(string partition, string documentId, T document, WriteOptions writeOptions)
        {
            var taskCompletionSource = new TaskCompletionSource<DocumentWrapper<T>>();

            MSWriteOptions msWriteOptions = new MSWriteOptions
            {
                DeviceTimeToLive = writeOptions.DeviceTimeToLive.Ticks
            };

            MSDataStore.Replace(partition, documentId, document.ToString(), msWriteOptions, (resultDoc) =>
            {
                DocumentWrapper<T> doc = new DocumentWrapper<T>
                {
                    Partition = resultDoc.Partition,
                    Id = resultDoc.DocumentId,
                    DeserializedValue = Document<T>.DeserializeString(resultDoc.DeserializedValue),
                    ETag = resultDoc.ETag,
                    LastUpdatedDate = (DateTime)resultDoc.LastUpdatedDate,
                    FromDeviceCache = resultDoc.FromDeviceCache,
                    Error = ConvertErrorToException(resultDoc.Error)
                };

                taskCompletionSource.TrySetResult(doc);
            });
            return taskCompletionSource.Task;
        }

        private static Task<DocumentWrapper<T>> PlatformDelete<T>(string partition, string documentId)
        {
            var taskCompletionSource = new TaskCompletionSource<DocumentWrapper<T>>();
            MSDataStore.Delete(partition, documentId, (resultDoc) =>
            {
                DocumentWrapper<T> doc = new DocumentWrapper<T>
                {
                    Partition = resultDoc.Partition,
                    Id = resultDoc.DocumentId,
                    ETag = resultDoc.ETag,
                    LastUpdatedDate = (DateTime)resultDoc.LastUpdatedDate,
                    FromDeviceCache = resultDoc.FromDeviceCache,
                    Error = ConvertErrorToException(resultDoc.Error)
                };

                taskCompletionSource.TrySetResult(doc);
            });
            return taskCompletionSource.Task;
        }

        static DataException ConvertErrorToException(MSDataSourceError error) 
        {
            NSErrorException exception = new NSErrorException(error.Error);
            return new DataException(exception.Message, exception.InnerException);
        }
    }
}

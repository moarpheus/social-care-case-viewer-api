using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace SocialCareCaseViewerApi.V1.Gateways
{
    public interface IMongoGateway
    {
        public void DropCollection(string collectionName);
        public void InsertRecord<T>(string collectionName, T objToAdd);
        public void UpsertRecord<T>(string collectionName, Guid id, T record);
        public void DeleteRecordById<T>(string collectionName, Guid id);
        public List<T> LoadRecords<T>(string collectionName);
        public T LoadRecordById<T>(string collectionName, Guid id);
        public IEnumerable<T1> LoadMultipleRecordsByProperty<T1, T2>(string collectionName, string propertyName,
            T2 propertyValue);
        public T1 LoadRecordByProperty<T1, T2>(string collectionName, string propertyName, T2 propertyValue);
    }
}
